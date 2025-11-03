using System.Collections.Generic;

public class AchievementManager
{
    private static AchievementManager _i;
    public static AchievementManager I => _i ??= new AchievementManager();
    
    public readonly List<int> AchievementInfo = new List<int>();
    private readonly Dictionary<int, AchievementInfo> _achievementInfoDic = new Dictionary<int, AchievementInfo>();
    
    /// <summary>
    /// 奖项成就
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool AwardAchievement(int index)
    {
        if (GetAchievementUnlockState(index))
        {
            return false;
        }
        UnlockAchievement(index);
        //AwardTrophy(this.GetAchievementInfo(index).Name, index.ToString());弹出成就窗口
        return true;
    }
    
    /// <summary>
    /// 获取成就信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AchievementInfo GetAchievementInfo(int index)
    {
        return _achievementInfoDic[index];
    }
    
    // private void SetDictionaryInfo(XmlNodeList nodeList, Dictionary<int, AchievementManager.AchievementInfo> dictionary)
    // {
    //     for (int i = 0; i < nodeList.Count; i++)
    //     {
    //         XmlNode xmlNode = nodeList.Item(i);
    //         if (xmlNode != null)
    //         {
    //             dictionary.Add(i, new AchievementManager.AchievementInfo
    //             {
    //                 Id = i,
    //                 Name = xmlNode.ChildNodes[0].InnerText,
    //                 Detail = xmlNode.ChildNodes[1].InnerText
    //             });
    //         }
    //     }
    // }
    
    /// <summary>
    /// 解锁所有成就
    /// </summary>
    public void AwardAll()
    {
        for (var i = 1; i < _achievementInfoDic.Count; i++)
            AwardAchievement(i);
    }
    
    /// <summary>
    /// 获取成就解锁状态
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetAchievementUnlockState(int index)
    {
        return AchievementInfo.Contains(index);
    }
    
    /// <summary>
    /// 解锁成就
    /// </summary>
    /// <param name="index"></param>
    private void UnlockAchievement(int index)
    {
        if (!GetAchievementUnlockState(index))
            AchievementInfo.Add(index);
        //R.Settings.Save();
    }

    #region 弹窗
    
    // private readonly Queue<Trophy> _trophyQueue = new Queue<Trophy>();
    // public void AwardTrophy(string trophyName, string spriteName)
    // {
    //     this._trophyQueue.Enqueue(new Trophy(trophyName, spriteName));
    //     if (!this._isPlaying)
    //     {
    //         this.AwardTrophy();
    //     }
    // }
    //
    // private Coroutine AwardTrophy()
    // {
    //     UITrophyNotificationController.Trophy trophy = this._trophyQueue.Dequeue();
    //     this._trophyName.text = trophy.TrophyName;
    //     this._trophyIcon.spriteName = trophy.SpriteName;
    //     this._widget.alpha = 1f;
    //     this._widget.transform.localPosition = new Vector3(0f, (float)this._widget.height, 0f);
    //     return base.StartCoroutine(this.AwardTrophyCoroutine());
    // }
    //
    // private IEnumerator AwardTrophyCoroutine()
    // {
    //     this._isPlaying = true;
    //     this._panel.gameObject.SetActive(true);
    //     yield return this._widget.transform.DOLocalMoveY(0f, 0.5f, false).WaitForCompletion();
    //     yield return new WaitForSeconds(5f);
    //     yield return this._widget.DOFade(0f, 0.5f).WaitForCompletion();
    //     this._panel.gameObject.SetActive(false);
    //     this._isPlaying = false;
    //     if (this._trophyQueue.Count != 0)
    //     {
    //         this.AwardTrophy();
    //     }
    //     yield break;
    // }
    #endregion

}

public struct AchievementInfo : IID
{
    /// <summary>
    /// ID
    /// </summary>
    public long ID { get; set; }

    /// <summary>
    /// 名称->都是简体中文
    /// </summary>
    public string Name;

    /// <summary>
    /// 详细
    /// </summary>
    public string Detail;
}

public class Trophy
{
    public Trophy(string trophyName, string spriteName)
    {
        TrophyName = trophyName;
        SpriteName = spriteName;
    }

    public string TrophyName { get; set; }

    public string SpriteName { get; set; }
}