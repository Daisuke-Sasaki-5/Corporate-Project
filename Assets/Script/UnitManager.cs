using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    private void Awake()
    {
        instance = this;
    }

    // バフ保存用データ
    
    // 兵種ごとの累積バフ
    private Dictionary<UnitStats.UnityType,int>attackBounusPerType = new Dictionary<UnitStats.UnityType,int>();
    private Dictionary<UnitStats.UnityType, int> hpBounusPerType = new Dictionary<UnitStats.UnityType, int>();

    // 全兵士に適用されるバフ
    private int globalAttackBonus = 0;
    private int globalHpBonus = 0;

    // 最大配置数
    public int maxUnitCount = 3;

    // 初期化
    private void Start()
    {
        foreach(UnitStats.UnityType type in System.Enum.GetValues(typeof(UnitStats.UnityType)))
        {
            attackBounusPerType[type] = 0;
            hpBounusPerType[type] = 0;
        }
    }

    // 報酬適用
    public void ApplyReward(RewardData reward)
    {
        switch(reward.rewardType)
        {
            case RewardType.AddUnit:
                AddUnitLimit(reward.value); 
                break;
            case RewardType.AddMaxUnit:
                AddMaxUnitLimit(reward.value); 
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

    private void AddUnitLimit(int value)
    {
        maxUnitCount += value;
    }

    private void AddMaxUnitLimit(int value)
    {
       maxUnitCount += value;
    }

    // 新ユニットにバフを適用　プレハブから生成した直後呼ぶ
    public void ApplyStatsToUnit(UnitStats stats)
    {
        // グローバルバフ
        stats.attackPower += globalAttackBonus;
        stats.Maxhp += globalHpBonus;

        // 兵種固有バフ
        stats.attackPower += attackBounusPerType[stats.unityType];
        stats.Maxhp += hpBounusPerType[stats.unityType];

        // HPを更新
        stats.currentHP = stats.Maxhp;
        if(stats.hpslider != null)
        {
            stats.hpslider.maxValue = stats.Maxhp;
            stats.hpslider.value = stats.currentHP;
        }
    }
}
