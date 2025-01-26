using Realyteam.Managers;
using Realyteam.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Watch : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private Button reloadLevelButton;
    [SerializeField]
    private Button exitButton;

    private void Start()
    {
        reloadLevelButton.onClick.AddListener(ReloadLevel);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int minutes = Mathf.FloorToInt(GameManager.Instance.Timer / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.Timer % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void ReloadLevel()
    {
        //remplazar por el SceneManager
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void ExitGame()
    {
        //VOLVER AL MENU PRINCIPAL
        Application.Quit();
    }

}
