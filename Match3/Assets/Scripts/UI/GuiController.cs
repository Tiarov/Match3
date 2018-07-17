using System;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GuiController : MonoBehaviour
    {
        [SerializeField]
        private Text ScoreText;
        [SerializeField]
        private Text TargetScoreText;
        [SerializeField]
        private Text SwapCount;

        private bool show = false;
        private string message;
        private Rect windowRect = new Rect(0, 0, 300, 110);

        private void OnEnable()
        {
            AddEventListeners();
        }

        private void OnDisable()
        {
            RemoveEventListeners();
            show = false;
        }
        void OnGUI()
        {
            if (show)
            {
                windowRect = GUI.Window(0, windowRect, DialogWindow, message);
            }
        }

        private void AddEventListeners()
        {
            MinigameController.InitGuiParamsEvent += OnInitGuiParams;
            MinigameController.ChangeScoreEvent += OnChangingScoreCount;
            MinigameController.ChangeSwapCountEvent += OnChangingSwapCount;
            MinigameController.GameOverEvent += OnGameOver;
        }

        private void RemoveEventListeners()
        {
            MinigameController.InitGuiParamsEvent -= OnInitGuiParams;
            MinigameController.ChangeScoreEvent -= OnChangingScoreCount;
            MinigameController.ChangeSwapCountEvent -= OnChangingSwapCount;
            MinigameController.GameOverEvent -= OnGameOver;
        }

        private void OnChangingSwapCount(int count)
        {
            SwapCount.text = count.ToString();
        }

        private void OnChangingScoreCount(int count)
        {
            ScoreText.text = count.ToString();
        }

        private void OnInitGuiParams(int score, int targetScore, int swapCount)
        {
            ScoreText.text = score.ToString();
            TargetScoreText.text = targetScore.ToString();
            SwapCount.text = swapCount.ToString();
        }

        private void OnGameOver(bool isWin)
        {
            windowRect.center = new Vector2(Screen.width / 2, Screen.height / 2);
            message = String.Format("You are {0}! Your score is {1} / {2}", (isWin ? "Win! " : "lose! "),
                ScoreText.text, TargetScoreText.text);
            show = true;
        }

        private void DialogWindow(int windowID)
        {
            if (GUI.Button(new Rect(5, 25, windowRect.width - 10, 25), "Restart"))
            {
                Application.LoadLevel(1);
            }

            if (GUI.Button(new Rect(5, 60, windowRect.width - 10, 25), "Main Menu"))
            {
                Application.LoadLevel(0);
            }
        }
    }
}
