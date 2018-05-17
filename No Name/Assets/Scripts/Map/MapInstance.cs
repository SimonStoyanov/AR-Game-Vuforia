using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInstance : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();

    [SerializeField] private GameObject enemy_killer = null;

    public List<GameObject> GetSpawners()
    {
        return spawners;
    }

    public GameObject GetEnemyKiller()
    {
        return enemy_killer;
    }
}
