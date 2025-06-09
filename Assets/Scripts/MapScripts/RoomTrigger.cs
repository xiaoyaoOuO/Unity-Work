using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomTrigger : MonoBehaviour
{
    [Header("ÉãÏñ»ú¹ÜÀíÆ÷")]
    public CameraManager cameraManager;

    private Bounds roomBounds;

    void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        roomBounds = col.bounds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraManager.TransitionToRoom(roomBounds);
        }
    }
}
