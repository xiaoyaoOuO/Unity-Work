// GemCollectible.cs
using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    public Animator animator;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            animator.Play("GemCollectAnim");
            Destroy(gameObject, 1.75f); // ���ݶ���ʱ������
            other.GetComponent<Player>().RefillDash();
        }
    }
}