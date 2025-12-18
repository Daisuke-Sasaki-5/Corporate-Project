using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    // 初期化
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (UnitStats.UnityType type in System.Enum.GetValues(typeof(UnitStats.UnityType)))
        {
            attackBounusPerType[type] = 0;
            hpBounusPerType[type] = 0;
        }
    }

    // バフ保存用データ
    
    // 兵種ごとの累積バフ
    private Dictionary<UnitStats.UnityType,int>attackBounusPerType = new Dictionary<UnitStats.UnityType,int>();
    private Dictionary<UnitStats.UnityType, int> hpBounusPerType = new Dictionary<UnitStats.UnityType, int>();

    // 全兵士に適用されるバフ
    private int globalAttackBonus = 0;
    private int globalHpBonus = 0;

    // 報酬適用
    public void ApplyReward(RewardData reward)
    {
        switch(reward.rewardType)
        {
            case RewardType.AddTotalUnit:
                GameManager.instance.totalUnitLimit += reward.value;
                break;
            case RewardType.AddUnitLimit:
                GameManager.instance.unitLimit[reward.unityType] += reward.value;
                break;
            case RewardType.AllAttackUp:
                globalAttackBonus += reward.value;
                break;
            case RewardType.AllHPUp:
                globalHpBonus += reward.value;
                break;
            case RewardType.UnitAttackUp:
                attackBounusPerType[reward.unityType] += reward.value;
                break;
                case RewardType.UnitHPUp:
                hpBounusPerType[reward.unityType] += reward.value;
                break;
        }
        Debug.Log("報酬適用完了");
    }

    // 新ユニットにバフを適用　プレハブから生成した直後呼ぶ
    public void ApplyStatsToUnit(UnitStats stats)
    {
        // 基礎値から再計算
        stats.attackPower = stats.baseattackPower + globalAttackBonus + attackBounusPerType[stats.unityType];

        stats.MaxHP = stats.baseMaxhp + globalHpBonus + hpBounusPerType[stats.unityType];

        stats.currentHP = stats.MaxHP;

        if(stats.hpslider != null)
        {
            stats.hpslider.maxValue = stats.MaxHP;
            stats.hpslider.value = stats.currentHP;
        }
    }

    // 最終的なステータス数値(攻撃力)
    public (int finalValue, int bonus) GetAttackWithBonus(UnitStats.UnityType type, UnitStats baseStats)
    {
        int bonus = globalAttackBonus + attackBounusPerType[type];

        int finalValue = baseStats.baseattackPower + bonus;

        return (finalValue, bonus);
    }

    // 最終的なステータス数値(HP)
    public (int finalValue, int bonus) GetHPWithBonus(UnitStats.UnityType type, UnitStats baseStats)
    {
        int bonus = globalHpBonus + hpBounusPerType[type];

        int finalValue = baseStats.baseMaxhp + bonus;

        return (finalValue, bonus);
    }
}
