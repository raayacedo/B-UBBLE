using UnityEngine;

namespace Realyteam.Player
{
    public class HandContactController : MonoBehaviour
    {
        [SerializeField]
        private GameObject hand;
  
        private void Update()
        {
            if (hand != null)
            {
                transform.position = hand.transform.position;
                transform.rotation = hand.transform.rotation;
            }
        }
    }
}