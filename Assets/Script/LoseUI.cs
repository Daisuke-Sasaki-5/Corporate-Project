using System;
using System.Collections;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    public GameObject uiRoot;
    [Header("敗北演出")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject loseImage;
    [SerializeField] private GameObject buttonRoot;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float waitAfterLose = 1f;

    private void Awake()
    {
        uiRoot.SetActive(false);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        loseImage.SetActive(false);
        buttonRoot.SetActive(false);
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        uiRoot.SetActive(true);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        loseImage.SetActive(false);
        buttonRoot.SetActive(false);

        // フェードイン
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // 敗北Image
        loseImage.SetActive(true);

        yield return new WaitForSecondsRealtime(waitAfterLose);

        // ボタン表示
        buttonRoot.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Debug.Log($"Buttons count: {buttonRoot.transform.childCount}");
    }
}
