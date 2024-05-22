using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Excel;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 检查枚举类型和表格对应的枚举类型是否对应不上
/// </summary>
public class CheckExcelExcel : EditorWindow
{
    private string LoadExcelPath { get; set; }
    private string LoadFolderPath { get; set; }
    private string LoadExcelFolderPath { get; set; }
    private string _message = string.Empty; //消息提示
    private string _enumType = string.Empty;
    private string[][] _excelData = null; //数据
    private Vector2 scrollPosition = Vector2.zero; //滑动
    private Vector2 scrollPosition1 = Vector2.zero; //滑动
    private int col = 4; //列
    private int row = 3; //行


    [MenuItem("Tools/检查Excel表个执行的行的枚举和脚本的枚举是否可以相对应#E #E")]
    public static void BuildPackageVersions()
    {
        if (!EditorWindow.HasOpenInstances<CheckExcelExcel>())
            GetWindow(typeof(CheckExcelExcel), false, "Excel数据填充").Show();
        else
            GetWindow(typeof(CheckExcelExcel)).Close();
    }

    private void OnGUI()
    {
        GUILayout.Space(5f);
        GUI.backgroundColor = Color.yellow;
        EditorGUILayout.LabelField("消息提示:", GUILayout.Width(80f));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择的文件路径:", EditorStyles.label, GUILayout.Width(130f));
        LoadExcelPath = EditorGUILayout.TextField(LoadExcelPath, GUILayout.Width(700f));
        EditorGUILayout.LabelField(_message, EditorStyles.label);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择的文件夹路径:", GUILayout.Width(130f));
        LoadFolderPath = EditorGUILayout.TextField(LoadFolderPath, GUILayout.Width(700f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("请输入枚举类型:", GUILayout.Width(130f));
        _enumType = EditorGUILayout.TextField(_enumType, GUILayout.Width(700f));
        if (GUILayout.Button("获取枚举类型", GUILayout.Width(80f)))
            _enumType = GetEnemType(LoadExcelPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        col = EditorGUILayout.IntField("需要获取的列(第一格未0):", col);
        row = EditorGUILayout.IntField("需要获取的开始的行(第一格未0):", row);
        // if (GUILayout.Button("开始获取", GUILayout.Width(80f)))
        //    GetExcelCol(col, row, _excelData);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打开Excel", GUILayout.Width(80f)))
            Application.OpenURL(LoadExcelPath);
        if (GUILayout.Button("打开Excel文件夹", GUILayout.Width(150f)))
            LoadExcelFolderPath = OpenFolderPanel();
        // if (GUILayout.Button("打开文件...", GUILayout.Width(100f)))
        // {
        //     LoadExcelPath = OpenFilePanel();
        //     _excelData = LoadExcelData(LoadExcelPath);
        //     _enumType = GetEnemType(LoadExcelPath);
        // }
        if (GUILayout.Button("打开动画文件夹...", GUILayout.Width(150f)))
            LoadFolderPath = OpenFolderPanel();
        if (GUILayout.Button("清除消息", GUILayout.Width(80f)))
            _message = string.Empty;
        if (GUILayout.Button("读取数据", GUILayout.Width(80f)))
            _excelData = LoadExcelData(LoadExcelPath);
        if (GUILayout.Button("开始比较", GUILayout.Width(80f)))
        {
            List<string> t1 = GetExcelCol(col, row, _excelData);
            List<string> t2 = GetEnumType(_enumType + "Type");
            if (string.IsNullOrEmpty(LoadFolderPath))
                Debug.LogError("请选择一个动画文件夹");
            string[] t3 = Directory.GetFiles(LoadFolderPath, "*.lf", SearchOption.AllDirectories);

            List<(string, string, string)> strList = new List<(string, string, string)>();

            var commonTemp = _enumType.Split('_');
            var commonTempStr = commonTemp[1];

            //第一次excel
            foreach (var t1Data in t1)
            {
                if (t1Data.EndsWith("_reverse"))continue;
                var str1 = $"{commonTempStr}_{t1Data}";
                strList.Add((str1.ToLower(), string.Empty, string.Empty));
            }

            //第二次枚举类型
            for (var i = 0; i < strList.Count; i++)
            {
                var sTuple = strList[i];
                foreach (var t2Data in t2)
                {
                    var str2 = $"{commonTempStr}_{t2Data}";
                    var changeValue = str2.ToLower();
                    if (!sTuple.Item1.Equals(changeValue)) continue;
                    sTuple.Item2 = changeValue;
                    strList[i] = sTuple;
                }
            }

            foreach (var t3Data in t3)
            {
                //去掉多余的
                string[] strArray = t3Data.Split('\\');
                string newStr = strArray[strArray.Length - 1].Replace(".lf", string.Empty);
                string str3 = $"{newStr.ToLower()}";
                //第三次
                for (var i = 0; i < strList.Count; i++)
                {
                    var sTuple = strList[i];
                    if (!sTuple.Item1.Equals(str3)) continue;
                    sTuple.Item3 = str3;
                    strList[i] = sTuple;
                }
            }

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            //检查
            foreach (var sTuple in strList)
            {
                if (string.IsNullOrEmpty(sTuple.Item2))
                    sb1.AppendLine($"当前表格中 {sTuple.Item1} 没有枚举值");
                if (string.IsNullOrEmpty(sTuple.Item3))
                    sb2.AppendLine($"当前表格中 {sTuple.Item1} 没有动画");
            }

            if (sb1.Length > 0)
                Debug.LogError(sb1.ToString());
            if (sb2.Length > 0)
                Debug.LogError(sb2.ToString());
        }

        EditorGUILayout.EndHorizontal();
        ReExcel();
        RefreshData();
    }

    private void RefreshData()
    {
        if (_excelData == null) return;
        var segmentation = 4;
        for (int i = 0; i < segmentation; i++)
        {
            string[] item1 = _excelData[i];
            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < item1.Length; j++)
            {
                string item2 = item1[j];
                _excelData[i][j] = EditorGUILayout.TextField(item2);
            }

            EditorGUILayout.EndHorizontal();
        }

        //显示数据
        scrollPosition = GUILayout.BeginScrollView(scrollPosition); //GUILayout.Width(400), GUILayout.Height(500)
        for (var i = segmentation; i < _excelData.Length; i++)
        {
            string[] item1 = _excelData[i];
            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < item1.Length; j++)
            {
                string item2 = item1[j];
                _excelData[i][j] = EditorGUILayout.TextField(item2);
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private void ReExcel()
    {
        if (!Directory.Exists(LoadExcelFolderPath)) return;
        if (string.IsNullOrEmpty(LoadExcelFolderPath)) return;
        string[] t4 = Directory.GetFiles(LoadExcelFolderPath, "*.xlsx", SearchOption.AllDirectories);
        if (t4.Length == 0) return;
        scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1, GUILayout.Height(40f)); //, GUILayout.Height(500)
        EditorGUILayout.BeginHorizontal();
        foreach (var t4Data in t4)
        {
            //去掉多余的
            string[] strArray = t4Data.Split('\\');
            string str3 = strArray[strArray.Length - 1].Replace(".xlsx", string.Empty);
            float textWidth = GUI.skin.label.CalcSize(new GUIContent(str3)).x;
            if (GUILayout.Button(str3, GUILayout.Width(textWidth + 10f))) //GUILayout.Width(150f)
            {
                LoadExcelPath = t4Data;
                _excelData = LoadExcelData(LoadExcelPath);
                _enumType = GetEnemType(LoadExcelPath);
            }
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private string GetEnemType(string str)
    {
        var temp1 = str.Split('\\');
        return temp1[temp1.Length - 1].Replace(".xlsx", string.Empty);
    }

    /// <summary>
    /// 读取Excel数据并保存为字符串锯齿数组(如果读取不出数据，请把ICSharpCode.SharpZipLib.dll也加入进来)
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private string[][] LoadExcelData(string filePath)
    {
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        DataSet dataSet = fileInfo.Extension == ".xlsx"
            ? ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet()
            : ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet();

        DataRowCollection rows = dataSet.Tables[0].Rows;
        string[][] data = new string[rows.Count][];
        for (var i = 0; i < rows.Count; ++i)
        {
            var columnCount = rows[i].ItemArray.Length;
            string[] columnArray = new string[columnCount];
            for (var j = 0; j < columnArray.Length; ++j)
                columnArray[j] = rows[i].ItemArray[j].ToString();
            data[i] = columnArray;
        }

        stream.Close();

        return data;
    }

    /// <summary>
    /// 通过反射获取执行的枚举类型
    /// </summary>
    /// <param name="enumName"></param>
    /// <param name="assemblyName"></param>
    private List<string> GetEnumType(string enumName, string assemblyName = "Assembly-CSharp")
    {
        List<string> strList = new List<string>();
        Assembly assembly = Assembly.Load(assemblyName);
        Type[] allType = assembly.GetTypes();
        foreach (var type in allType)
        {
            if (!type.IsEnum) continue;
            if (type.FullName == default) continue;
            if (!type.FullName.Equals(enumName)) continue;

            //把反射的枚举转成字符串添加
            Array ttt = Enum.GetValues(type);
            foreach (var data in ttt)
            {
                //Debug.LogError(data.ToString());
                strList.Add(data.ToString());
            }
        }

        return strList;
    }

    /// 获取指定的列
    /// </summary>
    /// <param name="rowStart">开始读取的行</param>
    /// <param name="colStart">开始读取的纵列</param>
    /// <param name="dataValue">数据</param>
    /// <returns></returns>
    private List<string> GetExcelCol(int rowStart, int colStart, string[][] dataValue)
    {
        List<string> strList = new List<string>();

        for (int i = 0; i < dataValue.Length; i++)
        {
            if (i < rowStart) continue;
            string[] rowValue = dataValue[i];
            for (int j = 0; j < rowValue.Length; j++)
            {
                if (j < colStart) continue;
                string str = dataValue[i][j];
                strList.Add(str);
                break;
            }
        }

        return strList;
    }


    /// <summary>
    /// 打开文件
    /// </summary>
    private string OpenFilePanel() => EditorUtility.OpenFilePanel("选择文件", LoadExcelPath, "xls,xlsx,csv");

    /// <summary>
    /// 打开文件夹
    /// </summary>
    /// <returns></returns>
    private string OpenFolderPanel() => EditorUtility.OpenFolderPanel("选择文件夹", "", ""); //Select Folder
}