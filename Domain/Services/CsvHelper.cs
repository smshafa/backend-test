﻿using System.Reflection;

namespace backend_test.Domain.Services;

public class CsvHelper<T> : IWriter<T>
{
    private static void CreateHeader<T>(List<T> list, StreamWriter sw)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length - 1; i++)
        {
            sw.Write(properties[i].Name + ",");
        }

        var lastProp = properties[properties.Length - 1].Name;
        sw.Write(lastProp + sw.NewLine);
    }

    private static void CreateRows<T>(List<T> list, StreamWriter sw)
    {
        foreach (var item in list)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                var prop = properties[i];
                sw.Write(prop.GetValue(item) + ",");
            }

            var lastProp = properties[properties.Length - 1];
            sw.Write(lastProp.GetValue(item) + sw.NewLine);
        }
    }

    public void WriteToFile(List<T> list, string filePath, string fileName)
    {
        using (StreamWriter sw = new StreamWriter(filePath + "\\" + fileName + ".csv"))
        {
            CreateHeader(list, sw);
            CreateRows(list, sw);
        }
    }
}