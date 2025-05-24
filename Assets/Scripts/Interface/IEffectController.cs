using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectController
{
    public void CameraShake(Vector2 direction);

    public void Freeze(float duration);

    public GameObject PlayerDashFX(Vector3 position);

    
}
