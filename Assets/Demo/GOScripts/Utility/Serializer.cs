using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Serializer 
{
    public static object Deserialize<T>(byte[] data) where T:new()
    {
        T result = new T();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter converter = new BinaryFormatter();
            memoryStream.Write(data, 0, data.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var obj = converter.Deserialize(memoryStream);
            return obj;
        }
    }

    public static byte[] Serialize<T>(object customType)
    {
        T c = (T)customType;
        BinaryFormatter converter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            converter.Serialize(memoryStream, c);
            return memoryStream.ToArray();
        }
    }

    internal static T NewInstance<T>(object dataObject)
    {
        return (T)Activator.CreateInstance(Type.GetType(dataObject.GetType().Name.Remove(dataObject.GetType().Name.Length - 5)));
    }
}
