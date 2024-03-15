using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMgr : MonoBehaviour
{
    public Camera gameCamera = null; // ս���е���Ϸ�����;
    public Player player = null; // ս���е����;

    public GameUICtrl uiGame = null; // ս������ϷUI����;

    public static FightMgr Instance = null;

    // test
    public GameObject uiBloodPrefab = null;

    private void Awake() {
        FightMgr.Instance = this;

        this.gameCamera = this.transform.Find("Main Camera").GetComponent<Camera>();

        this.uiGame = GameObject.Find("Canvas/GameUI").AddComponent<GameUICtrl>();
        this.uiGame.Init();

        this.player = this.transform.Find("Player").gameObject.AddComponent<Player>();
        this.player.Init();
    }
}
