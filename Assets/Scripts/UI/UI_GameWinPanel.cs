using Realyteam.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameWinPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Button creditsButton;
    [SerializeField]
    private Button exitButton;

    private void Start()
    {
        creditsButton.onClick.AddListener(CreditScene);
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

    private void CreditScene()
    {
        //remplazar por el SceneManager a los creditos
    }

    private void ExitGame()
    {
        //VOLVER AL MENU PRINCIPAL
        Application.Quit();
    }
}
