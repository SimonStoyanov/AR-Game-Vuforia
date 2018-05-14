﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject world_parent;

    public Vector2 grid_starting_pos;
    public float gird_slot_size = 5.0f;
    public int grid_size_x = 10;
    public int grid_size_y = 10;

    public Sprite grid_base_sprite;
    public Sprite grid_pressed_sprite;

    GridManager grid_manager = new GridManager();

	void Start ()
    {
        grid_manager.InitGrids();
        List<GridManager.Grid> grids = grid_manager.GetGrids();
        for(int i = 0; i < grids.Count; ++i)
        {
            grids[i].SetGridInfo(world_parent, grid_base_sprite, grid_pressed_sprite);
            grids[i].SetPrintGrid(true);
        }

    }

	void Update ()
    {

    }


}
