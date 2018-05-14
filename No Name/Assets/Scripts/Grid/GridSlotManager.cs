using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlotManager : MonoBehaviour
{
    public enum GridSlotType
    {
        GST_INTERACTABLE,
        GST_NO_INTERACTABLE,
    }

    [SerializeField]
    private GridSlotType slot_type = GridSlotType.GST_INTERACTABLE;

    private GridManager.Grid grid = null;

    public void SetGridManager(GridManager.Grid gm)
    {
        grid = gm;
    }

    public GridSlotType GetSlotType()
    {
        return slot_type;
    }

    private void OnMouseDown()
    {
        if(grid != null)
            grid.GridOnMouseDownCallback(this.gameObject);
    }
}
