using System;
using TMPro;
using UnityEngine;

public class UnitStatusPanel : MonoBehaviour
{
    public GameObject panelRoot;

    public TMP_Text swordText;
    public TMP_Text spearText;
    public TMP_Text bowText;

    // 基礎ステータス参考用
    public UnitStats swordBase;
    public UnitStats spearBase;
    public UnitStats bowBase;

    public void Start()
    {
        panelRoot.SetActive(false);
    }

    public void Toggle()
    {
        bool next = !panelRoot.activeSelf;
        panelRoot.SetActive(next);
        if (next)
            Refresh();
    }

    // 明示的に表示
    public void Show()
    {
        panelRoot.SetActive(true);
        Refresh();
    }

    // 明示的に非表示
    public void Hide()
    {
        panelRoot.SetActive(false);
    }

    public void Refresh()
    {
        UpdateUnit(UnitStats.UnityType.Sword, swordBase, swordText);

        UpdateUnit(UnitStats.UnityType.Spear, spearBase, spearText);

        UpdateUnit(UnitStats.UnityType.Bow, bowBase, bowText);
    }

    private string GetUnitDisplayName(UnitStats.UnityType type)
    {
        switch(type)
        {
            case UnitStats.UnityType.Sword:
                return "Sword";
            case UnitStats.UnityType.Spear:
                return "Spear";
            case UnitStats.UnityType.Bow:
                return "Bow";
            default:
                return type.ToString();
        }
    }

    private void UpdateUnit(UnitStats.UnityType type, UnitStats baseStats,TMP_Text text)
    {
        var atk = UnitManager.instance.GetAttackWithBonus(type,baseStats);
        var hp = UnitManager.instance.GetHPWithBonus(type,baseStats);

        string unitName = GetUnitDisplayName(type);

        string atkText = atk.bonus > 0 ? $"ATK{atk.finalValue} (+{atk.bonus})" : $"ATK{atk.finalValue}";
        string hpText = hp.bonus > 0 ? $"HP{hp.finalValue} (+{hp.bonus})" : $"HP{hp.finalValue}";

        text.text = $"{unitName}\n{atkText}\n{hpText}";
    }
}
