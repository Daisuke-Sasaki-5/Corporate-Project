using System.Collections.Generic;
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
            // タイルをランダムに選択
            EnemyPlaceTile tile = enemyTiles[Random.Range(0, freeTiles.Count)];

            // 敵プレハブをランダムに選択
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            tile.EnemyPlaceUnit(prefab);
        }
    }

    // ステージクリアごとに１回だけ呼ぶ
    public void IncreaseDifficluty()
    {
        int roll = Random.Range(0, 3);

        switch (roll)
        {
            case 0:
                attackBonus += 5;
                Debug.Log("攻撃力 増加");
                break;
            case 1:
                hpBonus += 10;
                Debug.Log("HP 増加");
                break;
            case 2:
                spawnCount++;
                Debug.Log("スポーン数 増加");
                break;
        }
    }
}
