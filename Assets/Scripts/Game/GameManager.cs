using Realyteam.Player;
using System;
using System.Collections;
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

            //esto es opcional lo puse mientras para arrancar el juego de una
            StartCoroutine(WaitStartGame());
        }

        private IEnumerator WaitStartGame()
        { 
           yield return new WaitForSeconds(3);
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
            OnGameStart?.Invoke();

            BubbleController.Instance.StartGame();
            timer = gameDuration;
            isGameRunning = true;
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
