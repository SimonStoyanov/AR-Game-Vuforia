﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject world_parent;

    [Header("UI")]
    [SerializeField] private Text wave_text;
    [SerializeField] private Text money_text;

    [Header("Grid Sprites")]
    [SerializeField] private Sprite grid_base_sprite;
    [SerializeField] private Sprite grid_pressed_sprite;

    [Header("Enemies")]
    [SerializeField] private GameObject enemy;

    [Header("Turrets")]
    [SerializeField] private GameObject turret;

    [Header("Map")]
    [SerializeField] private GameObject map;

    private GridManager grid_manager = new GridManager();
    private PathManager path_manager = new PathManager();
    private EventSystem event_system = new EventSystem();

    GridManager.Grid curr_grid = null;

    private List<GameObject> spawn_points = new List<GameObject>();

    private int curr_wave = 0;
    List<GameObject> enemies = new List<GameObject>();

    private int money = 0;

    public void Start()
    {
        event_system.Suscribe(OnEvent);

        // Starting grid and paths (don't use them before that)
        grid_manager.InitGrids();
        path_manager.InitPaths();

        // Setup grid
        curr_grid = grid_manager.GetGridByBridName("grid_1");

        if (curr_grid != null)
        {
            curr_grid.SetGridInfo(world_parent, grid_base_sprite, grid_pressed_sprite);
            curr_grid.SetPrintGrid(true);
        }
        
        // Get spawn points
        if(map != null)
        {
            MapInstance mi = map.GetComponent<MapInstance>();

            if(mi != null)
                spawn_points = mi.GetSpawners();      
        }

        UpdateMoneyUI(money);
        UpdateWaveUI(curr_wave);

        SpawnEnemy();
    }

    public void Update ()
    {
        CheckGridPlacement();

        if(Input.GetKeyDown("d"))
            SpawnEnemy();
    }

    private void CheckGridPlacement()
    {
        if(curr_grid.IsSlotSelected())
        {
            if(Input.GetKeyDown("a"))
            {
                Vector3 spawn_pos = curr_grid.GetSelectedSlot().GetGameObject().transform.position;

                SpawnTurret(spawn_pos);

                curr_grid.DeselectSelectedSlot();
            }
        }
    }

    public void SpawnEnemy()
    {
        if(spawn_points.Count > 0)
        {
            Vector3 spawn_pos = spawn_points[Random.Range(0, spawn_points.Count)].transform.position;
            PathManager.Path path = path_manager.GetCloserPath(spawn_pos);

            if(path != null)
            {
                if(enemy != null)
                {
                    GameObject curr_en = Instantiate(enemy, spawn_pos, Quaternion.identity);

                    if (world_parent != null)
                        curr_en.transform.parent = world_parent.transform;

                    curr_en.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    enemies.Add(curr_en);

                    FollowPath path_script = curr_en.GetComponent<FollowPath>();

                    if(path_script != null)
                        path_script.SetPath(path.GetPathList());

                    Stats stats = curr_en.GetComponent<Stats>();

                    if (stats != null)
                        stats.SetManagers(this, event_system);
                }
            }
        }
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public void SpawnTurret(Vector3 pos)
    {
        if(turret != null && world_parent != null)
        {
            GameObject curr_en = Instantiate(turret, pos, world_parent.transform.rotation);

            curr_en.transform.parent = world_parent.transform;

            TurretShoot turr_shoot = curr_en.GetComponent<TurretShoot>();

            if (turr_shoot != null)
                turr_shoot.SetManagers(this, event_system);
        }
    }

    public void OnEvent(EventSystem.Event ev)
    {
        if(ev.GetEventType() == EventSystem.EventType.ENEMY_KILLED)
        {
            enemies.Remove(ev.enemy_killed.killed);
        }
    }

    private void UpdateWaveUI(int wave)
    {
        if(wave_text != null)
        {
            wave_text.text = "Wave: " + wave.ToString();
        }
    }

    private void UpdateMoneyUI(int money)
    {
        if(money_text != null)
        {
            money_text.text = "Money: " + money.ToString();
        }
    }
}
