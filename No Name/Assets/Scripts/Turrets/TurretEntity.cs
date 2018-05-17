using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEntity : MonoBehaviour
{
    [SerializeField] private int price = 100;

    public int GetPrice()
    {
        return price;
    }
}
