using UnityEngine;
using System.Collections.Generic;

namespace Realyteam.Player
{
    public class BubbleController : MonoBehaviour
    {
        public static BubbleController Instance { get; private set; }

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
        private float airTimeThreshold = 3f;

        [Header("Health Zone Settings")]
        [SerializeField]
        private float healthZoneRate = 0.1f;
        [SerializeField]
        private float DamageZoneRate = 0.1f;

        private float currentHealth;
        private float timeInAir;
        private bool isGrounded;

        private float bubbleRadius;
        private Transform[] handTransforms;
        private Dictionary<Transform, bool> handInContact;

        public float CurrentHealth => currentHealth;
        public bool OnAirDamage { get; private set; }
        public bool OnHealthRestoring { get; private set; }
        public bool OnDamageTaken { get; private set; }

        #region Unity Callbacks
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

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

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HealthZone"))
            {
                Debug.Log("In healing zone");

                Heal(healthZoneRate);
                OnHealthRestoring = true;
            }
            else
            {
                OnHealthRestoring = false;
            }

            if (other.CompareTag("DamageZone"))
            {
                TakeDamage(DamageZoneRate);
                OnDamageTaken = true;
            }
            else
            {
                OnDamageTaken = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("HealthZone"))
            {
                OnHealthRestoring = false;
            }

            if (other.CompareTag("DamageZone"))
            {
                OnDamageTaken = false;
            }
        }
        #endregion

        #region Private Methods

        private void UpdateAirTime()
        {
            if (IsGrounded())
            {
                timeInAir = 0f;
                isGrounded = true;
                OnAirDamage = false;
            }
            else
            {
                timeInAir += Time.deltaTime;

                if (timeInAir > airTimeThreshold)
                {
                    float airTimeDamage = airTimeDamageRate * Time.deltaTime;
                    TakeDamage(airTimeDamage);
                    OnAirDamage = true;
                }
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
            // Destroy(gameObject);
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

        public void InflictDamage(float amount)
        {
            TakeDamage(amount);
        }
        #endregion
    }
}
