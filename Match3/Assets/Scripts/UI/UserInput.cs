using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UserInput : MonoBehaviour
    {
        private bool isActive = true;
        private bool isSelected = false;
        private Transform selectedItem;

        void Start()
        {
            AddEventListeners();
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void AddEventListeners()
        {
            EventsManager.ClickOnTileEvent += OnClickOnTile;
            EventsManager.TilesStartSwapEvent += DisableInput;
            EventsManager.TableReadyToInputEvent += EnableInput;
            MinigameController.GameOverEvent += OnGameOver;
        }

        private void RemoveEventListeners()
        {
            EventsManager.ClickOnTileEvent -= OnClickOnTile;
            EventsManager.TilesStartSwapEvent -= DisableInput;
            EventsManager.TableReadyToInputEvent -= EnableInput;
            MinigameController.GameOverEvent -= OnGameOver;

        }

        private void OnClickOnTile(Transform sender)
        {
            if (!isActive)
                return;

            if (isSelected)
            {
                var temp = selectedItem;
                DisableInput();
                isActive = true;
                if (selectedItem != sender)
                {
                    EventsManager.ClickToTrySwap(temp, sender);
                }
            }
            else
            {
                isSelected = true;
                selectedItem = sender;
                ChangeScaleSelectedItem(true);
            }
        }

        private void ChangeScaleSelectedItem(bool toMax)
        {
            if (selectedItem == null || selectedItem.transform == null)
                return;

            if (toMax)
                selectedItem.transform.localScale *= 1.3f;
            else
                selectedItem.transform.localScale /= 1.3f;
        }

        private void EnableInput()
        {
            isActive = true;
            isSelected = false;
            selectedItem = null;
        }

        private void DisableInput()
        {
            ChangeScaleSelectedItem(false);
            isActive = false;
            isSelected = false;
            selectedItem = null;
        }

        private void OnGameOver(bool value)
        {
            DisableInput();
            this.enabled = false;
        }
    }
}
