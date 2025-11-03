using System.IO;
using UnityEditor;
using ExcelDataReader;

/*--------脚本描述-----------
				
描述:
    表格转换帮助类

-----------------------*/

namespace ToolEditor
{
    public static class ExcelChangeHelper
    {
       /// <summary>
       /// 读取Excel数据并保存为字符串锯齿数组
       /// </summary>
       /// <param name="filePath"></param>
       /// <returns></returns>
        public static string[][] LoadExcel(this string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var dataSet = fileInfo.Extension == ".xlsx"
                ? ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet()
                : ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet();

            var rows = dataSet.Tables[0].Rows;
            var data = new string[rows.Count][];
            for (var i = 0; i < rows.Count; ++i)
            {
                var columnCount = rows[i].ItemArray.Length;
                var columnArray = new string[columnCount];
                for (var j = 0; j < columnArray.Length; ++j)
                    columnArray[j] = rows[i].ItemArray[j].ToString();
                data[i] = columnArray;
            }
            stream.Close();

            return data;
        }
    }
}
