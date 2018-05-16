using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridInstance : MonoBehaviour
{
    [SerializeField] private string grid_name;

    [HideInInspector]
    [SerializeField]
    private float slot_size = 0;

    private List<GridCreator.GridCreatorSlot> grid = new List<GridCreator.GridCreatorSlot>();

    public void CreateGrid(float _slot_size)
    {
        slot_size = _slot_size;

        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject go = transform.GetChild(i).gameObject;
            GridSlotManager gs = go.GetComponent<GridSlotManager>();

            GridCreator.GridCreatorSlot slot = new GridCreator.GridCreatorSlot(go, gs);

            grid.Add(slot);
        }

        CalculateGridChilds();
    }

    public List<GridCreator.GridCreatorSlot> GetGrid()
    {
        return grid;
    }

    public string GetGridName()
    {
        return grid_name;
    }

    private void CalculateGridChilds()
    {
        for (int i = 0; i < grid.Count; ++i)
        {
            GridCreator.GridCreatorSlot curr_slot = grid[i];

            curr_slot.near_childs.Clear();

            Vector3 curr_pos = curr_slot.go.transform.position;

            Vector3 up_pos = new Vector3(curr_pos.x, 0, curr_pos.z + slot_size);
            Vector3 down_pos = new Vector3(curr_pos.x, 0, curr_pos.z - slot_size);
            Vector3 left_pos = new Vector3(curr_pos.x - slot_size, 0, curr_pos.z);
            Vector3 right_pos = new Vector3(curr_pos.x + slot_size, 0, curr_pos.z);

            GridCreator.GridCreatorSlot up_slot = GetCloserSlot(up_pos, 0.2f);
            GridCreator.GridCreatorSlot down_slot = GetCloserSlot(down_pos, 0.2f);
            GridCreator.GridCreatorSlot left_slot = GetCloserSlot(left_pos, 0.2f);
            GridCreator.GridCreatorSlot right_slot = GetCloserSlot(right_pos, 0.2f);

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

    private GridCreator.GridCreatorSlot GetCloserSlot(Vector3 pos, float max_distance)
    {
        GridCreator.GridCreatorSlot ret = null;

        float min_dist = float.PositiveInfinity;
        for (int i = 0; i < grid.Count; ++i)
        {
            GridCreator.GridCreatorSlot curr_slot = grid[i];

            float dist = Vector3.Distance(pos, curr_slot.go.transform.position);

            if (dist < min_dist && dist < max_distance)
            {
                min_dist = dist;
                ret = curr_slot;
            }
        }

        return ret;
    }

    void DrawGrid()
    {
        for (int i = 0; i < grid.Count; ++i)
        {
            GridCreator.GridCreatorSlot curr_slot = grid[i];
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

            switch (grid[i].slot_manager.GetSlotType())
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

    private void Awake()
    {
        CreateGrid(slot_size);
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
