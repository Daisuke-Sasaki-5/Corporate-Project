using System;
using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    public bool isOccupied = false;

    public bool PlaceUnit(GameObject unitPrefab)
    {
        if (isOccupied) return false;

        Vector3 spwanPos = transform.position;
        spwanPos.y = 0f;

        Instantiate(unitPrefab, spwanPos, Quaternion.identity);
        isOccupied = true;

        return true;
    }
}
