using Simple.Common.SocketLib;
using System;
using System.Linq;

namespace Simple.Common.Cyhk
{
    public class Fields
    {
        public Field[] Fieldes { get; private set; }

        public byte[] Buffer { get; private set; }

        public Fields(Field[] fieldes, byte[] buffer)
        {
            this.Fieldes = fieldes;
            this.Buffer = buffer;
        }

        public T GetFieldValue<T>(string fieldName)
        {
            object val = GetFieldValue(fieldName);
          
            return val.GetValue<T>();
        }

        public object GetFieldValue(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException(nameof(fieldName));

            Field field = Fieldes.FirstOrDefault(f => f.Name == fieldName);
            if (field == null)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldName), "输入的字段名没有定义");
            }

            object value = null;

            byte[] valueBS = new byte[field.Range.Length];

            Array.Copy(Buffer, field.Range.MinIndex, valueBS, 0, valueBS.Length);

            if (field.ValueType == typeof(byte))    //byte类型
            {
                value = valueBS[0];
            }
            else if (field.ValueType == typeof(byte[])) //bytes数组
            {
                value = valueBS;
            }
            else if (field.ValueType == typeof(short))  //short类型
            {
                value = NetTools.NetworkToHostOrderFromBytesToShort(valueBS);
            }
            else if (field.ValueType == typeof(ushort))  //ushort类型
            {
                value = NetTools.NetworkToHostOrderFromBytesToUshort(valueBS);
            }
            else if (field.ValueType == typeof(int))   //int类型
            {
                value = NetTools.NetworkToHostOrderFromBytesToInt(valueBS);
            }
            else if (field.ValueType == typeof(uint))   //uint类型
            {
                value = NetTools.NetworkToHostOrderFromBytesToUint(valueBS);
            }
            else if (field.ValueType == typeof(string)) //string类型
            {
                value = NetTools.NetworkToHostOrderFromBytesToString(valueBS, 0, valueBS.Length);
            }
            else
            {
                throw new Exception("未解析数据类型");
            }

            return value;
        }
    }
}
