using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject escUI;
    public static GameManager instance;

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
        if(escUI != null)
        {
            escUI.SetActive(false);
        }
    }

    void Update()
    {
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

        ShowUI();
    }

    /// <summary>
    /// 敗北処理
    /// </summary>
    private void GameLose()
    {
        isGameOver = true;
        Debug.Log("敗北");

        ShowUI();
    }

    private void ShowUI()
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
