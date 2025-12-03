using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlaceTile : MonoBehaviour
{
    public bool isOccupied = false;

    public GameObject hpBarPrefab;

    public bool EnemyPlaceUnit(GameObject enemyPrefab)
    {
        if (isOccupied) return false;

        Vector3 spwanPos = transform.position;
        spwanPos.y = 0f;

        GameObject unit = Instantiate(enemyPrefab, spwanPos, Quaternion.LookRotation(Vector3.back));

        CreateHPBar(unit);

        // UnitStatsÇéÊìæ
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

    private void CreateHPBar(GameObject placedUnit)
    {
        // HPÉoÅ[ê∂ê¨
        GameObject hpobj = Instantiate(hpBarPrefab, placedUnit.transform);

        hpobj.transform.localPosition = new Vector3(0, 0.5f, 0); // ì™ÇÃè„Ç…ê›íË

        Slider slider = hpobj.GetComponentInChildren<Slider>();
        UnitStats stats = placedUnit.GetComponent<UnitStats>();

        stats.hpslider = slider;
        slider.maxValue = stats.Maxhp;
        slider.value = stats.currentHP;
    }
}
