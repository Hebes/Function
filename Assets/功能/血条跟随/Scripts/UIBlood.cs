using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlood : MonoBehaviour
{
    Image value = null;

    public void Init() {
        this.value = this.transform.Find("value").GetComponent<Image>();
    }

    public void SetPercent(float per) {
        per = (per < 0) ? 0 : per;
        per = (per > 1) ? 1 : per;

        this.value.fillAmount = per;
    }

    public void ShowAt(Vector3 screenPos) {
        // UGUI ---> ovverladģʽ��sceenPos == UIԪ�ص���������;
        this.transform.position = screenPos;
        // UGUI--->�����---�������תsceenTo����;

    }
}
