using System;
using System.IO;
using System.Reflection;

namespace Simple.Common
{
    public class ReflectHelper
    {
        /// <summary>
        /// 通过反射得到要获取的实例
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static Object GetInstance(string filePath, string className, params object[] args)
        {
            try
            {
                string oFilePath = filePath;
                if (!File.Exists(filePath))
                {
                    if (!string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.RelativeSearchPath))
                        filePath = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, oFilePath);
                    if (!File.Exists(filePath))
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oFilePath);
                    }
                }
                Assembly a = Assembly.LoadFrom(filePath);
                return GetInstance(className, a, args);
            }
            catch (Exception ex)
            {
                throw new Exception("反射方法GetInstance(string filePath, string className)出错！\n" + ex.Message);
            }
        }

        /// <summary>
        /// 通过反射得到要获取的实例
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static Object GetInstance(string className, Assembly a, params object[] args)
        {
            try
            {
                Type type = a.GetType(className, false);
                return Activator.CreateInstance(type, args);
            }
            catch (Exception ex)
            {
                throw new Exception("反射方法GetInstane(string className, System.Reflection.Assembly a)出错！\n" + ex.Message);
            }
        }
        /// <summary>
        /// 反射执行实例中的方法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public static object InvokeMethod(Assembly a, string className, string methodName, object[] methodParams)
        {
            try
            {
                object instance = GetInstance(className, a);
                Type t = instance.GetType();
                MethodInfo mi = t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                return mi.Invoke(instance, methodParams);
            }
            catch (Exception ex)
            {
                throw new Exception("InvokeMethod出错!\n" + ex.Message);
            }
        }
        /// <summary>
        /// 通过反射执行静态方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeStaticMethod(Type type, string methodName, object[] args)
        {
            try
            {
                return type.InvokeMember(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, args);
            }
            catch (Exception ex)
            {
                throw new Exception("反射方法InvokeStaticMethod(Type type, string methodName, object[] args)出错！\n" + ex.Message);
            }
        }
        /// <summary>
        /// 通过反射执行静态方法
        /// </summary>
        /// <param name="className"></param>
        /// <param name="a"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeStaticMethod(string className, Assembly a, string methodName, object[] args)
        {
            try
            {
                Type type = a.GetType(className, false);
                return InvokeStaticMethod(type, methodName, args);
            }
            catch (Exception ex)
            {
                throw new Exception("反射方法InvokeStaticMethod(string className, Assembly a, string methodName, object[] args)出错！\n" + ex.Message);
            }
        }
        /// <summary>
        /// 通过反射执行静态方法
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeStaticMethod(string filePath, string className, string methodName, object[] args)
        {
            try
            {
                string oFilePath = filePath;
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, oFilePath);
                    if (!File.Exists(filePath))
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oFilePath);
                    }
                }
                Assembly a = Assembly.LoadFrom(filePath);
                return InvokeStaticMethod(className, a, methodName, args);
            }
            catch (Exception ex)
            {
                throw new Exception("反射方法InvokeStaticMethod(string filePath, string className, string methodName, object[] args)出错！\n" + ex.Message);
            }
        }
    }
}
