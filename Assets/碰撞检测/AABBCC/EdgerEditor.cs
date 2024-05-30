// using UnityEngine;
// using System.Collections;
// using System.Text;
// using UnityEditor;
//
// [CustomEditor(typeof(Edger))]
// public class EdgerEditor : UnityEditor.Editor
// {
//
// 	Edger edger = null;
//
// 	public override void OnInspectorGUI()
// 	{
// 		edger = (Edger)target;
//
// 		base.OnInspectorGUI();
//
// 		//if (GUILayout.Button("从表中读取"))  //  按钮  
// 		//{
// 		//	ReadFromTable();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("去冗余"))  //  按钮  
// 		//{
// 		//	Del();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("平滑"))  //  按钮  
// 		//{
// 		//	Smooth();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("选择所有子节点"))  //  按钮  
// 		//{
// 		//	SelectChild();
// 		//	return;
// 		//}
//
// 		//EditorGUILayout.Separator();
//
// 		//if (GUILayout.Button("注册选择事件"))  //  按钮  
// 		//{
// 		//	RegisterSelectionChanged();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("移除选择事件"))  //  按钮  
// 		//{
// 		//	UnregisterSelectionChanged();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("打印所有位置"))
// 		//{
// 		//	PrintAllPositions();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("打印所有索引"))  //  按钮  
// 		//{
// 		//	PrintAllIndex();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("打印所有区域索引"))
// 		//{
// 		//	PrintAllAreaIndex();
// 		//	return;
// 		//}
//
// 		//if (GUILayout.Button("重新填充所有点"))
// 		//{
// 		//	edger.RefillAllPoints();
// 		//	return;
// 		//}
//
// 		EditorGUILayout.Separator();
//
// 		if (GUILayout.Button("根据Nvamesh生成边界点"))  //  按钮  
// 		{
// 			GeneratePoints();
//
// 			edger.Smooth();
//
// 			return;
// 		}
//
// 		if (GUILayout.Button("输出配置"))
// 		{
// 			PrintConfig();
//
// 			return;
// 		}
// 		if (GUILayout.Button("删除边界点"))
// 		{
// 			DelPoints();
//
// 			return;
// 		}
// 	}
//
// 	private void ReadFromTable()
// 	{
// 		if (edger != null)
// 		{
// 			edger.ReadFromTable();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
// 	private void Del()
// 	{
// 		if (edger != null)
// 		{
// 			edger.Del();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
// 	private void Smooth()
// 	{
// 		if (edger != null)
// 		{
// 			edger.Smooth();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
//
// 	private void SelectChild()
// 	{
// 		if (edger != null)
// 		{
// 			edger.SelectChild();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
//
// 	private void GeneratePoints()
// 	{
// 		if (edger != null)
// 		{
// 			edger.GeneratePoints();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
// 	private void DelPoints()
// 	{
// 		if (edger != null)
// 		{
// 			edger.DelSpheres();
// 		}
// 		else
// 		{
// 			Debug.LogError("edge 为null");
// 		}
// 	}
//
// 	//[ContextMenu("注册选择事件")]
// 	private void RegisterSelectionChanged()
// 	{
// 		Selection.selectionChanged = OnSelectionChanged;
// 	}
//
// 	//[ContextMenu("移除选择事件")]
// 	private void UnregisterSelectionChanged()
// 	{
// 		Selection.selectionChanged = null;
// 	}
//
// 	private void OnSelectionChanged()
// 	{
// 		var selectedSpheres = Selection.gameObjects;
//
// 		for (var i = 0; i < selectedSpheres.Length; i++)
// 		{
// 			var sphere = selectedSpheres[i];
//
// 			if (edger.allPoints.Contains(sphere))
// 			{
// 				continue;
// 			}
//
// 			edger.allPoints.Add(sphere);
// 		}
// 	}
//
// 	private void PrintAllPositions()
// 	{
// 		edger.Smooth();
//
// 		var result = new StringBuilder();
//
// 		for (int i = 0; i < edger.allPoints.Count; i++)
// 		{
// 			if (edger.allPoints[i] == null)
// 			{
// 				break;
// 			}
//
// 			result.AppendFormat("{0};", edger.allPoints[i].transform.position.x.ToString("f2"));
// 			result.AppendFormat("{0};", edger.allPoints[i].transform.position.z.ToString("f2"));
// 		}
// 		Debug.LogError(result);
//
// 		EditorGUIUtility.systemCopyBuffer = result.ToString();
// 	}
//
// 	private void PrintAllIndex()
// 	{
// 		var result = new StringBuilder();
//
// 		for (int i = 0; i < edger.allPoints.Count; i++)
// 		{
// 			result.AppendFormat("{0};", edger.FindShpereIndex(edger.allPoints[i]));
// 		}
//
// 		Debug.LogError(result.ToString());
//
// 		EditorGUIUtility.systemCopyBuffer = result.ToString();
// 	}
//
// 	private void PrintAllAreaIndex()
// 	{
// 		var result = new StringBuilder();
//
// 		for (int i = 0; i < edger.allAreas.Count; i++)
// 		{
// 			var area = edger.allAreas[i];
//
// 			result.AppendFormat("{0};", area.points.Count);
//
// 			for (var j = 0; j < area.points.Count; j++)
// 			{
// 				var point = area.points[j];
//
// 				result.AppendFormat("{0};", edger.FindShpereIndex(point));
// 			}
// 		}
//
// 		Debug.LogError(result.ToString());
//
// 		EditorGUIUtility.systemCopyBuffer = result.ToString();
// 	}
//
// 	private void PrintConfig()
// 	{
// 		var result = new StringBuilder();
//
// 		for (int i = 0; i < edger.allPoints.Count; i++)
// 		{
// 			if (edger.allPoints[i] == null)
// 			{
// 				break;
// 			}
//
// 			result.AppendFormat("{0};", edger.allPoints[i].transform.position.x.ToString("f2"));
// 			result.AppendFormat("{0};", edger.allPoints[i].transform.position.z.ToString("f2"));
// 		}
//
// 		result.Append("\t");
//
// 		for (int i = 0; i < edger.allAreas.Count; i++)
// 		{
// 			var area = edger.allAreas[i];
//
// 			result.AppendFormat("{0};", area.points.Count);
//
// 			for (var j = 0; j < area.points.Count; j++)
// 			{
// 				var point = area.points[j];
//
// 				result.AppendFormat("{0};", edger.FindShpereIndex(point));
// 			}
// 		}
//
// 		result.Append("\t");
//
// 		if (edger.stage01ToStage02Gate.Count > 1)
// 		{
// 			for (int i = 0; i < edger.stage01ToStage02Gate.Count; i++)
// 			{
// 				result.AppendFormat("{0};", edger.FindShpereIndex(edger.stage01ToStage02Gate[i]));
// 			}
// 		}
// 		else
// 		{
// 			result.Append("0");
// 		}
//
// 		result.Append("\t");
//
// 		if (edger.stage02ToStage03Gate.Count > 1)
// 		{
// 			for (int i = 0; i < edger.stage02ToStage03Gate.Count; i++)
// 			{
// 				result.AppendFormat("{0};", edger.FindShpereIndex(edger.stage02ToStage03Gate[i]));
// 			}
// 		}
// 		else
// 		{
// 			result.Append("0");
// 		}
//
// 		result.Append("\t");
//
// 		if (edger.stage01Split.Count > 1)
// 		{
// 			for (int i = 0; i < edger.stage01Split.Count; i++)
// 			{
// 				result.AppendFormat("{0};", edger.FindShpereIndex(edger.stage01Split[i]));
// 			}
// 		}
// 		else
// 		{
// 			result.Append("0");
// 		}
//
// 		result.Append("\t");
//
// 		if (edger.stage02Split.Count > 1)
// 		{
// 			for (int i = 0; i < edger.stage02Split.Count; i++)
// 			{
// 				result.AppendFormat("{0};", edger.FindShpereIndex(edger.stage02Split[i]));
// 			}
// 		}
// 		else
// 		{
// 			result.Append("0");
// 		}
//
// 		result.Append("\t");
//
// 		if (edger.stage03Split.Count > 1)
// 		{
// 			for (int i = 0; i < edger.stage03Split.Count; i++)
// 			{
// 				result.AppendFormat("{0};", edger.FindShpereIndex(edger.stage03Split[i]));
// 			}
// 		}
// 		else
// 		{
// 			result.Append("0");
// 		}
//
// 		Debug.LogError(result);
//
// 		EditorGUIUtility.systemCopyBuffer = result.ToString();
// 	}
// }
