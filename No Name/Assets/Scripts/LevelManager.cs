using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject world_parent;

    [Header("Grid Sprites")]
    [SerializeField] private Sprite grid_base_sprite;
    [SerializeField] private Sprite grid_pressed_sprite;

    [Header("Enemies")]
    [SerializeField] private GameObject enemy;

    [Header("Map")]
    [SerializeField] private GameObject map;

    private GridManager grid_manager = new GridManager();
    private PathManager path_manager = new PathManager();

    private List<GameObject> spawn_points = new List<GameObject>();

    private int curr_wave = 0;
    List<GameObject> enemies = new List<GameObject>();

    public void Start()
    {
        // Starting grid and paths (don't use them before that)
        grid_manager.InitGrids();
        path_manager.InitPaths();

        // Setup grid
        GridManager.Grid curr_grid = grid_manager.GetGridByBridName("grid_1");

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

        SpawnEnemy();
    }

    public void Update ()
    {

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
                    enemies.Add(curr_en);

                    FollowPath path_script = curr_en.GetComponent<FollowPath>();

                    if(path_script != null)
                    {
                        path_script.SetPath(path.GetPathList());
                    }
                }
            }
        }
    }
}
