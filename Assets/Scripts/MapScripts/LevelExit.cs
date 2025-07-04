// LevelExit.cs
using UnityEngine;
public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ÏÔÊ¾Í¨¹ØUI
            WinScreenUI winScreen = FindObjectOfType<WinScreenUI>();
            if (winScreen != null)
            {
                winScreen.ShowWinScreen();
            }
        }
    }
}