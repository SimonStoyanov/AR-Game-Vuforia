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
    }

    GameObject parent = null;
    private List<GridCreatorSlot> grid = new List<GridCreatorSlot>();

    public float slot_size = 0;
    public Vector2 grid_size = Vector2.zero;
    public Vector2 starting_pos = Vector2.zero;

    public void Awake()
    {        
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag("Grid");

            if (parent != null)
            {
                for (int i = 0; i < parent.transform.childCount; ++i)
                {
                    GameObject go = parent.transform.GetChild(i).gameObject;
                    GridSlotManager gs = go.GetComponent<GridSlotManager>();

                    GridCreatorSlot slot = new GridCreatorSlot(go, gs);

                    grid.Add(slot);
                }
            }
        }
           
    }

    public void CreateGrid()
    {
        ClearGrid();

        GameObject grid_parent = null;

        if(grid_size.x > 0 && grid_size.y > 0)
        {
            grid_parent = new GameObject();
            grid_parent.name = "Grid";
            grid_parent.tag = "Grid";
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
    }

    public void ClearGrid()
    {
        parent = GameObject.FindGameObjectWithTag("Grid");
        DestroyImmediate(parent);

        grid.Clear();
    }

    public List<GridCreatorSlot> GetGrid()
    {
        return grid;
    }

    void DrawGrid()
    {
        for (int i = 0; i < grid.Count; ++i)
        {
            GameObject curr_slot = grid[i].go;

            float half_slot_size = slot_size * 0.5f;
            float quad_x = curr_slot.transform.position.x - half_slot_size;
            float quad_y = curr_slot.transform.position.z - half_slot_size;
            float quad_w = curr_slot.transform.position.x + half_slot_size;
            float quad_z = curr_slot.transform.position.z + half_slot_size;

            Vector3 center = new Vector3(curr_slot.transform.position.x, 0, curr_slot.transform.position.z);

            Vector3 line1p1 = new Vector3(quad_x, 0, quad_y);
            Vector3 line1p2 = new Vector3(quad_x, 0, quad_z);

            Vector3 line2p1 = new Vector3(quad_x, 0, quad_z);
            Vector3 line2p2 = new Vector3(quad_w, 0, quad_z);

            Vector3 line3p1 = new Vector3(quad_w, 0, quad_z);
            Vector3 line3p2 = new Vector3(quad_w, 0, quad_y);

            Vector3 line4p1 = new Vector3(quad_w, 0, quad_y);
            Vector3 line4p2 = new Vector3(quad_x, 0, quad_y);

            Color color = Color.white;

            switch(grid[i].slot_manager.GetSlotType())
            {
                case GridSlotManager.GridSlotType.GST_INTERACTABLE:
                {
                    break;
                }
                case GridSlotManager.GridSlotType.GST_NO_INTERACTABLE:
                {
                    color = Color.magenta;
                    break;
                }
                case GridSlotManager.GridSlotType.GST_PATH:
                {
                    color = Color.green;
                    break;
                }
            }

            Debug.DrawLine(line1p1, center, Color.cyan);
            Debug.DrawLine(line1p1, line1p2);
            Debug.DrawLine(line2p1, line2p2);
            Debug.DrawLine(line3p1, line3p2);
            Debug.DrawLine(line4p1, line4p2);

            Debug.DrawLine(line1p1, center, color);
            Debug.DrawLine(line2p1, center, color);
            Debug.DrawLine(line3p1, center, color);
            Debug.DrawLine(line4p1, center, color);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        DrawGrid();

    }
}
