using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int dirx;
    private int diry;
    private bool isMoving = false;
    private float speed = 5.0f;
    private UIBlood uiBlood;
    private Transform mountPoint = null;

    public void Init() {
        this.diry = 0;
        this.dirx = 0;
        this.isMoving = false;
        this.speed = 5;

        // 血条
        this.uiBlood = FightMgr.Instance.uiGame.CreateUIBlood();
        this.uiBlood.SetPercent(0.3f);
        // end 

        this.mountPoint = this.transform.Find("mountPoint");
    }

    public void Move(int dirx, int diry) {
        if (this.dirx == dirx && this.diry == diry) {
            return;
        }

        this.dirx = dirx;
        this.diry = diry;
        if (this.dirx == 0 && this.diry == 0) { // 停止移动
            this.isMoving = false;
            return;
        }

        this.isMoving = true;
        float r = Mathf.Atan2(this.diry, this.dirx);
        float degree = r * Mathf.Rad2Deg;
        degree = degree - 90; // 对齐起点;
        degree = 360 - degree; // 时针方向;

        Vector3 eRot = this.transform.eulerAngles;
        eRot.y = degree;
        this.transform.eulerAngles = eRot;
    }

    private void WalkUpdate() {
        float dt = Time.deltaTime;
        float s = this.speed * dt;
        float sx = (s * this.dirx) / (1 << 16);
        float sz = (s * this.diry) / (1 << 16);

        Vector3 pos = this.transform.position;
        pos.x += sx;
        pos.z += sz;
        this.transform.position = pos;

        
    }

    public void Update() {
        if (this.isMoving) {
            this.WalkUpdate();
        }
    }

    private void syncUIBlood() {
        // 挂载点的世界坐标---》屏幕坐标;--->屏幕坐标--->UI机制下的坐标;
        Vector3 worldPos = this.mountPoint.position;
        Vector3 sceenPos = FightMgr.Instance.gameCamera.WorldToScreenPoint(worldPos);
        this.uiBlood.ShowAt(sceenPos);
    }

    public void LateUpdate()
    {
        this.syncUIBlood();
    }
}
