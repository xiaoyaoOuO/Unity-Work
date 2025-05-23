using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour,ICamera
{
        [SerializeField]
        private Camera mainCamera;
        private Vector2 offset;

        [SerializeField] private Bounds bounds;

        [SerializeField]
        private float ShakeStrength = 1;
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
                float x = dir.x * t;
                float y = dir.y * t;

                offset = new Vector2(x, y);
                elapsed += Time.deltaTime;
                yield return null;
            }
            offset = Vector2.zero;
        }

        public void SetCameraPosition(Vector2 pos)
        {
            mainCamera.transform.position = new Vector3(pos.x + offset.x, pos.y+offset.y, mainCamera.transform.position.z);
        }

        public void UpdateCameraPosition(Vector3 targetPosition)
        {
            var from = mainCamera.transform.position;
            var target = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
            var multiplier = 1f;


            // target.x = Mathf.Clamp(target.x, bounds.min.x + 3200 / 100 / 2f, bounds.max.x - 3200 / 100 / 2f);
            // target.y = Mathf.Clamp(target.y, bounds.min.y + 1800 / 100 / 2f, bounds.max.y - 1800 / 100 / 2f);

            Vector2 cameraPosition = from + (target - from) * (1f - (float)Mathf.Pow(0.01f / multiplier, Time.deltaTime));
            SetCameraPosition(cameraPosition);
        }

}
