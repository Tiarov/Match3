using System;
using UnityEngine;

public class EventsManager
{
    public static Action<Transform> ClickOnTileEvent;
    public static Action<Transform, Transform> ClickToSwapEvent;

    public static Action TilesStartSwapEvent;
    public static Action TilesFinishSwapEvent;
    public static Action TilesStartDestroingEvent;
    public static Action TilesFinishDestroingEvent;
    public static Action TilesStartMovingEvent;
    public static Action TilesFinishMovingEvent;
    public static Action NewTilesCreateEvent;
    public static Action<int> GetCountsEvent;
    public static Action TableReadyToInputEvent;

    public static void ClickOnTile(Transform sender)
    {
        if (ClickOnTileEvent != null)
            ClickOnTileEvent(sender);
    }

    public static void ClickToTrySwap(Transform sender1, Transform sender2)
    {
        if (ClickToSwapEvent != null)
            ClickToSwapEvent(sender1, sender2);
    }

    public static void TilesStartSwap()
    {
        if (TilesStartSwapEvent != null)
            TilesStartSwapEvent();
    }

    public static void TilesFinishSwap()
    {
        if (TilesFinishSwapEvent != null)
            TilesFinishSwapEvent();
    }

    public static void TilesStartDestroing()
    {
        if (TilesStartDestroingEvent != null)
            TilesStartDestroingEvent();
    }

    public static void TilesFinishDestroing()
    {
        if (TilesFinishDestroingEvent != null)
            TilesFinishDestroingEvent();
    }

    public static void TilesStartMoving()
    {
        if (TilesStartMovingEvent != null)
            TilesStartMovingEvent();
    }

    public static void TilesFinishMoving()
    {
        if (TilesFinishMovingEvent != null)
            TilesFinishMovingEvent();
    }

    public static void NewTilesCreate()
    {
        if (NewTilesCreateEvent != null)
            NewTilesCreateEvent();
    }

    public static void GetCounts(int count)
    {
        if (GetCountsEvent != null)
            GetCountsEvent(count);
    }

    public static void TableReadyToInput()
    {
        if (TableReadyToInputEvent != null)
            TableReadyToInputEvent();
    }
}
