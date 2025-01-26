using Realyteam.Managers;
using Realyteam.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject HUD;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private Button reloadLevelButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private GameObject gameOverPanel;


    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameOver += OnGameOver;

        reloadLevelButton.onClick.AddListener(ReloadLevel);
        exitButton.onClick.AddListener(ExitGame);

        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        healthSlider.value = BubbleController.Instance.CurrentHealth;

        int minutes = Mathf.FloorToInt(GameManager.Instance.Timer / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.Timer % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";

    }

    private void OnGameStart()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnGameOver()
    {
        HUD.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= OnGameStart;
            GameManager.Instance.OnGameOver -= OnGameOver;
        }
    }
}
