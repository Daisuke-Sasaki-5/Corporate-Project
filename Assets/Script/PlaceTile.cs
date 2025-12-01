using System;
using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    public GameObject placedUnit; // 乗っているユニット
    public bool isOccupied = false;

    private Renderer rende;
    private Color defaultcolor; // デフォルトの色を記録

    private void Awake()
    {
        rende = GetComponent<Renderer>();
        defaultcolor = rende.material.color;
    }

    public bool PlaceUnit(GameObject unitPrefab)
    {
        if (isOccupied) return false;

        Vector3 spwanPos = transform.position;
        spwanPos.y = 0f;

       placedUnit = Instantiate(unitPrefab, spwanPos, Quaternion.identity);

        // UnitStatsを取得
        UnitStats stats = placedUnit.GetComponent<UnitStats>();
        if(stats != null)
        {
            ApplyColor(stats.unityType);
        }

        isOccupied = true;
        return true;
    }

    public void RemoveUnit()
    {
        if (placedUnit != null)
        {
            Destroy(placedUnit);
            placedUnit = null;
        }
        isOccupied = false;

        // 色を元に戻す
        rende.material.color = defaultcolor;
    }

    private void ApplyColor(UnitStats.UnityType unityType)
    {
        Renderer rend = GetComponent<Renderer>();
        switch(unityType)
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
