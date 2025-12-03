using UnityEngine;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    [Header("全報酬リスト(ScriptableObject")]
    public List<RewardData> rewardPool = new List<RewardData>();

    [Header("UIプレハブへの参照")]
    public RewardUI rewardUI;

    private void Awake()
    {
        instance = this;
    }

    // ステージ勝利時に呼ぶ
    public void ShowRewards()
    {
        // ランダムに3つ抽選
        List<RewardData> selected = PickRandomRewards(3);

        rewardUI.Show(selected);
    }

    public List<RewardData> PickRandomRewards(int count)
    {
        List<RewardData> pool = new List<RewardData>(rewardPool);
        List<RewardData> result = new List<RewardData>();

        for(int i = 0; i < count; i++)
        {
            if(pool.Count == 0) break;

            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }

    // プレイヤーが選んだ報酬を適用
    public void ApplyReward(RewardData reward)
    {
        UnitManager.instance.ApplyReward(reward);
    }
}
