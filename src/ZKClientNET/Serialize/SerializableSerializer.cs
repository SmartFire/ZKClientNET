﻿using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZKClientNET.Serialize
{
    public class SerializableSerializer : IZKSerializer
    {
        public byte[] Serialize(object obj)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    return ms.GetBuffer();
                }
            }
            catch (IOException e)
            {
                throw;
            }
        }

        public object Deserialize(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    IFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(ms);
                }
            }
            catch (IOException e)
            {
                throw;
            }
        }
    }
}
