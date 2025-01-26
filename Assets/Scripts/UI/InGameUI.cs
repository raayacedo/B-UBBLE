using Realyteam.Managers;
using Realyteam.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header ("UI Elements")]
    [SerializeField]
    private UI_Watch watchHud;
    [SerializeField]
    private UI_GameOverPanel gameOverPanel;
    [SerializeField]
    private UI_GameWinPanel winPanel;

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameOver += OnGameOver;
        GameManager.Instance.OnGameFinished += OnWin;

        gameOverPanel.Hide();
        watchHud.Hide();
        winPanel.Hide();
    }

    private void OnGameStart()
    {
        watchHud.Show();
    }

    private void OnGameOver()
    {
        watchHud.Hide();
        gameOverPanel.Show();
    }

    private void OnWin() 
    {
        watchHud.Hide();
        winPanel.Show();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= OnGameStart;
            GameManager.Instance.OnGameOver -= OnGameOver;
            GameManager.Instance.OnGameFinished -= OnWin;
        }
    }
}
