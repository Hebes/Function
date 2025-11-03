// using System;
// using UnityEngine.UI;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using Game.UI;
// using PEUtils;
// using UnityEngine.UI.Extensions;
// using zhaorh.Excel;
//
// namespace zhaorh.UI
// {
//     /// <summary>
//     /// 场景loading
//     /// </summary>
//     public class SceneLoadingWindow : MonoBehaviour
//     {
//         System.Random random = new System.Random();
//
//         //进度条本体
//         private RectTransform barRect = null;
//         private Vector2 barSize;
//         //进度条头部
//         private RectTransform headRect = null;
//         private Vector3 headPos;
//         //提示文字
//         private Text tips = null;
//         //版本提示文字
//         private Text versionTip = null;
//         //进度提示
//         private Text percentText = null;
//         //进度显示
//         private Text percent = null;
//         //进度最大值（手动取的值）
//         private int MaxValue = 0;
//         //轮播的图片
//         private Image bg;
//         /// <summary>
//         /// 显示背景的Id 对应TLoading表
//         /// </summary>
//         private int bgIndexLoading = -1;
//         //提示文字
//         private List<int> tipIds = new List<int>();
//         //循环时间
//         private float cycleTime = 0f;
//         /// <summary>
//         /// 显示的进度
//         /// </summary>
//         private int process = 0;
//         /// <summary>
//         /// 界面显示的时间
//         /// </summary>
//         private float curTime = 0f;
//         private int index = 0;
//         /// <summary>
//         /// 显示版号和提示的文本
//         /// </summary>
//         private Text banhaoText;
//         private Text tipsText;
//         /// <summary>
//         /// 站在进度条前面的小人
//         /// </summary>
//         private string loadingFxName = "loading_weijier";
//         private RectTransform loadingFxRect = null;
//         private UISpriteAnimationFxLoader loadingFxLoader = null;
//         private Vector3 headPosFx;
//
//         /////////////////////////////////////////////////////////////
//         /// <summary>
//         /// 显示角色用的部分
//         /// </summary>
//         private Transform roleShowTrans = null;
//
//         private Image roleImage = null;
//             
//         private Image roleTitleImage = null;
//
//         private Zrh_TextExt roleTitleText = null;
//
//         private Gradient2 roleTitleGradient = null;
//
//         private RectTransform roleSkillInfo1 = null;
//
//         private Zrh_TextExt roleSkillLabelText1 = null;
//
//         private Zrh_TextExt roleSkillNameText1 = null;
//
//         private Zrh_TextExt roleSkillDetailText1 = null;
//
//         private RectTransform roleSkillInfo2 = null;
//
//         private Zrh_TextExt roleSkillLabelText2 = null;
//
//         private Zrh_TextExt roleSkillNameText2 = null;
//
//         private Zrh_TextExt roleSkillDetailText2 = null;
//
//         private Image roleTipImage = null;
//
//         private Zrh_TextExt roleTipText = null;
//
//         private Image roleWenziImage = null;
//
//         private Transform shuxingkezhi = null;
//
//         private Transform shuxingkezhiPanel = null;
//
//         private Image shuxingkezhiTipBgImage = null;
//
//         private Zrh_TextExt shuxingkezhiTipText = null;
//
//         private Transform boundaryLine = null;
//         /////////////////////////////////////////////////////////////
//
//         /// <summary>
//         /// 初始化加载的显示对象
//         /// </summary>
//         /// <param name="sceneName"></param>
//         public void InitLoading(string sceneName)
//         {
//             try
//             {
//                 progressValue = 0;
//                 var tlp = TableMgr.Table_TLoading_LoadingData.Values.ToList().Find((o) =>
//                 {
//                     return o.into_scene == sceneName;
//                 });
//                 if (tlp != null)
//                 {
//                     cycleTime = tlp.interval_time;
//                     //返回一个指定范围内的随机数. min max
//                     int bgIndex = random.Next(0, tlp.use_picture_index.Count);//随机底图下标
//                     bgIndexLoading = tlp.use_picture_index.Count > 0 ? tlp.use_picture_index[bgIndex] : -1; //如果下标地图为空则为-1
//                     tipIds = tlp.prompt_text_id.ToArray().OrderBy(s => Guid.NewGuid()).ToList(); //获取文本id
//                     string[] loadingFxNames = Tools.ToStringArray(tlp.loading_fx_list);//使用加载的序列帧动画 (小人跑步)
//                     int fxIndex = random.Next(0, loadingFxNames.Length);
//                     loadingFxName = loadingFxNames[fxIndex];
//                     //加载显示背景和文本提示
//                     ShowInfo();
//                 }
//                 else
//                 {
//                     Debug2.LogWarning($"初始化加载的显示对象 找不到关于 {sceneName} 表数据!");
//                 }
//             }
//             catch(Exception e)
//             {
//                 Debug2.Log($"[InitLoading] 初始化加载的显示对象 异常!");
//             }
//         }
//
//         /// <summary>
//         /// 设置进度条的值
//         /// value -> [0~100]
//         /// </summary>
//         public int progressValue
//         {
//             get
//             {
//                 return process;
//             }
//             set
//             {
//                 process = value;
//                 var val = (process / 100f) * MaxValue;
//                 headRect.localPosition = headPos + new Vector3(val, 0, 0);
//                 loadingFxRect.localPosition = headPosFx + new Vector3(val, 0, 0);
//                 percent.text = value + "%";
//             }
//         }
//
//         /// <summary>
//         /// 设置提示的文字
//         /// </summary>
//         public string tipsValue
//         {
//             set
//             {
//                 tips.text = value;
//             }
//         }
//
//         /// <summary>
//         /// 设置版本信息显示
//         /// </summary>
//         public string versionTipValue
//         {
//             set
//             {
//                 versionTip.text = value;
//             }
//         }
//
//         /// <summary>
//         /// 设置下载的量
//         /// </summary>
//         public string percentValue
//         {
//             set
//             {
//                 percentText.text = value;
//             }
//         }
//
//         /// <summary>
//         /// 显示信息
//         /// </summary>
//         public void ShowInfo()
//         {
//             ShowBg();
//             ShowTips();
//             banhaoText = transform.GetChildComponent<Text>("BanhaoText");
//             tipsText = transform.GetChildComponent<Text>("Tips");
//             SetBanhaoText();
//
//             loadingFxLoader.spritePrefabName = loadingFxName;//设置默认进度条小人
//             loadingFxLoader.Play();
//         }
//
//         #region ShowBg(显示背景)
//
//         /// <summary>
//         /// 显示背景
//         /// </summary>
//         public void ShowBg()
//         {
//             try
//             {
//                 //显示角色索引表
//                 var trp = TableMgr.Table_TLoading_ReferroleinfoData[bgIndexLoading.ToString()]; 
//
//                 if (trp != null)
//                 {
//                     //背景图片
//                     bg = ResMgr.Instance.ResLoad<Image>(trp.bg_texture_name);
//                     //填充的类型（2表示克制关系填充，1表示填充角色类，0表示不填充）
//                     if (trp.use_roleinfo_set == 1)
//                     {
//                         UseRoleinfoSet_1(trp);
//                     }
//                     else if(trp.use_roleinfo_set == 2)
//                     {
//                         UseRoleinfoSet_2(trp);
//                     }
//                     else
//                     {
//                         //关闭背景显示
//                         SettingCloseShowBg();
//                     }
//                 }
//                 else
//                 {
//                     SettingCloseShowBg();
//                 }
//             
//             }
//             catch
//             {
//                 SettingCloseShowBg();
//             }
//         }
//         /// <summary>
//         /// 表示填充角色类
//         /// </summary>
//         /// <param name="trp"></param>
//         void UseRoleinfoSet_1(Table_TLoading_Referroleinfo trp)
//         {
//             //角色的贴图
//             roleImage = ResMgr.Instance.ResLoad<Image>(trp.role_image_name);
//             //设置显示角色贴图的位置偏移
//             roleImage.rectTransform.anchoredPosition = new Vector3(trp.role_iamge_offset, roleImage.rectTransform.anchoredPosition.y, 0f);
//             //设置本地坐标大小
//             roleImage.SetNativeSize();
//             //提示文字图
//             roleWenziImage = ResMgr.Instance.ResLoad<Image>(trp.tip_wenzitu);
//             //有字样等级标识图 ss ssr 
//             roleTitleImage = ResMgr.Instance.ResLoad<Image>(trp.role_dengji_name);
//             //赤,橙,黄,用颜色区分的 等级标识图
//             roleTipImage = ResMgr.Instance.ResLoad<Image>(trp.chanchu_ditu);
//             //设置多语言
//             SetLocalKey(roleTitleText, trp.use_role_name);
//             //设置梯度渐变色
//             roleTitleGradient.EffectGradient.SetKeys(
//                 new GradientColorKey[]{
//                 new GradientColorKey { color = Tools.HexToColor(trp.use_role_name_color1), time = 0f} ,
//                 new GradientColorKey { color = Tools.HexToColor(trp.use_role_name_color2), time = 1f }
//                 },
//                 new GradientAlphaKey[]{
//                 new GradientAlphaKey { alpha = 1f, time = 0f },
//                 new GradientAlphaKey { alpha = 1f, time = 1f },
//                 }
//             );
//         
//             SetLocalKey(roleSkillLabelText1, trp.skill_label_name1);
//         
//             roleSkillInfo1.anchoredPosition = new Vector2(trp.text_info1_offset, roleSkillInfo1.anchoredPosition.y);
//         
//             roleSkillInfo2.anchoredPosition = new Vector2(trp.text_info2_offset, roleSkillInfo2.anchoredPosition.y);
//
//             SetLocalKey(roleSkillNameText1, trp.skill_name1);
//         
//             SetLocalKey(roleSkillDetailText1, trp.skill_detail1);
//         
//             SetLocalKey(roleSkillLabelText2, trp.skill_label_name2);
//         
//             SetLocalKey(roleSkillNameText2, trp.skill_name2);
//         
//             SetLocalKey(roleSkillDetailText2, trp.skill_deltail2);
//         
//             SetLocalKey(roleTipText, trp.chanchu_fangshi);
//         
//             roleTipText.gameObject.GetComponent<Text>().color = Tools.HexToColor(trp.chanchu_fangshi_color);
//         
//             roleShowTrans.gameObject.SetActive(true);
//         
//             roleImage.gameObject.SetActive(true);
//         
//             shuxingkezhi.gameObject.SetActive(false);
//         
//             boundaryLine.gameObject.SetActive(true);
//         }
//
//         void UseRoleinfoSet_2(Table_TLoading_Referroleinfo trp)
//         {
//             roleShowTrans.gameObject.SetActive(false);
//                     
//             roleImage = ResMgr.Instance.ResLoad<Image>(trp.role_image_name);
//                     
//             roleImage.SetNativeSize();
//                     
//             shuxingkezhiTipBgImage = ResMgr.Instance.ResLoad<Image>(trp.chanchu_ditu);;
//                     
//             SetLocalKey(shuxingkezhiTipText, trp.chanchu_fangshi);
//
//             //GuidePropertyPanel panel = UIWindow.AddPanel<GuidePropertyPanel>(shuxingkezhiPanel, null);
//                     
//             //panel.GetChildComponent<Image>("PropertyUI").enabled = false;
//                     
//             roleImage.gameObject.SetActive(true);
//                     
//             shuxingkezhi.gameObject.SetActive(true);
//                     
//             boundaryLine.gameObject.SetActive(true);
//         }
//         void SettingCloseShowBg()
//         {
//             roleImage.gameObject.SetActive(false);
//             
//             roleShowTrans.gameObject.SetActive(false);
//             
//             shuxingkezhi.gameObject.SetActive(false);
//             
//             boundaryLine.gameObject.SetActive(false); 
//         }
//
//
//         #endregion
//         
//         /// <summary>
//         /// 显示文字提示
//         /// </summary>
//         /// <param name="index"></param>
//         /// <returns></returns>
//         public void ShowTips()
//         {
//             if (tipIds.Count > 0)
//             {
//                 index = index % tipIds.Count;
//                 var tep = TableMgr.Table_TLocale_ZhData[tipIds[index].ToString()];
//                 if (tep == null)
//                 {
//                     Debug.LogError("文本不存在! id ： " + tipIds[index]);
//                     return;
//                 }
//                 tipsValue = tep.label;
//                 //真机在这里显示一下版本号
//                 if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
//                 {
//                     tipsValue = tep.label;
//                 }
//             
//                 index++;
//             }
//         }
//
//         void Update()
//         {
//             curTime += Time.deltaTime;
//             if (curTime > cycleTime)
//             {
//                 ShowTips();
//                 curTime = 0f;
//             }
//         }
//
//         public void InitUI()
//         {
//             barRect = transform.GetChildComponent<RectTransform>("ProgressBar/Bar/BarImage");
//             barSize = barRect.sizeDelta;
//
//             headRect = transform.GetChildComponent<RectTransform>("ProgressBar/Bar/Head");
//             headPos = headRect.localPosition;
//
//             tips = transform.GetChildComponent<Text>("ProgressBar/Tips/Text");
//
//             versionTip = transform.GetChildComponent<Text>("versionTips");
//
//             percentText = transform.GetChildComponent<Text>("percentText");
//
//             percent = transform.GetChildComponent<Text>("ProgressBar/Tips/Percent");
//
//             bg = transform.GetChildComponent<Image>("Bg/BgTexture");
//
//             var mainRect = transform.GetChildComponent<RectTransform>("ProgressBar/Bar");
//             MaxValue = (int)(mainRect.sizeDelta.x);
//
//             loadingFxRect = transform.GetChildComponent<RectTransform>("LoadingFx");
//             loadingFxLoader = loadingFxRect.transform.GetComponent<UISpriteAnimationFxLoader>();
//             headPosFx = loadingFxRect.localPosition;
//
//             //Debug.LogError("headPosFx" + headPosFx.ToString());
//
//             //////////////////////////////////////////////////////////////////
//             roleShowTrans = transform.Find("Bg/RoleShow");
//
//             roleImage = transform.GetChildComponent<Image>("Bg/RoleImage");
//
//             roleTitleImage = transform.GetChildComponent<Image>("Bg/RoleShow/RoleTitle/TitleImage");
//
//             roleTitleText = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleTitle/TitleText");
//
//             roleTitleGradient = transform.GetChildComponent<Gradient2>("Bg/RoleShow/RoleTitle/TitleText");
//
//             roleSkillInfo1 = transform.GetChildComponent<RectTransform>("Bg/RoleShow/RoleInfo1");
//
//             roleSkillLabelText1 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo1/RoleInfoTitle1");
//
//             roleSkillNameText1 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo1/RoleInfoTitle2");
//
//             roleSkillDetailText1 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo1/RoleInfoDetail");
//
//             roleSkillInfo2 = transform.GetChildComponent<RectTransform>("Bg/RoleShow/RoleInfo2");
//
//             roleSkillLabelText2 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo2/RoleInfoTitle1");
//
//             roleSkillNameText2 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo2/RoleInfoTitle2");
//
//             roleSkillDetailText2 = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/RoleInfo2/RoleInfoDetail");
//
//             roleTipImage = transform.GetChildComponent<Image>("Bg/RoleShow/Tips/TipImage");
//
//             roleTipText = transform.GetChildComponent<Zrh_TextExt>("Bg/RoleShow/Tips/TipText");
//
//             roleWenziImage = transform.GetChildComponent<Image>("Bg/RoleShow/wenziImage");
//
//             shuxingkezhi = transform.Find("Bg/shuxingkezhi");
//
//             shuxingkezhiPanel = transform.Find("Bg/shuxingkezhi/shuxingkezhiPanel");
//
//             shuxingkezhiTipBgImage = transform.GetChildComponent<Image>("Bg/shuxingkezhi/shuxingkezhiTip/bgImage");
//
//             shuxingkezhiTipText = transform.GetChildComponent<Zrh_TextExt>("Bg/shuxingkezhi/shuxingkezhiTip/bgText");
//
//             boundaryLine = transform.Find("Bg/boundaryLine");
//             //////////////////////////////////////////////////////////////////
//         }
//
//         public void Close()
//         {
//             //隐藏版本提示文字
//             versionTip.gameObject.SetActive(false);
//             //隐藏版本进度文本
//             percentText.gameObject.SetActive(false);
//             //整体隐藏
//             gameObject.SetActive(false);
//         }
//
//
//         private void SetBanhaoText()
//         {
//             // 只在开头显示版号信息
// 			bool isShow = "Splash".Equals(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
// 			banhaoText.text = isShow ? WorldConfig.banhao_shuoming : "";
// 			tipsText.text = isShow ? WorldConfig.game_warning_tips : "";
//         }
//
//         private void SetLocalKey(Zrh_TextExt text, string key)
//         {
//             if(string.IsNullOrEmpty(key))
//             {
//                 text.SetText("");
//             }
//             else
//             {
//                 text.SetLocaleKey(key);
//             }
//         }
//     }
// }
