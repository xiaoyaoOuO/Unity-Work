using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour, ICamera
{
    [Header("主摄像机引用")]
    [SerializeField] private Camera mainCamera;
    private Vector2 offset;

    [Header("当前房间/范围边界")]
    [SerializeField] private Bounds bounds;

    [Header("抖动参数")]
    [SerializeField] private float ShakeStrength = 1;
    [SerializeField]
    private AnimationCurve ShakeCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0, -1.4f, -7.9f, -7.9f),
        new Keyframe(0.27f, 0.78f, 23.4f, 23.4f),
        new Keyframe(0.54f, -0.12f, 22.6f, 22.6f),
        new Keyframe(0.75f, 0.042f, 9.23f, 9.23f),
        new Keyframe(0.9f, -0.02f, 5.8f, 5.8f),
        new Keyframe(0.95f, -0.006f, -3.0f, -3.0f),
        new Keyframe(1, 0, 0, 0)
    });

    private Coroutine roomTransitionCoroutine;

    public void Shake(Vector2 dir, float duration)
    {
        StartCoroutine(DOShake(dir, duration));
    }

    public IEnumerator DOShake(Vector2 dir, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = ShakeCurve.Evaluate(elapsed / duration) * ShakeStrength;
            offset = new Vector2(dir.x * t, dir.y * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        offset = Vector2.zero;
    }

    public void SetCameraPosition(Vector2 pos)
    {
        mainCamera.transform.position = new Vector3(pos.x + offset.x, pos.y + offset.y, mainCamera.transform.position.z);
    }

    public void UpdateCameraPosition(Vector3 targetPosition)
    {
        var from = mainCamera.transform.position;
        var target = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
        var multiplier = 1f;

        float halfW = mainCamera.orthographicSize * mainCamera.aspect;
        float halfH = mainCamera.orthographicSize;
        target.x = Mathf.Clamp(target.x, bounds.min.x + halfW, bounds.max.x - halfW);
        target.y = Mathf.Clamp(target.y, bounds.min.y + halfH, bounds.max.y - halfH);

        Vector2 cameraPosition = from + (target - from) * (1f - Mathf.Pow(0.01f / multiplier, Time.deltaTime));
        SetCameraPosition(cameraPosition);
    }

    public void TransitionToRoom(Bounds newBounds)
    {
        if (roomTransitionCoroutine != null)
            StopCoroutine(roomTransitionCoroutine);
        roomTransitionCoroutine = StartCoroutine(RoomTransitionCoroutine(newBounds));
    }

    private IEnumerator RoomTransitionCoroutine(Bounds newBounds)
    {
        bounds = newBounds;
        roomTransitionCoroutine = null;
        yield break;   
    }
}
