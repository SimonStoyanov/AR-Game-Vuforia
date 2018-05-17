using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{
    private EventSystem event_system = null;

    public void SetEventSystem(EventSystem es)
    {
        event_system = es;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(event_system != null)
        {
            if (other.gameObject.GetComponent<Stats>() != null)
            {
                EventSystem.Event ev = new EventSystem.Event(EventSystem.EventType.ENEMY_ARRIVES);
                ev.enemy_arrives.game_object = other.gameObject;
                event_system.SendEvent(ev);

                EventSystem.Event ev_to_del = new EventSystem.Event(EventSystem.EventType.ENEMY_TO_DELETE);
                ev_to_del.enemy_to_delete.go = other.gameObject;
                event_system.SendEvent(ev_to_del);

                Destroy(other.gameObject);
            }
        }
    }
}
