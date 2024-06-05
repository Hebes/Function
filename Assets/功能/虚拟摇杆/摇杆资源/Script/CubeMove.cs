using UnityEngine;

/// <summary>
/// 物体移动
/// </summary>
public class CubeMove : MonoBehaviour
{
    private Vector3 dir;

    void Start()
    {
        JpyStickPanel.Instance.move = CheckDirChange;
    }

    void Update()
    {
        this.transform.Translate(dir * Time.deltaTime, Space.World);
    }

    private void CheckDirChange(Vector2 value)
    {
        this.dir.x = value.x;
        this.dir.z = value.y;
    }
}
