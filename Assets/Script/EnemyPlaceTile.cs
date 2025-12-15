using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlaceTile : MonoBehaviour
{
    public bool isOccupied = false;

    public GameObject hpBarPrefab;

    private Renderer rend;
    private Color defaultColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }

    public bool EnemyPlaceUnit(GameObject enemyPrefab)
    {
        if (isOccupied) return false;

        Vector3 spwanPos = transform.position;
        spwanPos.y = 0f;

        GameObject unit = Instantiate(enemyPrefab, spwanPos, Quaternion.LookRotation(Vector3.back));

        CreateHPBar(unit);

        // UnitStatsを取得
        UnitStats stats = unit.GetComponent<UnitStats>();
        if (stats != null)
        {
            ApplyColor(stats.unityType);
            ApplyEnemyBuff(stats);
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
        // HPバー生成
        GameObject hpobj = Instantiate(hpBarPrefab, placedUnit.transform);

        hpobj.transform.localPosition = new Vector3(0, 0.5f, 0); // 頭の上に設定

        Slider slider = hpobj.GetComponentInChildren<Slider>();
        UnitStats stats = placedUnit.GetComponent<UnitStats>();

        stats.hpslider = slider;
        slider.maxValue = stats.MaxHP;
        slider.value = stats.currentHP;
    }

    // ステージクリア毎の再生成時ステータス強化
    public void ApplyEnemyBuff(UnitStats stats)
    {
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();

        // ステータス増加
        stats.MaxHP += spawner.hpBonus;
        stats.attackPower +=spawner.attackBonus;

        // HPバー更新
        stats.currentHP = stats.MaxHP;
        if(stats.hpslider != null)
        {
            stats.hpslider.maxValue = stats.MaxHP;
            stats.hpslider.value = stats.currentHP;
        }
    }

    // タイルの初期化(ステージリセット用)
    public void ResetTile()
    {
        isOccupied = false;
        rend.material.color = defaultColor;
    }
}
