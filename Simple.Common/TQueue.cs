using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Simple.Common
{
    /// <summary>
    /// 队列类（使用循环数组）(加上线程安全)
    /// </summary>
    /// <typeparam name="T">队列中元素的类型</typeparam>
    public class TQueue<T>
    {
        /// <summary>
        /// 通知的状态机
        /// </summary>
        private readonly object notice = new object();

        /// <summary>
        /// 通知有数据进入
        /// </summary>
        public readonly AutoResetEvent EnqueueEvent = new AutoResetEvent(false);

        /// <summary>
        /// 循环数组，初始大小为100
        /// </summary>
        T[] ary = new T[100];
        /// <summary>
        /// 队头
        /// </summary>
        int front = 0;
        /// <summary>
        /// 队尾
        /// </summary>
        int rear = 0;
        /// <summary>
        /// 队大小
        /// </summary>
        int size = 0;
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="t">入队元素</param>
        /// <param name="userLock">用户自定义锁</param>
        public void Enqueue(T t, bool userLock = false)
        {
            if (!userLock)
            {
                Lock();
            }
            //如果队列大小等于数组长度，那么数组大小加倍
            if (size == ary.Length)
            {
                DoubleSize();
            }
            ary[rear] = t;
            //队尾前移
            rear++;
            //这一句是循环数组的关键：
            //如果rear超过数组下标了，
            //那么将从头开始使用数组。
            rear %= ary.Length;
            //大小加一
            size++;

            if (!userLock)
            {
                UnLock();
                EnqueueEvent.Set();
            }
        }
        /// <summary>
        /// 批量入队
        /// </summary>
        /// <param name="tarr"></param>
        public void EnqueueRange(IList<T> tarr)
        {
            if (!tarr.Any()) return;
            Lock();
            foreach (T t in tarr)
            {
                Enqueue(t, true);
            }
            UnLock();
            EnqueueEvent.Set();
        }

        /// <summary>
        /// 批量入队
        /// </summary>
        /// <param name="tarr"></param>
        public void EnqueueRange(IList<T> tarr, int offset, int count)
        {
            if (!tarr.Any()) return;
            if (offset < 0 || count < 0 || offset > tarr.Count - 1 || offset + count > tarr.Count) throw new ArgumentOutOfRangeException();

            Lock();
            for (var i = offset; i < offset + count; i++)
            {
                T t = tarr[i];
                Enqueue(t, true);
            }
            UnLock();
            EnqueueEvent.Set();
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns>出队元素</returns>
        public T Dequeue(bool userLock = false)
        {
            if (!userLock)
                Lock();

            //如果大小为零，那么队列已经空了
            if (size == 0)
            {
                if (!userLock)
                    UnLock();
                throw new Exception("队列已经空了");
            }
            T t = ary[front];
            //队头前移
            front++;
            //这一句是循环数组的关键：
            //如果front超过数组下标了，
            //那么将从头开始使用数组。
            front %= ary.Length;
            //大小减一
            size--;

            if (!userLock)
                UnLock();

            return t;
        }
        /// <summary>
        /// 批量出队
        /// </summary>
        public int DequeueRange(T[] tarr, int offset, int count)
        {
            int len = 0;
            if (tarr.Length == 0) return len;
            if (offset < 0 || count < 0 || offset > tarr.Length - 1 || offset + count > tarr.Length) throw new ArgumentOutOfRangeException();

            Lock();
            for (var i = offset; i < offset + count; i++)
            {
                try
                {
                    tarr[i] = Dequeue(true);
                    len++;
                }
                catch
                {
                    break;
                }
            }
            UnLock();

            return len;
        }
        /// <summary>
        /// 出队所有元素
        /// </summary>
        /// <returns></returns>
        public T[] DequeueAll()
        {
            Lock();
            if (size == 0)
            {
                UnLock();
                return new T[0];
            }
            T[] tary = new T[size];
            for (var i = 0; i < tary.Length; i++)
            {
                tary[i] = ary[front];
                //队头前移
                front++;
                //这一句是循环数组的关键：
                //如果front超过数组下标了，
                //那么将从头开始使用数组。
                front %= ary.Length;
                //大小减一
                size--;
            }
            UnLock();
            return tary;
        }
        /// <summary>
        /// 从队头提取一个元素（不删除）
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            //Lock();
            //如果大小为零，那么队列已经空了
            if (size == 0)
            {
                UnLock();
                throw new Exception("队列已经空了");
            }
            T t = ary[front];
            //UnLock();
            return t;
        }
        /// <summary>
        /// 查找队列指定元素位置
        /// </summary>
        /// <returns></returns>
        public int Find(T t)
        {
            //Lock();
            //如果大小为零，那么队列已经空了
            if (size == 0)
            {
                //UnLock();
                return -1;
            }
            for (int i = 0; i < size; i++)
            {
                int index = (i + front) % ary.Length;
                T tmp = ary[index];
                if (tmp.Equals(t))
                {
                    //UnLock();
                    return i;
                }
            }
            //UnLock();
            return -1;
        }
        /// <summary>
        /// 输出数组
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            Lock();
            T[] aryy = new T[size];
            for (int i = 0; i < size; i++)
            {
                int index = (i + front) % ary.Length;
                aryy[i] = ary[index];
            }
            UnLock();
            return aryy;
        }

        /// <summary>
        /// 队大小
        /// </summary>
        public int Count
        {
            get
            {
                //Lock();
                int result = size;
                //UnLock();
                return result;
            }
        }
        /// <summary>
        /// 清除队中的元素
        /// </summary>
        public void Clear()
        {
            Lock();
            size = 0;
            front = 0;
            rear = 0;
            ary = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            ary = new T[100];
            UnLock();
        }
        /// <summary>
        /// 数组大小加倍
        /// </summary>
        private void DoubleSize()
        {
            //临时数组
            T[] temp = new T[ary.Length];
            //将原始数组的内容拷贝到临时数组
            Array.Copy(ary, temp, ary.Length);
            //原始数组大小加倍
            ary = new T[ary.Length * 2];
            //将临时数组的内容拷贝到新数组中
            for (int i = 0; i < size; i++)
            {
                ary[i] = temp[front];
                front++;
                front %= temp.Length;
            }
            front = 0;
            rear = size;
        }
        /// <summary>
        /// 锁定
        /// </summary>
        public void Lock()
        {
            Monitor.Enter(notice);
        }
        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLock()
        {
            Monitor.Exit(notice);
        }
    }
}
