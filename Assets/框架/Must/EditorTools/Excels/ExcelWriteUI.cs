using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ToolEditor
{
    /// <summary>
    /// https://blog.csdn.net/Xz616/article/details/128893023
    /// https://www.cnblogs.com/noteswiki/p/6095868.html
    /// https://blog.csdn.net/shuaiLS/article/details/107369443
    /// </summary>
    public class ExcelWriteUI : EditorWindow
    {
        [MenuItem("Tools/编辑Excel#E #E")]
        public static void BuildPackageVersions()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWriteUI>())
                GetWindow(typeof(ExcelWriteUI), false, "Excel数据读取").Show();
            else
                GetWindow(typeof(ExcelWriteUI)).Close();
        }
        
        
        private static string _newLoadExcelPath = string.Empty; //新路径
        private Vector2 _scroll1, _scroll2 = Vector2.zero; //滑动
        private string[][] _data; //数据
        private readonly string _excelFolderPathKey = "Excel存放路径Key"; //Excel存放路径Key
        private readonly string _savePath = Application.dataPath.Replace("Assets", string.Empty); // 保存的路径
        private static ExcelWriteUI _excelWriteUI;

        [SerializeField, Header("滑动调节"), Tooltip("滑动调节值"), Range(50f, 100f)]
        public float slideBox = 100f;

        private float _deleteButSice = 40f;
        private bool _excelTitleLoading;
        private string _save1Path = string.Empty;
        private string _save2Path = string.Empty;
        private string _save3Path = string.Empty;
        private string _openFolderPath; //加载的文件夹路径
        private string _loadExcelPath; //加载的文件路径

        private bool _isOpen;

        #region 存读
        private void OnEnable() => Load();
        private void OnDisable() => Save();
        
        [MenuItem("Tools/保存数据 #S")]
        private void Save()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWriteUI>()) return;
            Type type = GetType();
            var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var data in fieldsValue)
            {
                if (data.FieldType == typeof(string))
                    PlayerPrefs.SetString($"{Application.productName}{data.Name}Save", (string)data.GetValue(this));
                if (data.FieldType == typeof(int))
                    PlayerPrefs.SetInt($"{Application.productName}{data.Name}Save", (int)data.GetValue(this));
            }

            Debug.LogError("保存成功");
        }
        private void Load()
        {
            Type type = GetType();
            var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var data in fieldsValue)
            {
                if (data.FieldType == typeof(string))
                    data.SetValue(this, PlayerPrefs.GetString($"{Application.productName}{data.Name}Save"));
                if (data.FieldType == typeof(int))
                    data.SetValue(this, PlayerPrefs.GetInt($"{Application.productName}{data.Name}Save"));
            }
        }
        #endregion
        

        private void LeftUI() 
        {
            if (string.IsNullOrEmpty(_openFolderPath)) return;
            if (!Directory.Exists(_openFolderPath)) _openFolderPath = string.Empty;
            //GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            _scroll1 = GUILayout.BeginScrollView(_scroll1, GUILayout.Height(Screen.height), GUILayout.Width(200));
            foreach (var path in Directory.GetFiles(_openFolderPath))
            {
                if (path.EndsWith("meta")) continue;
                var fileName = path.Split('\\')[^1];
                if (GUILayout.Button(fileName))
                {
                    _loadExcelPath = path;
                    _data = _loadExcelPath.LoadExcel();
                    _isOpen = true;
                }
            }

            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void RightUI()
        {
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("清空Unity日志", GUILayout.Width(100f))) ClearConsole();
                if (GUILayout.Button("保存路径", GUILayout.Width(100f))) Save();
                if (GUILayout.Button("选择文件夹路径...", GUILayout.Width(100f))) _openFolderPath = EditorUtility.OpenFolderPanel("选择文件夹", default, default); //打开文件夹
                if (GUILayout.Button("打开文件夹", GUILayout.Width(100f))) Process.Start(_openFolderPath);
                EditorGUILayout.LabelField("文件夹路径:", EditorStyles.label, GUILayout.Width(80));
                EditorGUILayout.TextField(_openFolderPath);
                EditorGUILayout.EndHorizontal();

                slideBox = (int)EditorGUILayout.Slider("滑动调节值", slideBox, 50f, 100f);
                _deleteButSice = (int)EditorGUILayout.Slider("滑动调节值", _deleteButSice, 40f, 80f);


                GUILayout.Space(5f);
                EditorGUILayout.BeginVertical();
                {
                    //二进制文件路径
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("选择路径", GUILayout.Width(100f))) _save1Path = EditorUtility.OpenFolderPanel("选择文件夹", default, default);
                    if (GUILayout.Button("打开二进制文件路径", GUILayout.Width(120f))) Process.Start(_save1Path);
                    _save1Path = EditorGUILayout.TextField(_save1Path);
                    EditorGUILayout.EndHorizontal();

                    //C#文件路径
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("选择路径", GUILayout.Width(100f))) _save2Path = EditorUtility.OpenFolderPanel("选择文件夹", default, default);
                    if (GUILayout.Button("打开C#文件路径", GUILayout.Width(120f))) Process.Start(_save2Path);
                    _save2Path = EditorGUILayout.TextField(_save2Path);
                    EditorGUILayout.EndHorizontal();

                    //Excel文件夹读取路径
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("选择路径", GUILayout.Width(100f))) _save3Path = EditorUtility.OpenFolderPanel("选择文件夹", default, default);
                    if (GUILayout.Button("打开Excel文件路径", GUILayout.Width(120f))) Process.Start(_save3Path);
                    _save3Path = EditorGUILayout.TextField(_save3Path);
                    EditorGUILayout.EndHorizontal();

                    if (_isOpen) RefreshExcelTitleData();
                }
                EditorGUILayout.EndVertical();
                if (_isOpen) RefreshExcelData();
            }
            EditorGUILayout.EndVertical();
        }

        private void OnGUI()
        {
            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.BeginHorizontal();
            LeftUI();
            RightUI();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 刷新Exce按钮数据
        /// </summary>
        private void RefreshExcelTitleData()
        {
            if (_loadExcelPath == string.Empty) return;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加数据", GUILayout.Width(80f)))
            {
                if (_data == null)
                    throw new Exception("请先读取数据");
                var strs = new string[_data[0].Length];
                var strings = _data.ToList();
                strings.Add(strs);
                _data = strings.ToArray();
            }

            if (GUILayout.Button("写入数据", GUILayout.Width(80f)))
            {
                var excelName = new FileInfo(_newLoadExcelPath);
                //通过ExcelPackage打开文件
                using var package = new ExcelPackage(excelName);
                //1表示第一个表
                var worksheet = package.Workbook.Worksheets[1];
                //Debug.Log(worksheet.Name);
                for (var i = 0; i < _data.Length; i++)
                {
                    var item1 = _data[i];
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                    for (var j = 0; j < item1.Length; j++)
                    {
                        var item2 = item1[j];
                        if (IsInteger(item2, out var number1))
                            worksheet.SetValue(i + 1, j + 1, number1);
                        else if (IsFloateger(item2, out var number2))
                            worksheet.SetValue(i + 1, j + 1, number2);
                        else if (string.IsNullOrEmpty(item2))
                            worksheet.SetValue(i + 1, j + 1, null);
                        else
                            worksheet.Cells[i + 1, j + 1].Value = item2;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                package.Save(); //储存
            }

            if (GUILayout.Button("Excel转换", GUILayout.Width(80f)))
                ExcelChange.GenerateExcelInfo(_loadExcelPath);
            if (GUILayout.Button("打开Excel", GUILayout.Width(80f)))
                Application.OpenURL(_loadExcelPath);
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// 写入数据
        /// </summary>
        private void WirteData(string[][] data, int row, int col, string excelPath)
        {
            data[row][col] = string.Empty;
            var excelName = new FileInfo(excelPath);
            //通过ExcelPackage打开文件
            using var package = new ExcelPackage(excelName);
            //1表示第一个表
            var worksheet = package.Workbook.Worksheets[1];
            for (var j = 0; j < _data[0].Length; j++)
                worksheet.SetValue(row + 1, col + 1, null);
            package.Save(); //储存
        }

        /// <summary>
        /// 刷新Excel数据
        /// </summary>
        private void RefreshExcelData()
        {
            if (_data == null || _data.Length == 0) return;
            var width = position.width - 210;
            const int num = 3;
            GUILayout.BeginScrollView(_scroll2, false, false,
                GUILayout.Width(width - 10f), GUILayout.Height(90f));
            for (var i = 0; i < num; i++)
            {
                var item1 = _data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                GUILayout.Space(45f);
                ShowData(i, item1);
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            //显示数据
            _scroll2 = GUILayout.BeginScrollView(_scroll2, false, false,
                GUILayout.Width(width), GUILayout.Height(position.height - 250f)); //
            for (var i = num; i < _data.Length; i++)
            {
                var item1 = _data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                if (GUILayout.Button("删除", GUILayout.Width(_deleteButSice)))
                {
                    //删除内存数据
                    var strings = _data.ToList();
                    strings.Remove(item1);
                    _data = strings.ToArray();
                    //删除实际数据
                    var excelName = new FileInfo(_newLoadExcelPath);
                    //通过ExcelPackage打开文件
                    using var package = new ExcelPackage(excelName);
                    var worksheet = package.Workbook.Worksheets[1];
                    for (var j = 0; j < _data[0].Length; j++)
                        worksheet.SetValue(i + 1, j + 1, null);
                    package.Save(); //储存


                    //EditorGUILayout.EndHorizontal();
                    //continue;
                }

                ShowData(i, item1);
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            return;

            void ShowData(int i, string[] data)
            {
                for (var j = 0; j < data.Length; j++)
                    _data[i][j] = EditorGUILayout.TextField(data[j], GUILayout.MinWidth(slideBox));
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        private static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var logEntries = assembly.GetType("UnityEditor.LogEntries");
            var clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod?.Invoke(new object(), null);
        }


        private bool IsInteger(string input, out int number)
        {
            //string pattern = @"^-?\d+$";  
            //return IsMatch(input, pattern);  
            if (int.TryParse(input, out int i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }

        private bool IsFloateger(string input, out float number)
        {
            if (float.TryParse(input, out float i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
    }
}