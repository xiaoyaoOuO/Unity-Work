using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectController
{
    public void CameraShake(Vector2 direction);

    public void Freeze(float duration);

    public GameObject PlayerDashFX(Vector3 position);
    public void PlayerJumpFX(Vector3 position);
    public void PlayerLandFX(Vector3 position);
    public GameObject PlayerWallSlideFX(Vector3 position);
    public void PlayerWallJumpFX(Vector3 position);
    public GameObject PlayerDashTrailFX(Vector3 position);
    
}
