using System;
using UnityEngine;

public class EnemyPlaceTile : MonoBehaviour
{
    public bool isOccupied = false;

    public bool EnemyPlaceUnit(GameObject enemyPrefab)
    {
        if (isOccupied) return false;

        Vector3 spwanPos = transform.position;
        spwanPos.y = 0f;

        GameObject unit = Instantiate(enemyPrefab, spwanPos, Quaternion.LookRotation(Vector3.back));

        // UnitStats‚ðŽæ“¾
        UnitStats stats = unit.GetComponent<UnitStats>();
        if (stats != null)
        {
            ApplyColor(stats.unityType);
        }

        isOccupied = true;
        return true;
    }

    private void ApplyColor(UnitStats.UnityType unityType)
    {
        Renderer rend = GetComponent<Renderer>();
        switch (unityType)
        {
            case UnitStats.UnityType.Sword:
                rend.material.color = Color.red;
                break;
            case UnitStats.UnityType.Spear:
                rend.material.color = Color.blue;
                break;
            case UnitStats.UnityType.Bow:
                rend.material.color = Color.green;
                break;
        }
    }
}
