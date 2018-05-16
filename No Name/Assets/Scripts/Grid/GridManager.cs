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
        public Grid(string name, List<GridCreator.GridCreatorSlot> slots)
        {
            grid_name = name;
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

        public string GetGridName()
        {
            return grid_name;
        }

        private void GenerateRuntimeGrid(List<GridCreator.GridCreatorSlot> grid)
        {
            grid_slots.Clear();
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

                grid_slots.Add(slot);

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

            for (int i = 0; i < grid_slots.Count; ++i)
            {
                GridSlot curr_slot = grid_slots[i];

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

        public GridSlot GetClosestSlot(Vector3 pos)
        {
            GridSlot ret = null;

            float closest_distance = float.NegativeInfinity;
            for (int i = 0; i < grid_slots.Count; ++i)
            {
                GridSlot curr_slot = grid_slots[i];
                float curr_distance = Vector3.Distance(pos, curr_slot.GetGameObject().transform.position);

                if (curr_distance < closest_distance)
                {
                    ret = curr_slot;
                    closest_distance = curr_distance;
                }
            }

            return ret;
        }

        public GridSlot GetClosestPathSlot(Vector3 pos)
        {
            GridSlot ret = null;

            float closest_distance = float.NegativeInfinity;
            for (int i = 0; i < path_slots.Count; ++i)
            {
                GridSlot curr_slot = path_slots[i];
                float curr_distance = Vector3.Distance(pos, curr_slot.GetGameObject().transform.position);

                if (curr_distance < closest_distance)
                {
                    ret = curr_slot;
                    closest_distance = curr_distance;
                }
            }

            return ret;
        }

        private string grid_name;

        private GameObject grid_parent = null;

        private List<GridSlot> grid_slots = new List<GridSlot>();

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
        public List<GridSlot> near_slots = new List<GridSlot>();

        private bool is_used = false;
        private GameObject game_object = null;
        private SpriteRenderer sprite_renderer = null;
        private BoxCollider collider = null;
        private GridSlotManager.GridSlotType slot_type = GridSlotManager.GridSlotType.GST_INTERACTABLE;
    }

    public void InitGrids()
    {
        grids.Clear();

        GridInstance[] creator_grids = Object.FindObjectsOfType<GridInstance>();

        for(int i = 0; i < creator_grids.Length; ++i)
        {
            List<GridCreator.GridCreatorSlot> slots = creator_grids[i].GetGrid();
            string grid_name = creator_grids[i].GetGridName();

            CreateGrid(grid_name, slots);
        }
    }

    public void CreateGrid(string grid_name, List<GridCreator.GridCreatorSlot> creator_slots)
    {
        Grid grid = new Grid(grid_name, creator_slots);

        grids.Add(grid);
    }

    public List<Grid> GetGrids()
    {
        return grids;
    }

    public Grid GetGridByBridName(string name)
    {
        Grid ret = null;

        for(int i = 0; i < grids.Count; ++i)
        {
            if(grids[i].GetGridName() == name)
            {
                ret = grids[i];
                break;
            }
        }

        return ret;
    }

}
