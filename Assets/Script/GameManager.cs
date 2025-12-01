using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject escUI;

    private bool isGameStart = false;
    private bool isGameOver = false; // 勝敗が決まったかどうか

    private void Awake()
    {
        if(instance == null)
        {
            instance =  this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (startUI != null)
        {
            startUI.SetActive(false);
        }

        if(escUI != null)
        {
            escUI.SetActive(false);
        }

        StartCoroutine(WaitForFadeThenInit());
    }

    private IEnumerator WaitForFadeThenInit()
    {
        if(FadeManager.instance != null)
        {
            while(!FadeManager.instance.IsFadeComplete)
                yield return null;
        }

        InitGame();
    }

    // ==== ゲーム初期化(配置フェーズ) ====
    private void InitGame()
    {
        isGameStart = false;
        isGameOver = false;

        Time.timeScale = 0f;

        if(startUI != null)
            startUI.SetActive(true);

        // 敵を配置
        FindObjectOfType<EnemySpawner>().SpawnEnemies();
    }

    // ==== Startボタンから呼ぶ ====
    public void onClickStartGame()
    {
        StartGame();
    }

    private void StartGame()
    {
        isGameStart = true;
        Time.timeScale = 1f;

        if(startUI != null)
            startUI.SetActive(false);
    }

    void Update()
    {
        if (!isGameStart) return;
        if (isGameOver) return;

        CheckGameState();
    }

    /// <summary>
    /// 味方と敵の残りユニット数を確認して勝敗判定を行う
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void CheckGameState()
    {
        // タグでGameScene内のユニット検索
        GameObject[] enemis = GameObject.FindGameObjectsWithTag("EnemyUnit");
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerUnit");

        // 全滅判定
        if(enemis.Length == 0)
        {
            GameWin();
        }
        else if(players.Length == 0)
        {
            GameLose();
        }
    }

    /// <summary>
    /// 勝利処理
    /// </summary>
    private void GameWin()
    {
       isGameOver = true;
        Debug.Log("勝利");

        ShowEndUI();
    }

    /// <summary>
    /// 敗北処理
    /// </summary>
    private void GameLose()
    {
        isGameOver = true;
        Debug.Log("敗北");

        ShowEndUI();
    }

    private void ShowEndUI()
    {
        if (escUI != null)
        {
            escUI.SetActive(true); 
        }
    }

    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
