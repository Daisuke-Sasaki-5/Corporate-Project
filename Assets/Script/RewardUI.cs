using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RewardUI : MonoBehaviour
{
    public GameObject uiRoot;
    [Header("UI Text")]
    public List<Button> rewardButtons;
    public List<TMP_Text> rewardTexts;
    public Button nextStageButton;

    private List<RewardData> currentRewards;
    private bool rewardSelected = false;

    private void Awake()
    {
        if(uiRoot != null)
            uiRoot.SetActive(false);
        if(nextStageButton != null)
            nextStageButton.gameObject.SetActive(false);
    }

    public void Show(List<RewardData> rewards)
    {
        currentRewards = rewards;
        rewardSelected = false ;

        uiRoot.SetActive(true);
        nextStageButton.gameObject.SetActive(false);

        for (int i = 0; i < rewardButtons.Count; i++)
        {
            rewardButtons[i].interactable = true;

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
        if (rewardSelected) return;

        rewardSelected = true;
        RewardManager.instance.ApplyReward(currentRewards[index]);

        // 報酬ボタンを無効化
        foreach (var btn in rewardButtons) btn.interactable = false;

        // 次のステージへ移行
        nextStageButton.gameObject.SetActive(true);
        nextStageButton.onClick.RemoveAllListeners();
        nextStageButton.onClick.AddListener(OnNextStage);
    }

    private void OnNextStage()
    {
        uiRoot.SetActive(false);
        nextStageButton.gameObject.SetActive(false) ;
        GameManager.instance.NextStage();
    }
}
