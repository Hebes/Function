using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("是否跟随")] public bool IsFollowPivot = true;
    [Header("摄像机")] private Camera _camera;
    [Header("是否锁定")] private bool _isLock;
    [Header("是动态模糊")] private bool _isMotionBlur;
    [Header("平滑时间")] [SerializeField] private readonly float _smoothTime = 0.4f;
    [Header("手动偏移Y")] public float ManualOffsetY;
    [Header("视野")] private float _fieldOfView = 60f;
    [Header("当前速度")] private Vector3 _currentSpeed = Vector3.zero;

    private Transform _pivot; //观察点
    private Transform _player; //玩家观察点

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void LateUpdate()
    {
        if (IsFollowPivot && Pivot != null)
        {
            UpdateCamera(Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        ManualOffsetY = 0f;
        _fieldOfView = 52f;
        MovableCamera.position = MovableCamera.position.SetZ(-10f);
    }

    public Transform Pivot
    {
        get => _pivot ?? _player;
        set => _pivot = value;
    }

    public Transform MovableCamera => transform.parent;

    public float FieldOfView
    {
        get => _fieldOfView;
        set => _fieldOfView = Mathf.Clamp(value, 0f, 179f);
    }

    private float Fv2DeltaZ()
    {
        float num = Mathf.Abs(-10f);
        return num * Mathf.Tan(0.4537856f) / Mathf.Tan(FieldOfView / 2f * 0.0174532924f) - num;
    }

    private float MaxVisibleZ => -12f + Fv2DeltaZ();
    private float MaxDetectZ => MaxVisibleZ - -2f + Fv2DeltaZ();
    private float MinVisibleZ => -8f + Fv2DeltaZ();
    private float MinDetectZ => MinVisibleZ - -2f + Fv2DeltaZ();
    private Rect MaxVisibleRect => CameraRect(MaxVisibleZ);
    private Rect MaxDetectRect => CameraRect(MaxDetectZ);
    private Rect MinVisibleRect => CameraRect(MinVisibleZ);
    private Rect MinDetectRect => CameraRect(MinDetectZ);
    public YieldInstruction CameraMoveTo(Vector3 pos, float second, Ease ease = Ease.Linear)
    {
        if (_isLock) return null;
        IsFollowPivot = false;
        _isLock = true;
        pos.z = MovableCamera.position.z;
        KillTweening();
        return MovableCamera.DOMove(CameraPositionClamp(pos), second).SetEase(ease).OnComplete(delegate { CamereMoveFinished(false); }).WaitForCompletion();
    }
    public void CameraMoveToBySpeed(Vector3 pos, float speed, bool canReturn = false, Ease type = Ease.Linear)
    {
        if (!_isLock)
        {
            IsFollowPivot = false;
            _isLock = true;
            pos.z = MovableCamera.position.z;
            if (canReturn)
            {
                Vector3[] path =
                {
                    CameraPositionClamp(pos),
                    MovableCamera.position
                };
                MovableCamera.DOPath(path, speed).SetSpeedBased(true).SetEase(type).OnComplete(delegate { CamereMoveFinished(true); });
            }
            else
            {
                MovableCamera.DOMove(CameraPositionClamp(pos), speed).SetSpeedBased(true).SetEase(type).OnComplete(delegate { CamereMoveFinished(false); });
            }
        }
    }

    private void CamereMoveFinished(bool follow)
    {
        _isLock = false;
        IsFollowPivot = follow;
    }

    public void CameraZoom(Vector3 pos, float second, float deltaZ = 3f)
    {
        if (!_isLock)
        {
            _isLock = true;
            IsFollowPivot = false;
            float num = (1f - Mathf.Tan(0.4537856f) / Mathf.Tan(0.0174532924f * FieldOfView / 2f)) * 7f;
            pos.z = -10f + deltaZ + num;
            MovableCamera.DOMove(CameraPositionClamp(pos), second).SetEase(Ease.Linear).OnComplete(ZoomFinished);
        }
    }

    public void ZoomFinished()
    {
        _isLock = false;
    }

    public void CameraZoomFinished()
    {
        _pivot = _player;
        IsFollowPivot = true;
    }

    public void CameraShake(float second, float strength = 0.2f, ShakeTypeEnum type = ShakeTypeEnum.Rect, bool isLoop = false)
    {
        if (Math.Abs(second) < 0.01f)
        {
            return;
        }

        Vector3 strength2 = new Vector3(1f, 1f, 0f) * strength;
        if (type == ShakeTypeEnum.Vertical)
        {
            strength2.x = 0f;
        }

        if (type == ShakeTypeEnum.Horizon)
        {
            strength2.y = 0f;
        }

        KillTweening();
        transform.DOShakePosition(second, strength2, 100).SetLoops((!isLoop) ? 1 : -1).OnKill(OnShakeFinished).OnComplete(OnShakeFinished);
    }

    private void OnShakeFinished()
    {
        transform.localPosition = Vector3.zero;
    }

    public void KillTweening()
    {
        transform.DOKill();
    }

    public void OpenMotionBlur(float second, float scale, Vector3 pos)
    {
    }

    private IEnumerator CameraMotionBlur(float second, float scale, Vector3 pos)
    {
        // float startTime = Time.time;
        // float calTime = 0f;
        // _blur.preview = true;
        // do
        // {
        //     _blur.velocityScale = scale * calTime / second;
        //     Vector2 camPos = _camera.WorldToViewportPoint(pos);
        //     Vector2 blurPos = camPos * -2f + new Vector2(1f, 1f / _camera.aspect);
        //     Vector3 realBlurScale = blurPos;
        //     realBlurScale.z = 1f;
        //     realBlurScale *= 13f;
        //     _blur.previewScale = realBlurScale;
        //     calTime += Time.deltaTime;
        //     yield return null;
        // } while (Time.time - startTime < second);
        //
        // _blur.enabled = false;
        // this._isMotionBlur = false;
        yield return null;
    }

    public void CloseMotionBlur()
    {
    }

    public void EnableGlobalBloom()
    {
        //_globalBloom.enabled = true;
    }

    public void DisableGlobalBloom()
    {
        //_globalBloom.enabled = false;
    }

    public void CameraBloom(float recoveryTime, float waitingTime)
    {
        // if (_bloom == null)
        // {
        //     StartCoroutine(BloomCoroutine(recoveryTime, waitingTime));
        // }
    }

    private IEnumerator BloomCoroutine(float recoveryTime, float waitingTime)
    {
        // _bloom = R.Camera.AddComponent<BloomOptimized>();
        // _bloom.fastBloomShader = (_bloom.fastBloomShader ?? Shader.Find("Hidden/FastBloom"));
        // if (!_bloom.enabled)
        // {
        //     _bloom.enabled = true;
        // }
        //
        // _bloom.intensity = 2.5f;
        // _bloom.threshold = 0.3f;
        // yield return new WaitForSeconds(waitingTime);
        // float calTime = 0f;
        // float startTime = Time.time;
        // while (Time.time - startTime < recoveryTime)
        // {
        //     _bloom.intensity = Mathf.Lerp(2.5f, 0.38f, Mathf.Clamp(calTime, 0f, recoveryTime) / recoveryTime);
        //     _bloom.threshold = Mathf.Lerp(0.3f, 0.4f, Mathf.Clamp(calTime, 0f, recoveryTime) / recoveryTime);
        //     yield return null;
        //     calTime += Time.deltaTime;
        // }
        //
        // _bloom.enabled = false;
        // Object.Destroy(_bloom);
        // _bloom = null;
        yield break;
    }

    private Vector3 CameraPositionClamp(Vector3 pos)
    {
        float num = -pos.z * Mathf.Tan(0.4537856f);
        Vector2 vector = new Vector2(num * _camera.aspect, num);
        Rect cameraRange = GameArea.CameraRange;
        float num2 = cameraRange.width / (-2f * Mathf.Tan(FieldOfView / 2f) * _camera.aspect);
        cameraRange.xMin += vector.x;
        cameraRange.xMax -= vector.x;
        cameraRange.yMin += vector.y;
        cameraRange.yMax -= vector.y;
        Vector3 result;
        result.x = ((cameraRange.xMin >= cameraRange.xMax) ? cameraRange.center.x : Mathf.Clamp(pos.x, cameraRange.xMin, cameraRange.xMax));
        result.y = ((cameraRange.yMin >= cameraRange.yMax) ? cameraRange.center.y : Mathf.Clamp(pos.y, cameraRange.yMin, cameraRange.yMax));
        result.z = pos.z;
        return result;
    }

    private Vector3 CalculateFollowCameraPos()
    {
        // if (Pivot == null)
        // {
        //     return Vector3.zero;
        // }
        //
        // Vector3 position = Pivot.position;
        // Vector3? farestEnemyPosition = R.Enemy.GetFarestEnemyPosition(position, new Rect?(MaxDetectRect));
        // Vector3 vector;
        // if (farestEnemyPosition == null)
        // {
        //     vector = position;
        //     vector.z = -10f;
        // }
        // else
        // {
        //     vector = (position + farestEnemyPosition.Value) / 2f;
        //     float num;
        //     if (Mathf.Abs(((Vector2)position - (Vector2)farestEnemyPosition.Value).Slope()) > new Vector2(16f, 9f).Slope())
        //     {
        //         num = Mathf.Abs(position.y - farestEnemyPosition.Value.y);
        //     }
        //     else
        //     {
        //         num = Mathf.Abs(position.x - farestEnemyPosition.Value.x) * 9f / 16f;
        //     }
        //
        //     vector.z = -(num / 2f) / Mathf.Tan(0.4537856f);
        //     vector.z += -6f;
        //     vector.z = Mathf.Clamp(vector.z, MaxVisibleZ, MinVisibleZ);
        // }
        //
        // float distance = Physics2D.Raycast(vector, Vector2.down, 10f, LayerManager.GroundMask).distance;
        // float num2 = (ManualOffsetY + 2.4f) * (MovableCamera.position.z / -10f);
        // vector += Vector3.up * ((distance <= num2) ? (num2 - distance) : 0f);
        // vector.z += Fv2DeltaZ();
        // return vector;

        return default;
    }

    private Rect CameraRect(float z)
    {
        Vector2 vector;
        vector.y = -z * Mathf.Tan(0.4537856f);
        vector.x = vector.y * _camera.aspect;
        return new Rect
        {
            min = new Vector2(MovableCamera.position.x - vector.x, MovableCamera.position.y - vector.y),
            max = new Vector2(MovableCamera.position.x + vector.x, MovableCamera.position.y + vector.y)
        };
    }

    public void CameraResetPostionAfterSwitchScene()
    {
        IsFollowPivot = true;
        Vector3 vector = CameraPositionClamp(Pivot.position);
        MovableCamera.position = new Vector3(vector.x, vector.y, MovableCamera.position.z);
        UpdateCamera(1000f);
    }

    private void UpdateCamera(float deltaTime)
    {
        Vector3 pos = Vector3.SmoothDamp(MovableCamera.position, CalculateFollowCameraPos(), ref this._currentSpeed, this._smoothTime, float.PositiveInfinity, deltaTime);
        if (!_isLock)
        {
            MovableCamera.position = CameraPositionClamp(pos);
        }
    }

    private const float NormalPreviewScale = 13f;

    public const float CameraDefaultZ = -10f;

    private const float DeltaZ = -2f;

    private const float DefaultFv = 52f;

    public enum ShakeTypeEnum
    {
        Vertical,
        Horizon,
        Rect
    }
}