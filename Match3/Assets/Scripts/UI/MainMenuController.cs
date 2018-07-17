using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        void Awake()
        {
            PlayerPrefs.SetInt(MinigameController.SizeCountKey, ResolveSize());
        }

        private int ResolveSize()
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    return 3;
                case 1:
                    return 5;
                case 2:
                    return 6;
                case 3:
                    return 9;
                case 4:
                    return 15;
            }

            return 0;
        }
    }
}
