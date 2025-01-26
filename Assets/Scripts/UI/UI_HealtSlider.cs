using Realyteam.Player;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthSlider : MonoBehaviour
{
    [Header("Health Slider")]
    [SerializeField]
    private Slider healthSlider; 
    [SerializeField]
    private Image indicatorImage;
    [SerializeField]
    private Sprite goodHealthSprite; 
    [SerializeField]
    private Sprite badHealthSprite; 

    [Header("Color Settings")]
    [SerializeField]
    private Color goodHealthColor = Color.green; 
    [SerializeField]
    private Color badHealthColor = Color.red; 

    private void Update()
    {
        if (BubbleController.Instance == null) return;

        healthSlider.value = BubbleController.Instance.CurrentHealth;
        UpdateSliderColor();
        UpdateIndicatorImage();
    }

    private void UpdateSliderColor()
    {
        float healthPercentage = healthSlider.value / healthSlider.maxValue;
        Color currentColor = Color.Lerp(badHealthColor, goodHealthColor, healthPercentage);
        healthSlider.fillRect.GetComponent<Image>().color = currentColor;
    }

    private void UpdateIndicatorImage()
    {
        float healthPercentage = healthSlider.value / healthSlider.maxValue;
        if (healthPercentage <= 0.25f)
        {
            indicatorImage.sprite = badHealthSprite;
        }
        else
        {
            indicatorImage.sprite = goodHealthSprite;
        }
    }
}
