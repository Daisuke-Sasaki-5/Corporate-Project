using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject escUI;
    [SerializeField] private RewardUI rewardUI;

    // ユニット最大数
    public Dictionary<UnitStats.UnityType, int> unitLimit = new Dictionary<UnitStats.UnityType, int>()
    {
        {UnitStats.UnityType.Sword, 3 },
        {UnitStats.UnityType.Spear, 2 },
        {UnitStats.UnityType.Bow, 1 },
    };

    // 現在配置数
    public Dictionary<UnitStats.UnityType, int> placedCount = new Dictionary<UnitStats.UnityType, int>()
    {
        {UnitStats.UnityType.Sword, 0 },
        {UnitStats.UnityType.Spear, 0 },
        {UnitStats.UnityType.Bow, 0 },
    };

    public int totalUnitLimit = 3; // 初期値3体
    public int currentTotalPlaced = 0;

    public bool CanPlaceUnit(UnitStats.UnityType type)
    {
        // 兵種の上限
        if (placedCount[type] >= unitLimit[type]) 
            return false;

        // 全体の上限
        if(currentTotalPlaced >= totalUnitLimit)
            return false;

        return true;
    }

    public void AddPlacedUnit(UnitStats.UnityType type)
    {
        placedCount[type]++;
    }

    public void RemovePlaceUnit(UnitStats.UnityType type)
    {
        placedCount[type]--;

        if (placedCount[type] < 0)
            placedCount[type] = 0;
    }

    public void ResetPlacedUnits()
    {
        placedCount[UnitStats.UnityType.Sword] = 0;
        placedCount[UnitStats.UnityType.Spear] = 0;
        placedCount[UnitStats.UnityType.Bow] = 0;

        currentTotalPlaced = 0;
    }

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

        if(rewardUI != null && rewardUI.uiRoot != null)
        {
            rewardUI.uiRoot.SetActive(false);
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
        ResetPlacedUnits();

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

        // 報酬UIを表示
        RewardManager.instance.ShowRewards();

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
