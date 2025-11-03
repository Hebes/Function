using System.IO;
using System.Text;
using UnityEditor;

namespace ToolEditor
{
    /// <summary>
    /// 数据结构类
    /// </summary>
    public class ClassData
    {
        public const string CSharpSavePathKey = "C#文件保存路径";

        /// <summary>
        /// 放置要生成的C#文件的路径
        /// </summary>
        public static string CSharpSavePath = string.Empty;

        /// <summary>
        /// 通过Excel数据生成脚本文件
        /// </summary>
        public static void CreateScript(string filePath, string[][] data)
        {
            // StringBuilder sb = new StringBuilder();
            // string className = new FileInfo(filePath).Name.Split('.')[0];
            // sb.AppendLine("using System.Collections.Generic;");
            // sb.AppendLine("using Core;");
            // sb.AppendLine("using System;\n\t");
            // sb.AppendLine($"[Serializable]");
            // sb.AppendLine($"public class {className} : IData");
            // sb.AppendLine("{");
            // string[] filedTypeArray = data[(int)RowType.FIELD_TYPE];
            // string[] filedNameArray = data[(int)RowType.FIELD_NAME];
            // for (int i = 0; i < filedTypeArray.Length; ++i)
            // {
            //     sb.AppendLine($"\tpublic {filedTypeArray[i].PadRight(10, ' ')}\t{filedNameArray[i]};");
            // }
            // sb.AppendLine($"    public int GetId()\r\n    {{\r\n\t\treturn {data[0][0]};\r\n    }}");
            // sb.AppendLine("}");
            // CSharpSavePath.GenerateDirectory();
            // string path = $"{CSharpSavePath}/{className}.cs";
            // File.Delete(path);
            // File.WriteAllText(path, sb.ToString());
            // AssetDatabase.Refresh();
        }
    }
}
