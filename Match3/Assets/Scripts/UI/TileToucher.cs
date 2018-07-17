using UnityEngine;

public class TileToucher : MonoBehaviour
{
    void OnMouseDown()
    {
        EventsManager.ClickOnTile(transform);
    }
}
