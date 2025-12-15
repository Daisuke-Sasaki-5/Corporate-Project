using System;
using TMPro;
using UnityEngine;

public class UIUnitSelect : MonoBehaviour
{
    public UnitPlacer placer;

    [Header("各ユニットのプレハブ")]
    public GameObject swordPrefab;
    public GameObject spearPrefab;
    public GameObject bowPrefab;

    [Header("上限カウントテキスト")]
    public TMP_Text swordCountText;
    public TMP_Text spearCountText;
    public TMP_Text bowCountText;

    [Header("全体配置数テキスト")]
    public TMP_Text totalUnitText;

    private void Update()
    {
        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        var gm = GameManager.instance;

        swordCountText.text = $"{gm.placedCount[UnitStats.UnityType.Sword]}/{gm.unitLimit[UnitStats.UnityType.Sword]}";
        spearCountText.text = $"{gm.placedCount[UnitStats.UnityType.Spear]}/{gm.unitLimit[UnitStats.UnityType.Spear]}";
        bowCountText.text = $"{gm.placedCount[UnitStats.UnityType.Bow]}/{gm.unitLimit[UnitStats.UnityType.Bow]}";

        totalUnitText.text = $"Total Units {gm.currentTotalPlaced}/{gm.totalUnitLimit}";
    }

    public void PickSword()
    {
        placer.SelectUnit(swordPrefab);
    }

    public void PickSpear()
    {
        placer.SelectUnit(spearPrefab);
    }

    public void PickBow()
    {
       placer.SelectUnit(bowPrefab);
    }
}
