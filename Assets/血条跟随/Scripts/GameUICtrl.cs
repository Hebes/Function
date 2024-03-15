using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUICtrl : MonoBehaviour
{
    private joystick stick;
    private Transform uiBloodRoot;
    public void Init() {
        this.uiBloodRoot = this.transform.Find("UIBloodRoot");
        this.stick = this.transform.Find("Joystick").GetComponent<joystick>();
    }

    public void Update()
    {
        // ����ң�еķ������ƶ����ǵĽ�ɫ; 16.16
        FightMgr.Instance.player.Move((int)(this.stick.dir.x * ( 1 << 16)), (int)(this.stick.dir.y * (1 << 16)));
        // end 
    }

    public UIBlood CreateUIBlood() {
        // ���������Դ��ʵ����һ��; 
        GameObject blood = GameObject.Instantiate(FightMgr.Instance.uiBloodPrefab);
        blood.transform.SetParent(this.uiBloodRoot, false);

        UIBlood uiBlood = blood.AddComponent<UIBlood>();
        uiBlood.Init();
        // end

        return uiBlood;
    }
}
