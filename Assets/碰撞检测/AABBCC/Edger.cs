// using System;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using Game;
// using UnityEngine.SceneManagement;
// using zhaorh.Excel;
//
// public enum SceneEdgeType
// {
// 	stage01_edges,
// 	stage01_island,
// 	stage01_2_stage02,
// 	stage02_edges,
// 	stage02_island,
// 	stage02_2_stage03,
// 	stage03_edges,
// 	stage03_island
// }
//
// [ExecuteInEditMode]
// public class Edger : MonoBehaviour
// {
// 	public string levelName;
//
// 	public SceneEdgeType type;
//
// 	public void Start()
// 	{
// 		levelName = SceneManager.GetActiveScene().name;
//
// 		type = SceneEdgeType.stage01_edges;
// 	}
//
// 	[SerializeField]
// 	public List<GameObject> allPoints = new List<GameObject>();
//
// 	[SerializeField]
// 	public List<Area> allAreas = new List<Area>();
//
// 	public List<GameObject> stage01ToStage02Gate = new List<GameObject>();
//
// 	public List<GameObject> stage02ToStage03Gate = new List<GameObject>();
//
// 	public List<GameObject> stage01Split = new List<GameObject>();
//
// 	public List<GameObject> stage02Split = new List<GameObject>();
//
// 	public List<GameObject> stage03Split = new List<GameObject>();
//
// 	//[ContextMenu("从表中读取")]
// 	public void ReadFromTable()
// 	{
// 		if (string.IsNullOrEmpty(levelName))
// 		{
// 			Debug.LogError("错误");
// 			return;
// 		}
//
// 		List<Table_TRes_Levelinfo> tableItems = Table_TRes_Levelinfo.GetData().Values.ToList();
//
// 		Table_TRes_Levelinfo item = null;
//
// 		for (int i = 0; i < tableItems.Count; i++)
// 		{
// 			if (tableItems[i].level_name == levelName)
// 			{
// 				item = tableItems[i];
// 			}
// 		}
//
// 		if (item == null)
// 		{
// 			Debug.LogError("没有" + levelName);
// 			return;
// 		}
//
// 		List<float> positions = null;
//
// 		switch (type)
// 		{
// 			case SceneEdgeType.stage01_edges:
// 				positions = item.stage01_edges;
// 				break;
// 			case SceneEdgeType.stage01_island:
// 				positions = item.stage01_island;
// 				break;
// 			case SceneEdgeType.stage01_2_stage02:
// 				positions = item.stage01_to_stage02;
// 				break;
// 			case SceneEdgeType.stage02_edges:
// 				positions = item.stage02_edges;
// 				break;
// 			case SceneEdgeType.stage02_island:
// 				positions = item.stage02_island;
// 				break;
// 			case SceneEdgeType.stage02_2_stage03:
// 				positions = item.stage02_to_stage03;
// 				break;
// 			case SceneEdgeType.stage03_edges:
// 				positions = item.stage03_edges;
// 				break;
// 			case SceneEdgeType.stage03_island:
// 				positions = item.stage03_island;
// 				break;
// 			default:
// 				break;
// 		}
//
// 		for (int i = 0; i < allPoints.Count; i++)
// 		{
// 			if (allPoints[i] != null)
// 			{
// 				GameObject.DestroyImmediate(allPoints[i]);
// 			}
// 		}
//
// 		allPoints.Clear();
//
// 		if (positions != null && 1 < positions.Count)
// 		{
// 			for (int i = 0; i < positions.Count;)
// 			{
// 				int count = (int)positions[i];
// 				for (int j = 0; j < count; j = j + 2)
// 				{
// 					int index = (i + 1) + j;
// 					if (index < positions.Count)
// 					{
// 						Vector3 position;
// 						position.x = positions[index];
// 						position.y = 100;
// 						position.z = positions[index + 1];
// 						GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
// 						go.transform.position = position;
// 						go.transform.localScale = Vector3.one * 2;
// 						allPoints.Add(go);
//
// 					}
// 				}
// 				i = (i + 1) + count;
// 			}
// 		}
// 	}
//
// 	void Update()
// 	{
// 		for (int i = allPoints.Count - 1; i >= 0; i--)
// 		{
// 			if (allPoints[i] == null)
// 			{
// 				allPoints.RemoveAt(i);
// 			}
// 		}
//
// 		for (var i = allAreas.Count - 1; i >= 0; i--)
// 		{
// 			var area = allAreas[i];
//
// 			for (var j = area.points.Count - 1; j >= 0; j--)
// 			{
// 				if (area.points[j] == null)
// 				{
// 					area.points.RemoveAt(j);
// 				}
// 			}
//
// 			if (area.points.Count < 1)
// 			{
// 				allAreas.RemoveAt(i);
// 			}
// 		}
// 	}
//
// 	private void OnDrawGizmos()
// 	{
// 		Gizmos.color = Color.red;
//
// 		for (int i = 0; i < allAreas.Count; i++)
// 		{
// 			var area = allAreas[i];
// 			for (int j = 0; j < area.points.Count; j++)
// 			{
// 				var point1 = area.points[j];
// 				var point2 = area.points[(j + 1) % area.points.Count];
//
// 				if (point1 != null && point2 != null)
// 				{
// 					Gizmos.DrawLine(point1.transform.position, point2.transform.position);
// 				}
// 			}
// 		}
//
// 		for (var i = 0; i < stage01ToStage02Gate.Count - 1; i++)
// 		{
// 			Gizmos.DrawLine(stage01ToStage02Gate[i].transform.position, stage01ToStage02Gate[i + 1].transform.position);
// 		}
//
// 		for (var i = 0; i < stage02ToStage03Gate.Count - 1; i++)
// 		{
// 			Gizmos.DrawLine(stage02ToStage03Gate[i].transform.position, stage02ToStage03Gate[i + 1].transform.position);
// 		}
//
// 		for (var i = 0; i < stage01Split.Count - 1; i++)
// 		{
// 			Gizmos.DrawLine(stage01Split[i].transform.position, stage01Split[i + 1].transform.position);
// 		}
//
// 		for (var i = 0; i < stage02Split.Count - 1; i++)
// 		{
// 			Gizmos.DrawLine(stage02Split[i].transform.position, stage02Split[i + 1].transform.position);
// 		}
//
// 		for (var i = 0; i < stage03Split.Count - 1; i++)
// 		{
// 			Gizmos.DrawLine(stage03Split[i].transform.position, stage03Split[i + 1].transform.position);
// 		}
// 	}
//
// 	//[ContextMenu("去冗余")]
// 	public void Del()
// 	{
// 		for (int i = 0; i < allPoints.Count; i++)
// 		{
// 			if (allPoints.Contains(allPoints[i]))
// 			{
// 				allPoints[i] = null;
// 			}
// 		}
//
// 		for (int i = 0; i < allPoints.Count; i++)
// 		{
// 			if (allPoints[i] == null)
// 			{
// 				allPoints[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
// 				allPoints[i].transform.localScale = Vector3.one * 2;
// 			}
// 		}
// 	}
//
//
// 	//[ContextMenu("平滑")]
// 	public void Smooth()
// 	{
// 		for (int i = 0; i < allPoints.Count; i++)
// 		{
// 			if (allPoints[i] == null)
// 			{
// 				break;
// 			}
//
// 			Vector3 position = allPoints[i].transform.position;
//
// 			position.x = ((int)(position.x * 100)) * 0.01f;
// 			position.y = ((int)(position.y * 100)) * 0.01f;
// 			position.z = ((int)(position.z * 100)) * 0.01f;
//
// 			allPoints[i].transform.position = position;
// 		}
// 	}
//
// 	//[ContextMenu("刷入子节点")]
// 	public void SelectChild()
// 	{
// 		allPoints.Clear();
//
// 		int childCount = transform.childCount;
// 		for (int i = 0; i < childCount; i++)
// 		{
// 			Transform child = transform.GetChild(i);
// 			allPoints.Add(child.gameObject);
// 		}
// 	}
//
// 	//[ContextMenu("根据Nvamesh生成边界点")]
// 	public void GeneratePoints()
// 	{
// 		var triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
//
// 		var vertices = triangulation.vertices;
// 		var triangles = triangulation.indices;
//
// 		// 合并重复点
// 		vertices = MergeVertices(vertices, triangles);
//
// 		// 移除无效的三角形
// 		triangles = RemoveInvalidTriangles(triangles);
//
// 		// 初始化临接矩阵
// 		var adjacencyMatrix = InitAdjacencyMatrix(triangles);
//
// 		// 找边界点
// 		var boundaries = FindBoundaries(adjacencyMatrix);
//
// 		// 合并及排序同一环上的点
// 		var boundaryGroups = GroupAndSort(boundaries, adjacencyMatrix);
//
// 		// 移除无效的边界点
// 		RemoveInvalidBoundaries(boundaryGroups, vertices);
//
// 		// 生成小球
// 		GenerateSpheres(boundaryGroups, vertices);
// 	}
//
// 	private Vector3[] MergeVertices(Vector3[] vertices, int[] triangles)
// 	{
// 		var nonduplicationVertices = new List<Vector3>();
//
// 		for (var i = 0; i < triangles.Length; i++)
// 		{
// 			var currentVertex = vertices[triangles[i]];
//
// 			var index = nonduplicationVertices.FindIndex(vertex => AlmostEqual(vertex, currentVertex));
// 			if (index < 0)
// 			{
// 				nonduplicationVertices.Add(currentVertex);
// 				triangles[i] = nonduplicationVertices.Count - 1;
// 			}
// 			else
// 			{
// 				triangles[i] = index;
// 			}
// 		}
//
// 		return nonduplicationVertices.ToArray();
// 	}
//
// 	private int[] RemoveInvalidTriangles(int[] triangles)
// 	{
// 		var validTriangles = new List<int>();
//
// 		for (var i = 0; i < triangles.Length; i += 3)
// 		{
// 			if (triangles[i] == triangles[i + 1] || triangles[i] == triangles[i + 2] || triangles[i + 1] == triangles[i + 2])
// 			{
// 				continue;
// 			}
//
// 			validTriangles.Add(triangles[i]);
// 			validTriangles.Add(triangles[i + 1]);
// 			validTriangles.Add(triangles[i + 2]);
// 		}
//
// 		return validTriangles.ToArray();
// 	}
//
// 	private AdjacencyMatrix InitAdjacencyMatrix(int[] triangles)
// 	{
// 		var adjacencyMatrix = new AdjacencyMatrix();
//
// 		for (var i = 0; i < triangles.Length; i += 3)
// 		{
// 			adjacencyMatrix.Add(triangles[i + 2], triangles[i + 1]);
// 			adjacencyMatrix.Add(triangles[i + 1], triangles[i]);
// 			adjacencyMatrix.Add(triangles[i], triangles[i + 2]);
// 		}
//
// 		return adjacencyMatrix;
// 	}
//
// 	private int[] FindBoundaries(AdjacencyMatrix adjacencyMatrix)
// 	{
// 		var boundaryInices = new HashSet<int>();
//
// 		foreach (var adjacencyPair in adjacencyMatrix.matrix)
// 		{
// 			var a = adjacencyPair.Key;
// 			var adjacency = adjacencyPair.Value;
// 			foreach (var pointPair in adjacency)
// 			{
// 				var b = pointPair.Key;
// 				var edgeCount = pointPair.Value;
//
// 				if (edgeCount <= 1)
// 				{
// 					boundaryInices.Add(a);
// 					boundaryInices.Add(b);
// 				}
// 			}
// 		}
//
// 		return boundaryInices.ToArray();
// 	}
//
// 	private List<List<int>> GroupAndSort(int[] boundaries, AdjacencyMatrix adjacencyMatrix)
// 	{
// 		var allGroups = new List<List<int>>();
//
// 		var remainBoundaries = new List<int>(boundaries);
//
// 		while (remainBoundaries.Count > 0)
// 		{
// 			var oneGroup = FindGroupAndSort(remainBoundaries[0], adjacencyMatrix, remainBoundaries);
// 			if (oneGroup.Count >= 3)
// 			{
// 				allGroups.Add(oneGroup);
// 			}
// 		}
//
// 		return allGroups;
// 	}
//
// 	private List<int> FindGroupAndSort(int startDetect, AdjacencyMatrix adjacencyMatrix, List<int> boundaries)
// 	{
// 		var groupedBoundaries = new List<int>();
// 		groupedBoundaries.Add(startDetect);
// 		boundaries.Remove(startDetect);
//
// 		var nextDetect = FindNextDetect(startDetect, adjacencyMatrix);
// 		while (nextDetect != -1)
// 		{
// 			var startIndex = groupedBoundaries.IndexOf(nextDetect);
// 			if (startIndex > -1)
// 			{
// 				groupedBoundaries.RemoveRange(0, startIndex);
// 				break;
// 			}
//
// 			var currentDetect = nextDetect;
//
// 			if (boundaries.Contains(currentDetect))
// 			{
// 				boundaries.Remove(currentDetect);
// 			}
// 			else
// 			{
// 				break;
// 			}
//
// 			groupedBoundaries.Add(currentDetect);
// 			nextDetect = FindNextDetect(currentDetect, adjacencyMatrix);
// 		}
//
// 		return groupedBoundaries;
// 	}
//
// 	private int FindNextDetect(int currentDetect, AdjacencyMatrix adjacencyMatrix)
// 	{
// 		var adjacency = adjacencyMatrix.matrix[currentDetect];
//
// 		foreach (var pointPair in adjacency)
// 		{
// 			var point = pointPair.Key;
// 			var edgeCount = pointPair.Value;
//
// 			if (edgeCount <= 1)
// 			{
// 				return point;
// 			}
// 		}
//
// 		return -1;
// 	}
//
// 	private void RemoveInvalidBoundaries(List<List<int>> boundaryGroups, Vector3[] vertices)
// 	{
// 		for (var i = boundaryGroups.Count - 1; i >= 0; i--)
// 		{
// 			var boundaryGroup = boundaryGroups[i];
//
// 			for (var j = boundaryGroup.Count - 1; j >= 0; j--)
// 			{
// 				var last = boundaryGroup[(j - 1 + boundaryGroup.Count)%boundaryGroup.Count];
// 				var current = boundaryGroup[j];
// 				var next = boundaryGroup[(j + 1) % boundaryGroup.Count];
//
// 				var lastNormal = vertices[last].ToVector2XZ() - vertices[current].ToVector2XZ();
// 				var nextNormal = vertices[next].ToVector2XZ() - vertices[current].ToVector2XZ();
//
// 				var angle = Vector2.Angle(lastNormal, nextNormal);
//
// 				if (angle < 1f)
// 				{
// 					boundaryGroup.RemoveAt(j);
// 				}
// 			}
//
// 			if (boundaryGroup.Count < 3)
// 			{
// 				boundaryGroups.RemoveAt(i);
// 			}
// 		}
// 	}
//
// 	private void GenerateSpheres(List<List<int>> boundaryGroups, Vector3[] vertices)
// 	{
// 		allPoints.Clear();
//
// 		// 根据边界点生成小球
// 		for (int i = 0; i < boundaryGroups.Count; i++)
// 		{
// 			var area = new Area();
// 			var boundaryGroup = boundaryGroups[i];
// 			for (int j = 0; j < boundaryGroup.Count; j++)
// 			{
// 				var boundary = boundaryGroup[j];
// 				var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
// 				sphere.name = string.Format("sphere_{0}", boundary);
// 				sphere.transform.position = vertices[boundary];
// 				sphere.transform.localScale = Vector3.one * 2;
//
// 				allPoints.Add(sphere);
// 				area.points.Add(sphere);
// 			}
// 			allAreas.Add(area);
// 		}
// 	}
// 	public void DelSpheres()
// 	{
// 		foreach (var allPoint in allPoints)
// 		{
// 			GameObject.Destroy(allPoint);
// 		}
// 		allPoints.Clear();
// 		allAreas.Clear();
// 	}
//
// 	public int FindShpereIndex(GameObject sphere)
// 	{
// 		return allPoints.IndexOf(sphere);
// 	}
//
// 	private bool AlmostEqual(Vector3 lhs, Vector3 rhs)
// 	{
// 		return (lhs - rhs).sqrMagnitude <= 0.01f;
// 	}
//
// 	private class AdjacencyMatrix
// 	{
// 		public Dictionary<int, Dictionary<int, int>> matrix = new Dictionary<int, Dictionary<int, int>>();
//
// 		public void Add(int a, int b)
// 		{
// 			if (!matrix.ContainsKey(a))
// 			{
// 				matrix[a] = new Dictionary<int, int>();
// 			}
// 			if (!matrix[a].ContainsKey(b))
// 			{
// 				matrix[a][b] = 0;
// 			}
//
// 			if (matrix.ContainsKey(b))
// 			{
// 				if (matrix[b].ContainsKey(a))
// 				{
// 					matrix[a][b] = matrix[b][a];
//
// 					matrix[b][a] = matrix[b][a] + 1;
// 				}
// 			}
//
// 			matrix[a][b] = matrix[a][b] + 1;
// 		}
// 	}
//
// 	[Serializable]
// 	public class Area
// 	{
// 		public List<GameObject> points = new List<GameObject>();
// 	}
// }
