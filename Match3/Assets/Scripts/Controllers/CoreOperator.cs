using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Match3Engine;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CoreOperator : MonoBehaviour
    {
        [SerializeField]
        private int SwapSpeed = 6;
        [SerializeField]
        private int FallSpeed = 5;

        private BoardViewManager _manager;
        private Match3Table _coreTable;
        private List<TileViewModel> _viewModels;
        private delegate void SomeMethod();

        public int XSize { get; private set; }
        public int YSize { get; private set; }
        public int VariableCount { get; private set; }

        void OnEnable()
        {
            AddEventListeners();
        }

        void OnDisable()
        {
            RemoveEventListeners();
        }

        #region Ititialize methods

        public void InitCoreOperator(int xCount, int yCount, BoardViewManager manager)
        {
            InitializeProperties(xCount, yCount);
            InitalizeManager(manager);
            InitializeMatch3Table();
        }

        private void InitializeProperties(int xCount, int yCount)
        {
            XSize = xCount;
            YSize = yCount;

            if (XSize < 4 || YSize < 4)
                VariableCount = 3;
            else
                VariableCount = 4;

            SwapSpeed = yCount + 1;
            FallSpeed = yCount - 1;
        }

        private void InitalizeManager(BoardViewManager manager)
        {
            _manager = manager;
            _manager.Initialize(XSize * YSize, _manager.transform);
        }

        private void InitializeMatch3Table()
        {
            _coreTable = new Match3Table(XSize, YSize, VariableCount);

            _viewModels = new List<TileViewModel>(XSize * YSize);

            foreach (var model in _coreTable.Table)
            {
                _viewModels.Add(CreateTileViewModel(model, new Vector2(model.X, model.Y)));
            }
        }

        #endregion Ititialize methods
    
        private void CreateTiles(IEnumerable<Match3TileModel> m3TileModelsList, bool yOffset = false)
        {
            foreach (var m3 in m3TileModelsList)
            {
                _viewModels.Add(CreateTileViewModel(m3, new Vector2(m3.X, yOffset ? YSize + m3.Y : m3.Y)));
            }
        }

        private void RemoveTiles(IEnumerable<Match3TileModel> executeList)
        {
            foreach (var model in executeList)
            {
                var tileViewModel = _viewModels.Find(v => v.Model == model);
                if (tileViewModel != null)
                {
                    _viewModels.Remove(tileViewModel);
                    _manager.RemoveTile(tileViewModel.Transform);
                }
            }
        }

        private TileViewModel CreateTileViewModel(Match3TileModel model, Vector2 position)
        {
            var po = _manager.CreateTile(model.Index, position);
            return new TileViewModel(model, po.transform, po);
        }

        private void UpdateSwapTilesPositions(TileViewModel model1, TileViewModel model2)
        {
            EventsManager.TilesStartSwap();

            _manager.UpdatePosition(new[] { model1, model2 }, SwapSpeed);
        }

        private void UpdateTilesPositions()
        {
            _manager.UpdatePosition(_viewModels, FallSpeed);
        }

        private void HandleSwap(Transform t1, Transform t2)
        {
            if (t1 == t2 && _coreTable == null)
                return;

            TileViewModel firstTile = null;
            TileViewModel secondTile = null;

            foreach (var vm in _viewModels)
            {
                if (vm.Transform == t1)
                    firstTile = vm;
                if (vm.Transform == t2)
                    secondTile = vm;

                if (firstTile != null && secondTile != null && _coreTable.TrySwap(firstTile.Model, secondTile.Model))
                {
                    UpdateSwapTilesPositions(firstTile, secondTile);
                    return;
                }
            }
        }

        private bool TryClean()
        {
            var executeList = new List<Match3TileModel>();
            if (!_coreTable.TryCleanTiles(executeList))
            {
                return false;
            }

            EventsManager.GetCounts(executeList.Count);
            RemoveTiles(executeList);

            return true;
        }

        private void NextIteration()
        {
            if (TryClean())
            {
                _coreTable.SdvigTiles(YSize);
                CreateTiles(_coreTable.GenerateAddictiveTiles(), true);
                UpdateTilesPositions();
            }
            else
            {
                EventsManager.TableReadyToInput();
            }
        }

        private IEnumerator AdjornedLaunching(SomeMethod method, float timeout = 0.5f)
        {
            yield return new WaitForSeconds(timeout);
            method();
        }

        #region EventHandlers

        private void AddEventListeners()
        {
            EventsManager.ClickToSwapEvent += OnClickToSwap;
            EventsManager.TilesFinishSwapEvent += OnTilesFinishSwap;
            EventsManager.TilesFinishMovingEvent += OnTilesFinishMoving;
        }

        private void RemoveEventListeners()
        {
            EventsManager.ClickToSwapEvent -= OnClickToSwap;
            EventsManager.TilesFinishSwapEvent -= OnTilesFinishSwap;
            EventsManager.TilesFinishMovingEvent -= OnTilesFinishMoving;
        }

        private void OnClickToSwap(Transform sender1, Transform sender2)
        {
            HandleSwap(sender1, sender2);
        }

        private void OnTilesFinishSwap()
        {
            StartCoroutine(AdjornedLaunching(NextIteration));
        }

        private void OnTilesFinishMoving()
        {
            StartCoroutine(AdjornedLaunching(NextIteration));
        }

        #endregion EventHandlers
    }
}
