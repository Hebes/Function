using System;
using DG.Tweening.Plugins.Core.PathCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Farm2D
{
    /// <summary>
    /// https://blog.csdn.net/Xz616/article/details/128893023
    /// https://www.cnblogs.com/noteswiki/p/6095868.html
    /// https://blog.csdn.net/shuaiLS/article/details/107369443
    /// </summary>
    public class ExcelWrite : EditorWindow
    {
        private static string _loadExcelPath = string.Empty; //Excel读取路径
        private string _openFolderPath; //打开文件的路径
        private Vector2 _scrollPosition, _scrollExcel = Vector2.zero; //滑动
        private string[][] _data; //数据
        private string _message = string.Empty; //消息提示
        private readonly string _saveOpenFolderPathKey = "SaveOpenFolderPathKey"; //存储的字段
        private readonly string _saveLoadExcelPathKey = "SaveLoadExcelPathKey"; //存储的字段
        private static ExcelWrite _excelWrite;

        private void Awake()
        {
            excelWriteWindow ??= GetWindow<ExcelWrite>();
        }

        [MenuItem("Tools/编辑Excel#E #E")]
        public static void BuildPackageVersions()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWrite>())
            {
                //_excelWrite = this;
                //_editorWindow = GetWindow(typeof(ExcelWrite));
                _excelWrite = GetWindow<ExcelWrite>();
                _excelWrite.titleContent = new GUIContent("Excel数据读取");
                _excelWrite.Show();
                _excelWrite.Load();
                //GetWindow(typeof(ExcelWrite), false, ).Show();
            }
            else
            {
                GetWindow(typeof(ExcelWrite)).Close();
            }
        }

        private void OnDestroy()
        {
            _excelWrite.Save();
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private List<string> ReadFile(string filePath)
        {
            List<string> lineDataList = new List<string>();
            FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(fileStream);
            while (reader.ReadLine() is { } line)
                lineDataList.Add(line);
            reader.Close();
            fileStream.Close();
            return lineDataList;

            //byte[] buffer = new byte[fileStream.Length];
            //fileStream.Read(buffer, 0, buffer.Length);
            //string fileContent = System.Text.Encoding.UTF8.GetString(buffer);
            //fileContent.Split('\t');
            //Debug.Log(fileContent);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        private void OpenFilePanel()
        {
            string directory = EditorUtility.OpenFilePanel("选择文件", _loadExcelPath, "xls,xlsx,csv");
            if (!string.IsNullOrEmpty(directory))
                _loadExcelPath = directory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        private void GetFilesName(string dirPath)
        {
            string[] paths = Directory.GetFiles($"{Application.dataPath}/Editor/Excels");
        }


        /// <summary>
        /// 绘制 选择要分析的UI
        /// </summary>
        private void DrawSelectUI()
        {
            EditorGUILayout.Space();
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                Rect rect = hScope.rect;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUI.Box(rect, "");

                EditorGUILayout.LabelField("选择UI面板:", GUILayout.Width(100 / 4f));
            }
        }

        private static ExcelWrite excelWriteWindow = null;

        //视图宽度一半
        private float halfViewWidth;

        //视图高度一半
        private float halfViewHeight;
        private float tempValue = 210f;
        private float temp1Value;
        private float temp2Value;

        private void Save()
        {
            PlayerPrefs.SetString(_saveOpenFolderPathKey, _openFolderPath);
            PlayerPrefs.SetString(_saveLoadExcelPathKey, _loadExcelPath);
        }

        private void Load()
        {
            _openFolderPath = PlayerPrefs.GetString(_saveOpenFolderPathKey);
            _loadExcelPath = PlayerPrefs.GetString(_saveLoadExcelPathKey);
        }

        private void LeftUI()
        {
            if (string.IsNullOrEmpty(_openFolderPath)) return;
            if (!Directory.Exists(_openFolderPath)) _openFolderPath = string.Empty;
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            {
                //C:\Users\Eros\Desktop\Demo\Function\Assets\Excel工具\Editor\Excels
                _scrollExcel = GUILayout.BeginScrollView(_scrollExcel, GUILayout.Height(Screen.height), GUILayout.Width(200));
                string[] paths1 = Directory.GetFiles(_openFolderPath);
                foreach (var path in paths1)
                {
                    if (path.EndsWith("meta")) continue;
                    string[] strings = path.Split('\\');
                    string fileName = strings[^1];
                    if (GUILayout.Button(fileName))
                    {
                        _loadExcelPath = path;
                        ReadData();
                    }
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
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("加载Excel文件", EditorStyles.boldLabel, GUILayout.Width(80f));
                if (GUILayout.Button("清空保存数据", GUILayout.Width(90f)))
                    PlayerPrefs.DeleteAll();
                if (GUILayout.Button("打开文件夹...", GUILayout.Width(80f)))
                    _openFolderPath = EditorUtility.OpenFolderPanel("选择文件夹", default, default); //打开文件夹
                EditorGUILayout.LabelField("消息提示:", GUILayout.Width(80f));
                GUILayout.Space(5f);
                EditorGUILayout.LabelField(_message, EditorStyles.label);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("选择的文件夹路径:", EditorStyles.label, GUILayout.Width(100));
                _openFolderPath = EditorGUILayout.TextField(_openFolderPath);
                if (!Directory.Exists(_openFolderPath)) _openFolderPath = string.Empty;
                EditorGUILayout.EndHorizontal();

                if (!string.IsNullOrEmpty(_loadExcelPath))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("选择文件路径:", EditorStyles.label, GUILayout.Width(100));
                    _loadExcelPath = EditorGUILayout.TextField(_loadExcelPath);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("读取数据", GUILayout.Width(80f)))
                        ReadData();
                    if (GUILayout.Button("清除消息", GUILayout.Width(80f)))
                        _message = string.Empty;
                    if (GUILayout.Button("打开Excel", GUILayout.Width(80f)))
                        Application.OpenURL(_loadExcelPath);
                    if (GUILayout.Button("Excel转换", GUILayout.Width(80f)))
                    {
                        ExcelChange.GenerateExcelInfo();
                        _message = "转换成功";
                    }

                    if (GUILayout.Button("添加数据", GUILayout.Width(80f)))
                    {
                        if (_data == null)
                        {
                            _message = "请先读取数据";
                            return;
                        }

                        string[] strArray = new string[_data[0].Length];
                        List<string[]> strings = _data.ToList();
                        strings.Add(strArray);
                        _data = strings.ToArray();
                        _message = "数据添加成功";
                    }

                    if (GUILayout.Button("写入数据", GUILayout.Width(80f)))
                    {
                        FileInfo _excelName = new FileInfo(_loadExcelPath);
                        //通过ExcelPackage打开文件
                        using (ExcelPackage package = new ExcelPackage(_excelName))
                        {
                            //1表示第一个表
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                            //Debug.Log(worksheet.Name);
                            for (int i = 0; i < _data.Length; i++)
                            {
                                string[] item1 = _data[i];
                                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                                for (global::System.Int32 j = 0; j < item1.Length; j++)
                                {
                                    string item2 = item1[j];
                                    if (IsInteger(item2, out int number1))
                                        worksheet.SetValue(i + 1, j + 1, number1);
                                    else if (IsFloateger(item2, out float number2))
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

                        _message = "数据写入成功";
                    }

                    tempValue = EditorGUILayout.FloatField(tempValue, GUILayout.Width(100));
                    temp1Value = EditorGUILayout.FloatField(temp1Value, GUILayout.Width(100));
                    temp2Value = EditorGUILayout.FloatField(temp2Value, GUILayout.Width(100));

                    EditorGUILayout.EndHorizontal();

                    RefreshData();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnGUI()
        {
            // halfViewWidth = excelWriteWindow.position.width / 2f;
            // halfViewHeight = excelWriteWindow.position.height / 2f;
            GUI.backgroundColor = Color.yellow;

            EditorGUILayout.BeginHorizontal();
            LeftUI();
            RightUI();
            EditorGUILayout.EndHorizontal();
        }


        private void RefreshData()
        {
            if (_data == default) return;
            var width = position.width - tempValue;
            using (new EditorGUILayout.ScrollViewScope(new Vector2(_scrollPosition.x, 0f),
                       GUILayout.Height(65f), GUILayout.Width(width)))
            {
                for (var i = 0; i < 3; i++)
                {
                    var item1 = _data[i];
                    EditorGUILayout.BeginHorizontal(); //GUILayout.Width(position.width)
                    GUILayout.Space(85f);
                    ShowData(i, item1);
                    EditorGUILayout.EndHorizontal();
                }
            }

            using (var scrollViewScope = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(65f),GUILayout.Width(width)))
            {
                _scrollPosition = scrollViewScope.scrollPosition;

                for (int i = 3; i < _data.Length; i++)
                {
                    string[] item1 = _data[i];
                    EditorGUILayout.BeginHorizontal(); //GUILayout.Width(position.width)
                    if (GUILayout.Button("删除", GUILayout.Width(80f)))
                    {
                        //删除内存数据
                        List<string[]> strings = _data.ToList();
                        strings.Remove(item1);
                        _data = strings.ToArray();
                        //删除实际数据
                        int deleteNumber = i;
                        FileInfo _excelName = new FileInfo(_loadExcelPath);
                        //通过ExcelPackage打开文件
                        using (ExcelPackage package = new ExcelPackage(_excelName))
                        {
                            //1表示第一个表
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                            for (int j = 0; j < _data[0].Length; j++)
                                worksheet.SetValue(deleteNumber + 1, j + 1, null);
                            package.Save(); //储存
                        }

                        _message = "数据删除成功";
                    }

                    ShowData(i, item1);
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(temp1Value);
        }

        private void ReadData()
        {
            _data = _loadExcelPath.LoadExcel(); //读取Excel数据
            _message = "数据读取成功";
        }


        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="i"></param>
        /// <param name="dataArray"></param>
        private void ShowData(int i, string[] dataArray)
        {
            for (var j = 0; j < dataArray.Length; j++)
            {
                var data = dataArray[j];
                _data[i][j] = EditorGUILayout.TextField(data);
            }
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