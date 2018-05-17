using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject world_parent;

    [Header("UI")]
    [SerializeField] private Text wave_text;
    [SerializeField] private Text money_text;
    [SerializeField] private GameObject map_not_found_panel;

    [Header("Grid Sprites")]
    [SerializeField] private Sprite grid_base_sprite;
    [SerializeField] private Sprite grid_pressed_sprite;

    [Header("Enemies")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private float max_time_spawn = 5;
    [SerializeField] private float min_time_spawn = 1;

    [Header("Turrets")]
    [SerializeField] private GameObject blue_turret;
    [SerializeField] private GameObject red_turret;
    [SerializeField] private GameObject green_turret;

    [Header("Map")]
    [SerializeField] private GameObject map;

    [Header("Trackers")]
    [SerializeField] private GameObject map_tracker;
    [SerializeField] private GameObject red_turret_tracker;
    [SerializeField] private GameObject blue_turret_tracker;
    [SerializeField] private GameObject green_turret_tracker;

    private GridManager grid_manager = new GridManager();
    private PathManager path_manager = new PathManager();
    private EventSystem event_system = new EventSystem();

    GridManager.Grid curr_grid = null;

    private List<GameObject> spawn_points = new List<GameObject>();

    private int curr_wave = 0;
    List<GameObject> enemies = new List<GameObject>();

    private int enemies_to_spawn = 0;
    Timer enemies_to_spawn_timer = new Timer();
    private float to_spawn_random_time = 0.0f;

    private int money = 0;

    private bool map_found = false;
    private bool placing_turret = false;

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

            if (mi != null)
            {
                spawn_points = mi.GetSpawners();

                GameObject enemy_killer = mi.GetEnemyKiller();

                if(enemy_killer != null)
                {
                    EnemyKiller e_killer_script = enemy_killer.GetComponent<EnemyKiller>();

                    if(e_killer_script != null)
                    {
                        e_killer_script.SetEventSystem(event_system);
                    }
                }
            }
            
        }

        // Trackers
        if(map_tracker != null)
        {
            MarkerDetectionScript md = map_tracker.GetComponent<MarkerDetectionScript>();

            if (md != null)
                md.SetEventSystem(event_system);
        }

        if (red_turret_tracker != null)
        {
            MarkerDetectionScript md = red_turret_tracker.GetComponent<MarkerDetectionScript>();

            if (md != null)
                md.SetEventSystem(event_system);
        }

        if (blue_turret_tracker != null)
        {
            MarkerDetectionScript md = blue_turret_tracker.GetComponent<MarkerDetectionScript>();

            if (md != null)
                md.SetEventSystem(event_system);
        }

        if (green_turret_tracker != null)
        {
            MarkerDetectionScript md = green_turret_tracker.GetComponent<MarkerDetectionScript>();

            if (md != null)
                md.SetEventSystem(event_system);
        }

        // UI
        UpdateMoneyUI(money);
        UpdateWaveUI(curr_wave);
    }

    public void Update ()
    {
        CheckNextWave();
        SpawnEnemies();

        CheckGridPlacement();
    }

    private void WinMoney(int add)
    {
        if(add > 0)
        {
            money += add;

            UpdateMoneyUI(money);
        }
    }

    private void SpendMoney(int spend)
    {
        if(spend > 0)
        {
            money -= spend;

            if (money < 0)
                money = 0;

            UpdateMoneyUI(money);
        }
    }

    private bool CanBuy(int price)
    {
        return price <= money;
    }

    private void CheckGridPlacement()
    {
        if (curr_grid.IsSlotSelected())
        {
            StartPlacingTurret();
        }
    }

    private void StartPlacingTurret()
    {
        if(!placing_turret)
        {
            Time.timeScale = 0;

            placing_turret = true;
        }
    }

    private void FinishPlacingTurret()
    {
        if(placing_turret && map_found)
        {
            Time.timeScale = 1;

            placing_turret = false;

            curr_grid.DeselectSelectedSlot();
        }
    }

    private bool CheckNextWave()
    {
        bool ret = false;

        if (enemies_to_spawn <= 0)
        {
            if (enemies.Count <= 0)
            {
                NextWave();
            }
        }

        return ret;
    }

    private void NextWave()
    {
        ++curr_wave;

        UpdateWaveUI(curr_wave);

        enemies_to_spawn = curr_wave;

        enemies_to_spawn_timer.Start();

        to_spawn_random_time = Random.Range(min_time_spawn, max_time_spawn + 1);
    }

    private void SpawnEnemies()
    {
        if(enemies_to_spawn > 0)
        {
            if(enemies_to_spawn_timer.ReadTime() > to_spawn_random_time)
            {
                SpawnEnemy();

                --enemies_to_spawn;

                enemies_to_spawn_timer.Start();

                to_spawn_random_time = Random.Range(min_time_spawn, max_time_spawn + 1);
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

    public void SpawnTurret(Vector3 pos, TurretShoot.TurretType type)
    {
        if(world_parent != null)
        {
            GameObject curr_en = null;

            switch (type)
            {
                case TurretShoot.TurretType.BLUE:
                    if(blue_turret != null)
                        curr_en = Instantiate(blue_turret, pos, world_parent.transform.rotation);
                    break;
                case TurretShoot.TurretType.RED:
                    if (red_turret != null)
                        curr_en = Instantiate(red_turret, pos, world_parent.transform.rotation);
                    break;
                case TurretShoot.TurretType.GREEN:
                    if (green_turret != null)
                        curr_en = Instantiate(green_turret, pos, world_parent.transform.rotation);
                    break;
            }

            if (curr_en != null)
            {
                curr_en.transform.parent = world_parent.transform;

                TurretShoot turr_shoot = curr_en.GetComponent<TurretShoot>();

                if (turr_shoot != null)
                    turr_shoot.SetManagers(this, event_system);
            }
        }
    }

    public void OnEvent(EventSystem.Event ev)
    {
        if(ev.GetEventType() == EventSystem.EventType.ENEMY_TO_DELETE)
        {
            enemies.Remove(ev.enemy_to_delete.go);
        }

        if(ev.GetEventType() == EventSystem.EventType.ENEMY_KILLED)
        {
            WinMoney(10);
        }

        if (ev.GetEventType() == EventSystem.EventType.ENEMY_ARRIVES)
        {

        }

        if(ev.GetEventType() == EventSystem.EventType.TRACKER_FOUND)
        {
            if (ev.tracker_found.tracker_go == map_tracker)
            {
                SetMapFound(true);
            }

            if(curr_grid.IsSlotSelected())
            { }
        }

        if (ev.GetEventType() == EventSystem.EventType.TRACKER_LOST)
        {
            if (ev.tracker_lost.tracker_go == map_tracker)
            {
                SetMapFound(false);
            }
        }

        if(placing_turret)
        {
            Vector3 slot_pos = curr_grid.GetSelectedSlot().GetGameObject().transform.position;

            if (ev.GetEventType() == EventSystem.EventType.TRACKER_FOUND)
            {
                if (ev.tracker_found.tracker_go == blue_turret_tracker)
                {
                    SpawnTurret(slot_pos, TurretShoot.TurretType.BLUE);

                    FinishPlacingTurret();
                }
            }

            if (ev.GetEventType() == EventSystem.EventType.TRACKER_FOUND)
            {
                if (ev.tracker_found.tracker_go == red_turret_tracker)
                {
                    SpawnTurret(slot_pos, TurretShoot.TurretType.RED);

                    FinishPlacingTurret();
                }
            }

            if (ev.GetEventType() == EventSystem.EventType.TRACKER_FOUND)
            {
                if (ev.tracker_found.tracker_go == green_turret_tracker)
                {
                    SpawnTurret(slot_pos, TurretShoot.TurretType.GREEN);

                    FinishPlacingTurret();
                }
            }
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

    private void SetMapFound(bool set)
    {
        if(set)
        {
            if(map_not_found_panel != null)
                map_not_found_panel.SetActive(false);
            
            if(!placing_turret)
                Time.timeScale = 1;

            map_found = true;
        }
        else
        {
            if (map_not_found_panel != null)
                map_not_found_panel.SetActive(true);

            Time.timeScale = 0;

            map_found = false;
        }
    }

    public EventSystem GetEventSystem()
    {
        return event_system;
    }
}
