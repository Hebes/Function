using UnityEditor;
using UnityEditor.SceneManagement;

public class ToolScene //: EditorWindow
{
    public static ToolScene Instance { get; private set; }

    /// <summary>
    /// https://blog.csdn.net/LWKlwk11/article/details/127278265
    /// </summary>
    [MenuItem("Assets/切换Init场景", false, 1000)]
    [MenuItem("GameObject/切换Init场景", false, 1000)]
    public static void SwitchScene()
    {
        EditorSceneManager.OpenScene("Assets/Resources/AssetsPackage/Scenes/SceneInit.unity");
        //Instance = new ToolScene();
        //Type type = Instance.GetType();
        ////object obj = Activator.CreateInstance(type);
        //MethodInfo methodInfo = type.GetMethod("ChangeScene");//, System.Reflection.BindingFlags.Static
        //MenuItem menuItem = methodInfo.GetAttribute<MenuItem>();
        //menuItem.menuItem = "Assets/从Start场景开始Play/11";
        //methodInfo?.Invoke(11, null);
        //int count = SceneManager.sceneCountInBuildSettings;
        //string[] scene_names = new string[count];
        //string[] scene_paths = new string[count];
        //for (int i = 0; i < count; i++)
        //{
        //    scene_names[i] = SceneUtility.GetScenePathByBuildIndex(i);
        //    //从Assets路径下到此场景的路径
        //    Debug.Log("Assets路径开始到场景的路径为：" + scene_names[i]);
        //    scene_paths[i] = SceneUtility.GetScenePathByBuildIndex(i);
        //    //路径
        //    Debug.Log("路径为：" + scene_paths[i]);
        //    string[] strs = scene_names[i].Split('/');
        //    string str = strs[strs.Length - 1];
        //    strs = str.Split('.');
        //    str = strs[0];
        //    scene_names[i] = str;
        //    //场景的名字
        //    Debug.Log("场景的名字为：" + str);
        //}
        //Scene scene = SceneManager.GetActiveScene();//获取到单前场景的名字的代码
    }

    //[MenuItem("Assets/从Start场景开始Play/")]
    //public static void ChangeScene(string scenePath)
    //{
    //    EditorSceneManager.OpenScene(scenePath);
    //    EditorApplication.isPlaying = true;
    //}
}
