using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem
{
    public enum EventType
    {
        ENEMY_ADDED,
        ENEMY_TO_DELETE,
        ENEMY_KILLED,
        ENEMY_ARRIVES,

        TRACKER_FOUND,
        TRACKER_LOST,
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

        // Enemies
        public class EnemyAdded
        {
            public EnemyAdded() { }

            public GameObject go = null;
        }   public EnemyAdded enemy_added = new EnemyAdded();

        public class EnemyToDelete
        {
            public EnemyToDelete() { }

            public GameObject go = null;
        }   public EnemyToDelete enemy_to_delete = new EnemyToDelete();

        public class EnemyKilled
        {
            public EnemyKilled() { }

            public GameObject killer = null;
            public GameObject killed = null;
        }   public EnemyKilled enemy_killed = new EnemyKilled();

        public class EnemyArrives
        {
            public EnemyArrives() { }

            public GameObject game_object = null;
        }   public EnemyArrives enemy_arrives = new EnemyArrives();

        // Trackers
        public class TrackerFound
        {
            public TrackerFound() { }

            public GameObject tracker_go = null;
        }   public TrackerFound tracker_found = new TrackerFound();

        public class TrackerLost
        {
            public TrackerLost() { }

            public GameObject tracker_go = null;
        }   public TrackerLost tracker_lost = new TrackerLost();

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
