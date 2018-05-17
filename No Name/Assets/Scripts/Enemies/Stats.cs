using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private float life = 100;
    private float curr_life = 0;

    private LevelManager level_manager = null;
    private EventSystem event_system = null;

    public void SetManagers(LevelManager level_man, EventSystem ev_sys)
    {
        level_manager = level_man;

        event_system = ev_sys;
        event_system.Suscribe(OnEvent);
    }

    private void Awake()
    {
        curr_life = life;
    }

    public void OnHit(GameObject dealer, int dmg)
    {
        DealDamage(dealer, dmg);
    }
	
    public void DealDamage(GameObject dealer, int dmg)
    {
        if(dmg > 0)
        {
            curr_life -= dmg;
        }

        if (curr_life < 0)
            curr_life = 0;

        CheckDeath(dealer);
    }

    public bool IsDead()
    {
        return curr_life <= 0;
    }

    private void CheckDeath(GameObject dealer)
    {
        if(IsDead())
        {
            // Send Event
            EventSystem.Event to_delete_event = new EventSystem.Event(EventSystem.EventType.ENEMY_TO_DELETE);
            to_delete_event.enemy_to_delete.go = this.gameObject;
            event_system.SendEvent(to_delete_event);

            EventSystem.Event death_event = new EventSystem.Event(EventSystem.EventType.ENEMY_KILLED);
            death_event.enemy_killed.killed = this.gameObject;
            death_event.enemy_killed.killer = dealer;
            event_system.SendEvent(death_event);

            Destroy(gameObject);
        }
    }

    public void OnEvent(EventSystem.Event ev)
    {
    
    }

    public void OnDestroy()
    {
        if(event_system != null)
            event_system.UnSuscribe(OnEvent);
    }
}
