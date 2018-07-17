using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MinigameController : MonoBehaviour
    {
        public const string SizeCountKey = "SizeCountKey";

        [SerializeField]
        private Transform TileParent;
        [Range(3, 15)]
        public int xSize = 6;
        [Range(3, 15)]
        public int ySize = 6;

        private int minSize = 3;
        private int maxSize = 15;

        private int currentScore = 0;
        private int maxScore = 200;
        private int SwapCount = 30;

        public static Action<int, int, int> InitGuiParamsEvent;
        public static Action<int> ChangeScoreEvent;
        public static Action<int> ChangeSwapCountEvent;
        public static Action<bool> GameOverEvent;

        void Start()
        {
            InitializeProperies();

            UpdateParentObjectTransform();
            InitializeCoreOperator();

            AddEventListeners();

            MessageToGui();
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void InitializeProperies()
        {
            xSize = ySize = PlayerPrefs.GetInt(SizeCountKey, 5);
            PlayerPrefs.SetInt(SizeCountKey, xSize);
            if (xSize == 3)
                maxScore = 100;
        }

        private void UpdateParentObjectTransform()
        {
            float koef = 1;

            if (ySize > 2)
            {
                koef = Mathf.Pow(0.92f, ySize - 9);
            }
            if (xSize > 2 && xSize > ySize)
            {
                koef = Mathf.Pow(0.92f, xSize - 9);
            }

            TileParent.localScale *= koef;
            TileParent.position =
                new Vector3(Camera.main.transform.position.x - ((float)xSize - 1) / 2, Camera.main.transform.position.y - ((float)ySize - 1) / 2,
                    z: 0) * koef;
        }

        private void InitializeCoreOperator()
        {
            var cO = GetComponent<CoreOperator>();
            if (!cO)
                cO = gameObject.AddComponent<CoreOperator>();

            var bVM = TileParent.GetComponent<BoardViewManager>();
            if (!bVM)
                bVM = gameObject.AddComponent<BoardViewManager>();

            cO.InitCoreOperator(xSize, ySize, bVM);
        }

        private void MessageToGui()
        {
            if (InitGuiParamsEvent != null)
                InitGuiParamsEvent(currentScore, maxScore, SwapCount);
        }

        private void AddEventListeners()
        {
            EventsManager.GetCountsEvent += OnGetCounts;
            EventsManager.TilesStartSwapEvent += OnTilesStartSwap;
            EventsManager.TableReadyToInputEvent += OnTableReadyToInput;
        }

        private void RemoveEventListeners()
        {
            EventsManager.GetCountsEvent -= OnGetCounts;
            EventsManager.TilesStartSwapEvent -= OnTilesStartSwap;
            EventsManager.TableReadyToInputEvent -= OnTableReadyToInput;
        }

        private void OnGetCounts(int count)
        {
            currentScore += count;
            if (ChangeScoreEvent != null)
                ChangeScoreEvent(currentScore);
        }

        private void OnTilesStartSwap()
        {
            if (--SwapCount < 0)
                SwapCount = 0;
            if (ChangeSwapCountEvent != null)
                ChangeSwapCountEvent(SwapCount);
        }

        private void OnTableReadyToInput()
        {
            if (SwapCount <= 0)
            {
                if (GameOverEvent != null)
                    GameOverEvent(currentScore >= maxScore);
            }
        }
    }
}
