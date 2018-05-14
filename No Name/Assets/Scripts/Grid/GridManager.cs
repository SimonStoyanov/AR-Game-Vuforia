using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private GridCreator grid_creator = null;

    private List<Grid> grids = new List<Grid>();

    [HideInInspector]
    public class Grid
    {
        public Grid(List<GridCreator.GridCreatorSlot> slots)
        {
            GenerateRuntimeGrid(slots);
        }

        public void SetGridInfo(GameObject _grid_parent, Sprite grid_sprite, Sprite grid_pressed_sprite)
        {
            selected_grid_slot = null;

            grid_parent = _grid_parent;
            grid_slot_sprite = grid_sprite;
            grid_slot_pressed_sprite = grid_pressed_sprite;

            for (int i = 0; i < interactable_slots.Count; ++i)
            {
                interactable_slots[i].GetSpriteRenderer().sprite = grid_sprite;
            }
        }

        private void GenerateRuntimeGrid(List<GridCreator.GridCreatorSlot> grid)
        {
            gird_slots.Clear();
            interactable_slots.Clear();
            non_interactable_slots.Clear();
            path_slots.Clear();

            for (int i = 0; i < grid.Count; ++i)
            {
                GameObject curr_go = grid[i].go;

                SpriteRenderer srend = curr_go.AddComponent<SpriteRenderer>();
                srend.sprite = grid_slot_sprite;
                srend.enabled = false;

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

        public void SetPrintGrid(bool set)
        {
            for (int i = 0; i < interactable_slots.Count; ++i)
            {
                interactable_slots[i].GetSpriteRenderer().enabled = set;
            }
        }

        GridSlot GetGridSlotByGameObject(GameObject go)
        {
            GridSlot ret = null;

            for (int i = 0; i < gird_slots.Count; ++i)
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

            if (slot != null)
            {
                if (selected_grid_slot != null)
                    selected_grid_slot.GetSpriteRenderer().sprite = grid_slot_sprite;

                if (slot.GetSlotType() == GridSlotManager.GridSlotType.GST_INTERACTABLE)
                {
                    selected_grid_slot = slot;

                    selected_grid_slot.GetSpriteRenderer().sprite = grid_slot_pressed_sprite;
                }
            }
        }

        private GameObject grid_parent = null;
        private Vector2 grid_size = Vector2.zero;
        private float slot_size = 0.0f;

        private List<GridSlot> gird_slots = new List<GridSlot>();

        private List<GridSlot> path_slots = new List<GridSlot>();
        private List<GridSlot> interactable_slots = new List<GridSlot>();
        private List<GridSlot> non_interactable_slots = new List<GridSlot>();

        private Sprite grid_slot_sprite = null;
        private Sprite grid_slot_pressed_sprite = null;

        private GridSlot selected_grid_slot = null;

        private GridCreator creator = null;
    }

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

        public void AddNearSlot(GridSlot slot) { near_slots.Add(slot); }

        public bool GetIsUsed() { return is_used; }
        public GameObject GetGameObject() { return game_object; }
        public SpriteRenderer GetSpriteRenderer() { return sprite_renderer; }
        public BoxCollider GetCollider() { return collider; }
        public GridSlotManager.GridSlotType GetSlotType() { return slot_type; }
        List<GridSlot> near_slots = new List<GridSlot>();

        private bool is_used = false;
        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private BoxCollider collider = null;
        private GridSlotManager.GridSlotType slot_type = GridSlotManager.GridSlotType.GST_INTERACTABLE;
    }

    public void InitGrids()
    {
        GridInstance[] grids = Object.FindObjectsOfType<GridInstance>();

        for(int i = 0; i < grids.Length; ++i)
        {
            List<GridCreator.GridCreatorSlot> slots = grids[i].GetGrid();

            CreateGrid(slots);
        }
    }

    public void CreateGrid(List<GridCreator.GridCreatorSlot> creator_slots)
    {
        Grid grid = new Grid(creator_slots);

        grids.Add(grid);
    }

    public List<Grid> GetGrids()
    {
        return grids;
    }

}
