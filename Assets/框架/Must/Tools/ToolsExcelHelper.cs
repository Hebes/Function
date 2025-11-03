using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

public static partial class Tools
{
    /// <summary>
    /// 读取Excel
    /// ReadExcel1($"{Application.dataPath}/1.xlsx", "sheet1");
    /// XML DOM ： https://www.runoob.com/dom/dom-element.html
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="worksheetsName"></param>
    /// <exception cref="Exception"></exception>
    private static string[][] ReadExcel1(string filePath, string worksheetsName)
    {
        //读取ZIP文档
        using FileStream stream = File.OpenRead(filePath);
        using ZipArchive archive = new System.IO.Compression.ZipArchive(stream, System.IO.Compression.ZipArchiveMode.Read); // 打开 ZIP 存档（XLSX 文件实际上是一个 ZIP 文件）

        //加载指定工作表
        ZipArchiveEntry worksheetEntry = archive.GetEntry($"xl/worksheets/{worksheetsName}.xml");
        if (worksheetEntry == null)
            throw new Exception("没有这个工作表");
        using StreamReader reader1 = new StreamReader(worksheetEntry.Open(), Encoding.UTF8);
        XmlDocument doc1 = new XmlDocument();
        doc1.Load(reader1);

        //读取共享字符串
        ZipArchiveEntry sharedStringEntry = archive.GetEntry("xl/sharedStrings.xml");
        string[] sharedStrings2 = Array.Empty<string>();
        if (sharedStringEntry != null)
        {
            using StreamReader reader2 = new StreamReader(sharedStringEntry.Open(), Encoding.UTF8);
            XmlDocument doc2 = new XmlDocument();
            doc2.Load(reader2);
            XmlNodeList stringNodes = doc2.GetElementsByTagName("si"); // 获取所有共享字符串
            sharedStrings2 = new string[stringNodes.Count];
            for (int i = 0; i < stringNodes.Count; i++)
                sharedStrings2[i] = stringNodes[i].InnerText;
        }


        // 获取内容
        XmlNodeList rows = doc1.GetElementsByTagName("row");
        string[][] temp = new string[rows.Count][];

        for (int i = 0; i < rows.Count; i++)
        {
            XmlNode row = rows[i];
            string[] temp2 = new string[row.ChildNodes.Count];
            for (int j = 0; j < row.ChildNodes.Count; j++)
            {
                XmlNode cell = row.ChildNodes[j];
                string cellValue = GetCellValue(cell, sharedStrings2); // 处理单元格类型，如果需要，可以根据类型进行转换  https://zhuanlan.zhihu.com/p/386114685
                temp2[j] = cellValue;
                //Debug.Log(cellValue + "\t");
            }

            temp[i] = temp2;
        }

        return temp;

        //另外一种获取方式
        // foreach (XmlNode row in rows)
        // {
        //     foreach (XmlNode cell in row.ChildNodes)
        //     {
        //         // 处理单元格类型，如果需要，可以根据类型进行转换  https://zhuanlan.zhihu.com/p/386114685
        //         string cellValue = GetCellValue(cell, sharedStrings2);
        //         Debug.Log(cellValue + "\t");
        //     }
        // }

        string GetCellValue(XmlNode cell, string[] sharedStrings)
        {
            string cellValue = string.Empty;

            if (cell["v"] != null)
            {
                string value = cell["v"].InnerText;

                // 处理共享字符串  Attributes节点属性  cell.Name节点名称  cell.InnerText节点值
                if (cell.Attributes["t"] != null && cell.Attributes["t"].Value == "s")
                {
                    int index = int.Parse(value);
                    cellValue = sharedStrings[index]; // 获取共享字符串的值
                }
                else
                {
                    cellValue = value;
                }
            }

            return cellValue;
        }
    }
}