using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private GameObject shoot_point;

    [SerializeField] private GameObject x_rotation_point;
    [SerializeField] private GameObject y_rotation_point;

    [SerializeField] private float range = 50.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float rotation_speed = 50.0f;
    [SerializeField] private float time_between_shoots = 2.0f;

    private LevelManager level_manager = null;
    private EventSystem event_system = null;

    private GameObject target = null;

    private Timer time_between_shoots_timer = new Timer();

    public void SetManagers(LevelManager level_man, EventSystem ev_sys)
    {
        level_manager = level_man;
        event_system = ev_sys;

        event_system.Suscribe(OnEvent);
    }

    public void OnEvent(EventSystem.Event ev)
    {
        if (ev.GetEventType() == EventSystem.EventType.ENEMY_KILLED)
        {
            if (ev.enemy_killed.killed == target)
                target = null;
        }
    }

    private void LookForTarget()
    {
        if (level_manager != null)
        {
            List<GameObject> enemies = level_manager.GetEnemies();

            GameObject closer_go = null;
            float closer = float.PositiveInfinity;
            for(int i = 0; i < enemies.Count; ++i)
            {
                float distance = Vector3.Distance(enemies[i].transform.position, gameObject.transform.position);

                if(distance < closer && distance < range)
                {
                    closer = distance;
                    closer_go = enemies[i];
                }
            }

            target = closer_go;
        }
    }

    private void AimTarget()
    {
        if(target != null)
        {
            if (y_rotation_point != null)
            {
                Vector3 target_pos = target.transform.position;

                Vector3 last_rot = y_rotation_point.transform.localRotation.eulerAngles;
                y_rotation_point.transform.LookAt(target_pos);
                Vector3 curr_rot = y_rotation_point.transform.localRotation.eulerAngles;

                Vector3 final_rotation = new Vector3(last_rot.x, curr_rot.y, last_rot.z);

                y_rotation_point.transform.localRotation = Quaternion.Euler(final_rotation);
            }

            if (x_rotation_point != null)
            {
                Vector3 target_pos = target.transform.position;

                Vector3 last_rot = x_rotation_point.transform.localRotation.eulerAngles;
                x_rotation_point.transform.LookAt(target_pos);
                Vector3 curr_rot = x_rotation_point.transform.localRotation.eulerAngles;

                Vector3 final_rotation = new Vector3(curr_rot.x, last_rot.y, last_rot.z);

                x_rotation_point.transform.localRotation = Quaternion.Euler(final_rotation);
            }
        }
    }

    private void Shoot()
    {
        if(time_between_shoots_timer.ReadTime() > time_between_shoots)
        {
            time_between_shoots_timer.Start();

            if(bullet != null && shoot_point != null && x_rotation_point != null)
            {
                Quaternion rot = Quaternion.Euler(x_rotation_point.transform.forward);
                GameObject bullet_ins = Instantiate(bullet, shoot_point.transform.position, rot);
                Bullet bullet_script = bullet_ins.GetComponent<Bullet>();

                if(bullet_script != null)
                    bullet_script.SetInfo(speed, damage, x_rotation_point.transform.forward, gameObject, x_rotation_point.transform.rotation);
                
            }
        }
    }

    private void CheckOutOfRange()
    {
        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > range)
                target = null;
        }
    }

    public void Start()
    {
        time_between_shoots_timer.Start();
    }

    public void Update()
    {
        if(target == null)
        {
            LookForTarget();
        }
        else
        {
            CheckOutOfRange();
            AimTarget();
            Shoot();
        }
    }
}
