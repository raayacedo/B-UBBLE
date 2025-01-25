using UnityEngine;
using System.Collections.Generic;

namespace Realyteam.Player
{
    public class BubbleController : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField]
        private Rigidbody bubbleRigidbody;
        [SerializeField]
        private float pushForce = 5f;
        [SerializeField]
        private float edgeThreshold = 0.1f;

        [Header("Gravity Settings")]
        [SerializeField]
        private float gravityForce = 9.8f;

        [Header("Health Settings")]
        [SerializeField]
        private float maxHealth = 100f;
        [SerializeField]
        private float healthDecreaseRate = 1f; 
        [SerializeField]
        private float airTimeDamageRate = 5f;
        [SerializeField]
        private float airTimeMultiplier = 10f; 

        private float currentHealth;
        private float timeInAir;
        private bool isGrounded;

        private float bubbleRadius;
        private Transform[] handTransforms;
        private Dictionary<Transform, bool> handInContact;

        public float CurrentHealt => currentHealth;

        #region Unity Callbacks
        private void Start()
        {
            currentHealth = maxHealth;
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

            UpdateAirTime();
            DecreaseHealthOverTime();
        }
        #endregion

        #region Private Methods

        private void UpdateAirTime()
        {
            if (IsGrounded())
            {
                if (!isGrounded && timeInAir > 0)
                {
                    // Calculate damage based on time in air
                    float landingDamage = timeInAir * airTimeMultiplier;
                    TakeDamage(landingDamage);

                    // Reset air time
                    timeInAir = 0f;
                }

                isGrounded = true;
            }
            else
            {
                timeInAir += Time.deltaTime;
                isGrounded = false;
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, bubbleRadius + 0.1f);
        }

        private void DecreaseHealthOverTime()
        {
            TakeDamage(healthDecreaseRate * Time.deltaTime);
        }

        private void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnBubbleDestroyed();
            }
        }

        private void OnBubbleDestroyed()
        {
            Debug.Log("Bubble destroyed!");
            Destroy(gameObject);
        }

        #endregion

        #region Public Methods
        public void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        #endregion
    }
}
