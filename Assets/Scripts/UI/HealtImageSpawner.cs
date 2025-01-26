using UnityEngine;

namespace Realyteam.Player
{
    public class HealtImageSpawner : MonoBehaviour
    {
        [Header("Settings")]
        public ParticleSystem healtParticle;

        private bool isPlaying = false;

        private void Update()
        {
            if (BubbleController.Instance.OnHealthRestoring)
            {
                if (!isPlaying)
                {
                    healtParticle.Play();
                    isPlaying = true;
                }
            }
            else
            {
                if (isPlaying)
                {
                    healtParticle.Stop();
                    isPlaying = false;
                }
            }
        }
    }
}