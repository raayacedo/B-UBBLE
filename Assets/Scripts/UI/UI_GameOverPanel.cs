using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPanel : MonoBehaviour
{
    [Header("UI Elements")]
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
