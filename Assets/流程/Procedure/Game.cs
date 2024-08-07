using System.IO;
using UnityEngine;
using XFramework;
using XFramework.Fsm;

namespace XFramework
{
    /// <summary>
    /// 这个类挂在初始场景中,是整个游戏的入口
    /// </summary>
    public class Game : MonoBehaviour
    {
        // 初始流程
        [HideInInspector] public string typeName;
        [SerializeReference] public ProcedureBase startProcedure;

        public ProcedureBase ccc;
        public static Game activeGame { get; private set; }

        void Awake()
        {
            if (activeGame != null)
            {
                DestroyImmediate(this);
            }
            else
            {
                activeGame = this;
            }

            InitAllModel();

            // 设置运行形后第一个进入的流程
            System.Type type = System.Type.GetType(typeName);
            if (startProcedure != null)
            {
                ProcedureManager.Instance.UpdateProcedure(startProcedure);
                ProcedureManager.Instance.ChangeProcedure(startProcedure.GetType());
            }
            else
                Debug.LogError("当前工程还没有任何流程");

            DontDestroyOnLoad(this);
        }

        void Update()
        {
            GameEntry.ModuleUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        public void OnGUI()
        {
        }

        private void OnDestroy()
        {
        }

        /// <summary>
        /// 初始化模块，这个应该放再各个流程中，暂时默认开始时初始化所有模块
        /// </summary>
        public void InitAllModel()
        {
            // Start2
            //GameEntry.AddModule<EntityManager>();
            GameEntry.AddModule<FsmManager>();
            //GameEntry.AddModule<GraphicsManager>();
            // GameEntry.AddModule<DataSubjectManager>();
#if UNITY_EDITOR
            string mapInfoPath = $"{Application.streamingAssetsPath}/pathMap.info";
            //ResourceManager.GeneratePathMap(mapInfoPath, "Assets/Res");
            //GameEntry.AddModule<ResourceManager>(new AssetDataBaseLoadHelper());
#else
        GameEntry.AddModule<ResourceManager>(new AssetBundleLoadHelper(), mapInfoPath);
#endif
            GameEntry.AddModule<UIHelper>();
            // End2
        }

        private void OnValidate()
        {
            if (ProcedureManager.IsValid)
            {
                ProcedureManager.Instance.UpdateProcedure(startProcedure);
            }
        }
    }
}