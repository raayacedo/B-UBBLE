using UnityEngine;

namespace Realyteam.Player
{
    public class FeedbackSphere : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color healColor = new Color(0, 1, 0, 0.5f); 
        [SerializeField] private Color damageColor = new Color(1, 0, 0, 0.5f); 
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
            if (bubbleController == null) return;

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
            // Incrementa el tiempo del pulso.
            pulseTime += Time.deltaTime * transitionSpeed;

            // Oscila el valor de alfa entre 0 y el valor alfa del color objetivo usando Mathf.PingPong.
            float alpha = Mathf.PingPong(pulseTime, targetColor.a);

            // Combinar el color objetivo con la transparencia pulsante.
            Color animatedColor = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);
            sphereMaterial.color = animatedColor;
        }

        private void ResetTransparency()
        {
            pulseTime = 0; // Reinicia el tiempo de la animación.
            sphereMaterial.color = transparentColor;
        }
    }
}
