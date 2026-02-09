using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyPlaceTile> enemyTiles;
    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] private int spawnCount; // 何体湧かせるか

    [Header("Enemy Buff")]
    public int hpBonus = 0;
    public int attackBonus = 0;

    private void Awake()
    {
        enemyTiles = new List<EnemyPlaceTile>(GetComponentsInChildren<EnemyPlaceTile>());
    }

    public void SpawnEnemies()
    {
        // 空いているタイルだけを集める
        List<EnemyPlaceTile> freeTiles = enemyTiles.FindAll(t => t.isOccupied == false);
        if (freeTiles.Count == 0) return;

        for (int i = 0; i < spawnCount; i++)
        {
            int index = Random.Range(0, freeTiles.Count);

            // タイルをランダムに選択
            EnemyPlaceTile tile = freeTiles[index];
            freeTiles.RemoveAt(index); // 二度同じタイルを使わせない

            // 敵プレハブをランダムに選択
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            tile.EnemyPlaceUnit(prefab);
        }
    }

    // ステージクリアごとに１回だけ呼ぶ
    public void IncreaseDifficluty()
    {
        int roll = Random.Range(0, 2);

        if(roll == 0) attackBonus += 10;
        else hpBonus += 10;

        if(GameManager.instance.stage % 4 == 0) // 4ステージごと
            spawnCount++;
    }
}
