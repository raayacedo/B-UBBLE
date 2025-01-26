using Realyteam.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Realyteam.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField]
        private float gameDuration = 120f;

        private float timer;
        private bool isGameRunning;

        public Action OnGameStart;
        public Action OnGameOver;
        public Action OnGameFinished;

        public float Timer => timer;
        public bool IsGameRunning => isGameRunning;

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
            BubbleController.Instance.OnBubbleExplode += EndGame;
            BubbleController.Instance.OnGoal += FinishGame;

            timer = gameDuration;
            isGameRunning = false;
            StartGame();
        }

        private void Update()
        {
            if (isGameRunning)
            {
                UpdateTimer();
            }
        }

        private void StartGame()
        {
            if (isGameRunning) return;

            timer = gameDuration;
            isGameRunning = true;
            OnGameStart?.Invoke();
            Debug.Log("Game Started");
        }

        private void EndGame()
        {
            if (!isGameRunning) return;

            isGameRunning = false;
            OnGameOver?.Invoke();

            Debug.Log("Game Over");
        }

        private void FinishGame() 
        {
            if (!isGameRunning) return;
            isGameRunning = false;

            OnGameFinished?.Invoke();

            Debug.Log("Game Finish");


        }

        private void UpdateTimer()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                Debug.Log("Time's up!");
                EndGame();
            }
        }

        private void OnDisable()
        {
            BubbleController.Instance.OnBubbleExplode -= EndGame;
            BubbleController.Instance.OnGoal -= FinishGame;
        }

    }
}
