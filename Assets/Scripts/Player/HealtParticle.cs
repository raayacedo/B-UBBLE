using UnityEngine;

namespace Realyteam.Player
{
    public class HealtParticle : MonoBehaviour
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