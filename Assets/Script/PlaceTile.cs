using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTile : MonoBehaviour
{
    public GameObject placedUnit; // 乗っているユニット
    public bool isOccupied = false;

    private Renderer rende;
    private Color defaultcolor; // デフォルトの色を記録

    public GameObject hpBarPrefab;

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

        CreateHPBar(placedUnit);

        // UnitStatsを取得
        UnitStats stats = placedUnit.GetComponent<UnitStats>();
        if(stats != null)
        {
            // 報酬による強化値を適用する
           UnitManager.instance.ApplyStatsToUnit(stats); 

            ApplyColor(stats.unityType);
        }

        isOccupied = true;
        return true;
    }

    private void CreateHPBar(GameObject placedUnit)
    {
        // HPバー生成
        GameObject hpobj = Instantiate(hpBarPrefab,placedUnit.transform);

        hpobj.transform.localPosition = new Vector3(0, 0.5f, 0); // 頭の上に設定

        Slider slider = hpobj.GetComponentInChildren<Slider>();
        UnitStats stats = placedUnit.GetComponent<UnitStats>();

        stats.hpslider = slider;
        slider.maxValue = stats.Maxhp;
        slider.value = stats.currentHP;
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
