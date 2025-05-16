using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour, IEffectController
{
    public void CameraShake(Vector2 direction)
    {
        Game.instance.cameraManager.Shake(direction,0.2f);
    }

    public void Freeze(float duration)
    {
        StartCoroutine(Game.instance.Freeze(duration));
    }
}
