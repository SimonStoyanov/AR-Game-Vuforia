using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem
{
    public enum EventType
    {
        ENEMY_ADDED,
        ENEMY_KILLED,
    }

    public class Event
    {
        public Event(EventType e_type)
        {
            event_type = e_type;
        }

        public EventType GetEventType()
        {
            return event_type;
        }

        public class EnemyAdded
        {
            public EnemyAdded() { }

            public GameObject go = null;
        }
        public EnemyAdded enemy_added = new EnemyAdded();

        public class EnemyKilled
        {
            public EnemyKilled() { }

            public GameObject killer = null;
            public GameObject killed = null;
        }   public EnemyKilled enemy_killed = new EnemyKilled();

        private EventType event_type;
    }

    public delegate void OnEventDel(Event ev);
    public event OnEventDel OnEvent;

    public void SendEvent(Event ev)
    {
        OnEvent(ev);
    }

    public void Suscribe(OnEventDel del)
    {
        OnEvent += del;
    }

    public void UnSuscribe(OnEventDel del)
    {
        OnEvent -= del;
    }
}
