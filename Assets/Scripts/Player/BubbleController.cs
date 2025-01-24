using UnityEngine;
using System.Collections.Generic;

namespace Realyteam.Player
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody bubbleRigidbody;
        [SerializeField]
        private float pushForce = 5f;
        [SerializeField]
        private float edgeThreshold = 0.1f;
        [SerializeField]
        private float gravityForce = 9.8f;

        private float bubbleRadius;
        private Transform[] handTransforms;
        private Dictionary<Transform, bool> handInContact;

        private void Start()
        {
            bubbleRadius = GetComponent<SphereCollider>().radius * transform.localScale.x;

            GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
            handTransforms = new Transform[hands.Length];
            handInContact = new Dictionary<Transform, bool>();

            for (int i = 0; i < hands.Length; i++)
            {
                handTransforms[i] = hands[i].transform;
                handInContact[handTransforms[i]] = false;
            }
        }

        private void FixedUpdate()
        {
            Vector3 downwardForce = Vector3.up * gravityForce;
            bubbleRigidbody.AddForce(downwardForce, ForceMode.Acceleration);

            foreach (Transform hand in handTransforms)
            {
                Vector3 handPosition = hand.position;
                float distanceFromCenter = Vector3.Distance(handPosition, transform.position);

                if (distanceFromCenter >= bubbleRadius - edgeThreshold)
                {
                    if (!handInContact[hand])
                    {
                        Vector3 pushDirection = (handPosition - transform.position).normalized;
                        bubbleRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                        handInContact[hand] = true;
                    }
                }
                else
                {
                    handInContact[hand] = false;
                }
            }
        }
    }
}