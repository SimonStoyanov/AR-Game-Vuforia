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

                CalculateGridChilds();
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

            CalculateGridChilds();
        }

    }

    private void CalculateGridChilds()
    {
        for (int i = 0; i < grid.Count; ++i)
        {
            GridCreatorSlot curr_slot = grid[i];

            curr_slot.near_childs.Clear();

            Vector3 curr_pos = curr_slot.go.transform.position;

            Vector3 up_pos = new Vector3(curr_pos.x, 0, curr_pos.z + slot_size);
            Vector3 down_pos = new Vector3(curr_pos.x, 0, curr_pos.z - slot_size);
            Vector3 left_pos = new Vector3(curr_pos.x - slot_size, 0, curr_pos.z);
            Vector3 right_pos = new Vector3(curr_pos.x + slot_size, 0, curr_pos.z);

            GridCreatorSlot up_slot = GetCloserSlot(up_pos, 0.2f);
            GridCreatorSlot down_slot = GetCloserSlot(down_pos, 0.2f);
            GridCreatorSlot left_slot = GetCloserSlot(left_pos, 0.2f);
            GridCreatorSlot right_slot = GetCloserSlot(right_pos, 0.2f);

            if (up_slot != null && up_slot != curr_slot && !curr_slot.near_childs.Contains(up_slot.go))
                curr_slot.near_childs.Add(up_slot.go);

            if (down_slot != null && down_slot != curr_slot && !curr_slot.near_childs.Contains(down_slot.go))
                curr_slot.near_childs.Add(down_slot.go);

            if (left_slot != null && left_slot != curr_slot && !curr_slot.near_childs.Contains(left_slot.go))
                curr_slot.near_childs.Add(left_slot.go);

            if (right_slot != null && right_slot != curr_slot && !curr_slot.near_childs.Contains(right_slot.go))
                curr_slot.near_childs.Add(right_slot.go);
        }
    }


    private GridCreatorSlot GetCloserSlot(Vector3 pos, float max_distance)
    {
        GridCreatorSlot ret = null;

        float min_dist = float.PositiveInfinity;
        for (int i = 0; i < grid.Count; ++i)
        {
            GridCreatorSlot curr_slot = grid[i];

            float dist = Vector3.Distance(pos, curr_slot.go.transform.position);

            if (dist < min_dist && dist < max_distance)
            {
                min_dist = dist;
                ret = curr_slot;
            }
        }

        return ret;
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
            GridCreatorSlot curr_slot = grid[i];
            GameObject curr_slot_go = curr_slot.go;

            float half_slot_size = slot_size * 0.5f;
            float quad_x = curr_slot_go.transform.position.x - half_slot_size;
            float quad_y = curr_slot_go.transform.position.z - half_slot_size;
            float quad_w = curr_slot_go.transform.position.x + half_slot_size;
            float quad_z = curr_slot_go.transform.position.z + half_slot_size;

            Vector3 center = new Vector3(curr_slot_go.transform.position.x, 0, curr_slot_go.transform.position.z);

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

            // Draw child conections
            for (int c = 0; c < curr_slot.near_childs.Count; ++c)
            {
                GameObject child_go = curr_slot.near_childs[c];
                Vector3 near_pos_center = new Vector3(child_go.transform.position.x, 0, child_go.transform.position.z);

                Debug.DrawLine(center, near_pos_center, Color.yellow);
            }
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
