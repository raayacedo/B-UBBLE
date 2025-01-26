using UnityEngine;

namespace Realyteam.Player
{
    public class FeedbackSphere : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color healColor; 
        [SerializeField] private Color damageColor; 
        [SerializeField] private float transitionSpeed = 2f;

        private Material sphereMaterial;
        private Color transparentColor;
        private BubbleController bubbleController;
        private bool isHealing;
        private bool isTakingDamage;
        private bool isAirDamage;

        private float pulseTime;

        private void Start()
        {
            sphereMaterial = GetComponent<Renderer>().material;

            transparentColor = new Color(0, 0, 0, 0);

            bubbleController = BubbleController.Instance;
        }

        private void Update()
        {
            if (bubbleController == null || bubbleController.OnDead) 
            {
                ResetTransparency();
                return;
            } 

            isHealing = bubbleController.OnHealthRestoring;
            isTakingDamage = bubbleController.OnDamageTaken;
            isAirDamage = bubbleController.OnAirDamage;

            if (isHealing)
            {
                AnimatePulse(healColor);
            }
            else if (isTakingDamage || isAirDamage)
            {
                AnimatePulse(damageColor);
            }
            else
            {
                ResetTransparency();
            }
        }

        private void AnimatePulse(Color targetColor)
        {
            pulseTime += Time.deltaTime * transitionSpeed;
            float alpha = Mathf.PingPong(pulseTime, targetColor.a);
            Color animatedColor = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);
            sphereMaterial.color = animatedColor;
        }

        private void ResetTransparency()
        {
            pulseTime = 0;
            sphereMaterial.color = transparentColor;
        }
    }
}
