using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class RewardUI : MonoBehaviour
{
    public GameObject uiRoot;
    public List<Button> rewardButtons;
    public List<TMP_Text> rewardTexts;

    private List<RewardData> currentRewards;

    public void Show(List<RewardData> rewards)
    {
        currentRewards = rewards;

        uiRoot.SetActive(true);

        for (int i = 0; i < rewardButtons.Count; i++)
        {
            if (i < rewards.Count)
            {
                rewardButtons[i].gameObject.SetActive(true);

                // 表示名
                rewardTexts[i].text = rewards[i].displayName;

                int index = i;
                rewardButtons[i].onClick.RemoveAllListeners();
                rewardButtons[i].onClick.AddListener(() => OnSelect(index));
            }
            else
            {
                rewardButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnSelect(int index)
    {
        uiRoot.SetActive(false);
        RewardManager.instance.ApplyReward(currentRewards[index]);

        // 次のステージへ移行(後で追加)
    }
}
