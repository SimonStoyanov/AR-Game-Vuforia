using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreator : MonoBehaviour
{
    public class GridCreatorSlot
    {
        public GridCreatorSlot(GameObject _go, GridSlotManager _slot_manager)
        {
            go = _go;
            slot_manager = _slot_manager;
        }

        public GameObject go;
        public GridSlotManager slot_manager = null;
        public List<GameObject> near_childs = new List<GameObject>();
    }

    GameObject curr_parent = null;

    public float slot_size = 0;
    public Vector2 grid_size = Vector2.zero;
    public Vector2 starting_pos = Vector2.zero;

    public void Awake()
    {        
        if (curr_parent == null)
        {
            curr_parent = GameObject.FindGameObjectWithTag("Grid");
        }      
    }

    public void CreateGrid()
    {
        List<GridCreatorSlot> grid = new List<GridCreatorSlot>();

        ClearGrid();

        GameObject grid_parent = null;
        GridInstance grid_instance = null;

        if (grid_size.x > 0 && grid_size.y > 0)
        {
            grid_parent = new GameObject();
            grid_parent.name = "Grid";
            grid_parent.tag = "Grid";

            grid_instance = grid_parent.AddComponent<GridInstance>();
        }

        for (int i = 0; i < grid_size.x; ++i)
        {
            for (int y = 0; y < grid_size.y; ++y)
            {
                Vector2 slot_pos = new Vector2((i * slot_size) + starting_pos.x, (y * slot_size) + starting_pos.y);

                GameObject go = new GameObject();
                go.transform.position = new Vector3(slot_pos.x, 0, slot_pos.y);
                go.transform.rotation = Quaternion.Euler(90, 0, 0);
                go.transform.localScale = new Vector3(slot_size, slot_size, 1);
                go.name = "Grid [" + i + "] " + "[" + y + "]";

                go.transform.parent = grid_parent.transform;

                GridSlotManager gs = go.AddComponent<GridSlotManager>();

                GridCreatorSlot slot = new GridCreatorSlot(go, gs);

                grid.Add(slot);
            }
        }

        if(grid_instance != null)
        {
            grid_instance.CreateGrid(slot_size);
        }
    }

    public void ClearGrid()
    {
        curr_parent = GameObject.FindGameObjectWithTag("Grid");
        DestroyImmediate(curr_parent);
    }

    public void FinishGrid()
    {
        curr_parent = GameObject.FindGameObjectWithTag("Grid");
        curr_parent.tag = "FinishedGrid";
        curr_parent.name = "FinishedGrid";
    }
}
