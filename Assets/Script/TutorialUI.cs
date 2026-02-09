using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root; // パネル全体
    [SerializeField] private Image tutorialImage;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Turial Images")]
    [SerializeField] private Sprite[] tutorialSprites;

    private int currentIndex = 0;

    private void Start()
    {
        root.SetActive(false);
    }

    public void Open()
    {
        currentIndex = 0;
        root.SetActive(true);
        UpdateUI();
    }

    public void Close()
    {
        root.SetActive(false);
    }

    public void OnClickNext()
    {
        if(currentIndex < tutorialSprites.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    public void OnClickBack()
    {
        if(currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        tutorialImage.sprite = tutorialSprites[currentIndex];

        // 戻るボタン:最初は非表示
        backButton.gameObject.SetActive(currentIndex > 0);

        // 次へボタン:最後は非表示
        nextButton.gameObject.SetActive(currentIndex < tutorialSprites.Length - 1);
    }
}
