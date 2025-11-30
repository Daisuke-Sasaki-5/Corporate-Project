using System;
using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    public bool isOccupied = false;

    public bool PlaceUnit(GameObject unitPrefab)
    {
        if (isOccupied) return false;

        Instantiate(unitPrefab, transform.position, Quaternion.identity);
        isOccupied = true;
        return true;
    }
}
