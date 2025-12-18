using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("表示UI")]
    [SerializeField] private GameObject[] showObjects; // 順番に出すオブジェクト
    [SerializeField] private float interval = 0.5f;    // 表示間隔

    [SerializeField] private AudioClip readyBGM;

    void Start()
    {
        AudioManager.instance.PlayBGM(readyBGM);

        // カーソルを表示
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"[Start] Time.timeScale = {Time.timeScale}");
        // 最初は全部表示
        foreach (var obj in showObjects)obj.SetActive(false);

        // 順番表示
        StartCoroutine(ShowUI());
    }

    private IEnumerator ShowUI()
    {
        Time.timeScale = 1f;
        foreach (var obj in showObjects)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
    }

    // ==== ボタン処理 ====

    /// <summary>
    /// リトライボタン押下でゲームのリトライ
    /// </summary>
    public void OnRetryButton()
    {

        Time.timeScale = 1;

        // ゲームシーンをロード
        SceneManager.LoadScene("GamePlayScene");
    }

    /// <summary>
    /// タイトルボタン押下でタイトルシーンへ
    /// </summary>
    public void OnTitleButton()
    {

        Time.timeScale = 1;

        // タイトルシーンをロード
        SceneManager.LoadScene("Title");
    }
}
