using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1)]
        private float Volume = 1f;
        [SerializeField]
        private AudioClip click;
        [SerializeField]
        private AudioClip trySwap;
        [SerializeField]
        private AudioClip tileDestroy;
        [SerializeField]
        private AudioClip getCounts;
        [SerializeField]
        private AudioClip tilesFinishedMoving;

        private void OnEnable()
        {
            AddEventListeners();
        }

        private void OnDisable()
        {
            RemoveEventListeners();
        }

        private void AddEventListeners()
        {
            EventsManager.ClickOnTileEvent += OnClickOnTile;
            EventsManager.TilesStartSwapEvent += OnTilesStartSwap;
            EventsManager.GetCountsEvent += OnGetCounts;
            EventsManager.TilesFinishMovingEvent += OnTilesFinishedMoving;
        }

        private void RemoveEventListeners()
        {
            EventsManager.ClickOnTileEvent -= OnClickOnTile;
            EventsManager.TilesStartSwapEvent -= OnTilesStartSwap;
            EventsManager.GetCountsEvent -= OnGetCounts;
            EventsManager.TilesFinishMovingEvent -= OnTilesFinishedMoving;
        }

        private void OnClickOnTile(Transform sender)
        {
            PlaySoundOnCamera(click);
        }

        private void OnTilesStartSwap()
        {
            PlaySoundOnCamera(trySwap);
        }

        private void OnGetCounts(int count)
        {
            PlaySoundOnCamera(getCounts);
            for (int i = 0; i < (count < 6 ? count : 5); i++)
                PlaySoundOnCamera(tileDestroy, count < 4 ? 0.5f : 0.35f);
        }

        private void OnTilesFinishedMoving()
        {
            PlaySoundOnCamera(tilesFinishedMoving);
        }

        private void PlaySoundOnCamera(AudioClip clip, float volume = 0.5f)
        {
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume * Volume);
        }
    }
}
