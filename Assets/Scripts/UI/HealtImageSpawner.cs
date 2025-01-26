using UnityEngine;

namespace Realyteam.Player
{
    public class HealtImageSpawner : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject healtParticle;

        private void Update()
        {
            if (BubbleController.Instance.OnHealthRestoring)
            {
                healtParticle.SetActive(true);
            }
            else
            {
                healtParticle.SetActive(false);
            }
        }
    }
}