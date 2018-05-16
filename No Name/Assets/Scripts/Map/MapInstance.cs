using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInstance : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();

    public List<GameObject> GetSpawners()
    {
        return spawners;
    }
}
