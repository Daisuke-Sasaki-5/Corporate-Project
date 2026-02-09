using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class RewardUI : MonoBehaviour
{
    public GameObject uiRoot;
    [Header("UI Text")]
    public List<Button> rewardButtons;
    public List<TMP_Text> rewardTexts;
    public Button nextStageButton;

    private List<RewardData> currentRewards;
    private bool rewardSelected = false;

    [Header("報酬選択演出")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject victoryImage;
    [SerializeField] private GameObject rewardContentRoot;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float waitAfterVictory = 1f;
 
    private void Awake()
    {
        uiRoot.SetActive(false);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        victoryImage.SetActive(false);
        rewardContentRoot.SetActive(false);

        if(nextStageButton != null)
            nextStageButton.gameObject.SetActive(false);
    }

    public void Show(List<RewardData> rewards)
    {
        currentRewards = rewards;
        rewardSelected = false ;

        StopAllCoroutines();
        StartCoroutine(ShowSequence());
    }

    // 勝利後の報酬選択UI演出処理
    private IEnumerator ShowSequence()
    {
        uiRoot.SetActive(true);

        // 初期状態
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        victoryImage.SetActive(false);
        rewardContentRoot.SetActive(false);

        // パネルをフェードイン
        float t = 0f;
        while(t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 勝利Image表示
        victoryImage.SetActive(true);

        // 1秒待つ
        yield return new WaitForSecondsRealtime(waitAfterVictory);

        // 報酬UIを一気に表示
        rewardContentRoot.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        SetupRewardButtons();
    }

    private void SetupRewardButtons()
    {
        nextStageButton.gameObject.SetActive(false);

        for (int i = 0; i < rewardButtons.Count; i++)
        {
            rewardButtons[i].interactable = true;

            if (i < currentRewards.Count)
            {
                rewardButtons[i].gameObject.SetActive(true);

                // 表示名
                rewardTexts[i].text = currentRewards[i].displayName;

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
        StopAllCoroutines();

        victoryImage.SetActive(false);
        rewardContentRoot.SetActive(false);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        uiRoot.SetActive(false);
        nextStageButton.gameObject.SetActive(false);

        GameManager.instance.NextStage();
    }
}
