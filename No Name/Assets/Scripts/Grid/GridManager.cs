using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private GridCreator grid_creator = null;

    [HideInInspector]
    public class GridSlot
    {
        public GridSlot(GameObject go, GridSlotManager.GridSlotType type, SpriteRenderer srend, BoxCollider coll)
        {
            game_object = go;
            sprite_renderer = srend;
            collider = coll;
            slot_type = type;
        }

        public bool GetIsUsed() { return is_used; }
        public GameObject GetGameObject() { return game_object; }
        public SpriteRenderer GetSpriteRenderer() { return sprite_renderer; }
        public BoxCollider GetCollider() { return collider; }
        public GridSlotManager.GridSlotType GetSlotType() { return slot_type; }

        private bool is_used = false;
        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private BoxCollider collider = null;
        private GridSlotManager.GridSlotType slot_type = GridSlotManager.GridSlotType.GST_INTERACTABLE;
    }

    GameObject grid_parent = null;
    Vector2 grid_size = Vector2.zero;
    float slot_size;

    List<GridSlot> gird_slots = new List<GridSlot>();

    List<GridSlot> path_slots = new List<GridSlot>();
    List<GridSlot> interactable_slots = new List<GridSlot>();
    List<GridSlot> non_interactable_slots = new List<GridSlot>();

    Sprite grid_slot_sprite = null;
    Sprite grid_slot_pressed_sprite = null;

    GridSlot selected_grid_slot = null;

    public void InitGrid(GameObject _grid_parent, Sprite grid_sprite, Sprite grid_pressed_sprite)
    {
        selected_grid_slot = null;

        grid_parent = _grid_parent;
        grid_slot_sprite = grid_sprite;
        grid_slot_pressed_sprite = grid_pressed_sprite;


        GameObject grid_creator_go = GameObject.FindGameObjectWithTag("GridGenerator");

        if (grid_creator_go != null)
        {
            grid_creator = grid_creator_go.GetComponent<GridCreator>();

            if (grid_creator != null)
            {
                List<GridCreator.GridCreatorSlot> grid = grid_creator.GetGrid();

                GenerateRuntimeGrid(grid);
            }
        }
    }

    public void GenerateRuntimeGrid(List<GridCreator.GridCreatorSlot> grid)
    {
        gird_slots.Clear();
        interactable_slots.Clear();
        non_interactable_slots.Clear();
        path_slots.Clear();

        for (int i = 0; i < grid.Count; ++i)
        {
            Debug.Log("hi");

            GameObject curr_go = grid[i].go;

            SpriteRenderer srend = curr_go.AddComponent<SpriteRenderer>();
            srend.sprite = grid_slot_sprite;

            BoxCollider bcoll = curr_go.AddComponent<BoxCollider>();
            bcoll.isTrigger = true;

            GridSlotManager smanager = curr_go.GetComponent<GridSlotManager>();
            smanager.SetGridManager(this);

            GridSlot slot = new GridSlot(curr_go, smanager.GetSlotType(), srend, bcoll);

            gird_slots.Add(slot);

            if (smanager != null)
            {
                switch (smanager.GetSlotType())
                {
                    case GridSlotManager.GridSlotType.GST_INTERACTABLE:
                    {
                        interactable_slots.Add(slot);
                        break;
                    }

                    case GridSlotManager.GridSlotType.GST_NO_INTERACTABLE:
                    {
                        non_interactable_slots.Add(slot);
                        break;
                    }

                    case GridSlotManager.GridSlotType.GST_PATH:
                    {
                        path_slots.Add(slot);
                        break;
                    }
                }
            }
        }
    }

    GridSlot GetGridSlotByGameObject(GameObject go)
    {
        GridSlot ret = null;

        for(int i = 0; i < gird_slots.Count; ++i)
        {
            GridSlot curr_slot = gird_slots[i];

            if (curr_slot.GetGameObject() == go)
            {
                ret = curr_slot;
                break;
            } 
        }

        return ret;
    }

    public void GridOnMouseDownCallback(GameObject go)
    {
        GridSlot slot = GetGridSlotByGameObject(go);

        if(slot != null)
        {
            if(selected_grid_slot != null)
                selected_grid_slot.GetSpriteRenderer().sprite = grid_slot_sprite;

            if (slot.GetSlotType() == GridSlotManager.GridSlotType.GST_INTERACTABLE)
            {
                selected_grid_slot = slot;

                selected_grid_slot.GetSpriteRenderer().sprite = grid_slot_pressed_sprite;
            }
        }
    }

    public void DrawGridDebug()
    {
        //for(int i = 0; i < gird_slots.Count; ++i)
        //{
        //    GridSlot curr_slot = gird_slots[i];

        //    float half_slot_size = slot_size * 0.5f;
        //    float quad_x = curr_slot.GetPosition().x - half_slot_size;
        //    float quad_y = curr_slot.GetPosition().y - half_slot_size;
        //    float quad_w = curr_slot.GetPosition().x + half_slot_size;
        //    float quad_z = curr_slot.GetPosition().y + half_slot_size;

        //    Vector3 center = new Vector3(curr_slot.GetPosition().x, 0, curr_slot.GetPosition().y);

        //    Vector3 line1p1 = new Vector3(quad_x, 0, quad_y);
        //    Vector3 line1p2 = new Vector3(quad_x, 0, quad_z);

        //    Vector3 line2p1 = new Vector3(quad_x, 0, quad_z);
        //    Vector3 line2p2 = new Vector3(quad_w, 0, quad_z);

        //    Vector3 line3p1 = new Vector3(quad_w, 0, quad_z);
        //    Vector3 line3p2 = new Vector3(quad_w, 0, quad_y);

        //    Vector3 line4p1 = new Vector3(quad_w, 0, quad_y);
        //    Vector3 line4p2 = new Vector3(quad_x, 0, quad_y);

        //    Debug.DrawLine(line1p1, center, Color.red);
        //    Debug.DrawLine(line1p1, line1p2);
        //    Debug.DrawLine(line2p1, line2p2);
        //    Debug.DrawLine(line3p1, line3p2);
        //    Debug.DrawLine(line4p1, line4p2);
        //}
    }
}
