using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlotManager : MonoBehaviour
{
    public enum GridSlotType
    {
        GST_INTERACTABLE,
        GST_NO_INTERACTABLE,
        GST_PATH,
    }

    [SerializeField]
    private GridSlotType slot_type = GridSlotType.GST_INTERACTABLE;

    private GridManager gird_manager = null;

    public void SetGridManager(GridManager gm)
    {
        gird_manager = gm;
    }

    public GridSlotType GetSlotType()
    {
        return slot_type;
    }

    private void OnMouseDown()
    {
        gird_manager.GridOnMouseDownCallback(this.gameObject);
    }
}
