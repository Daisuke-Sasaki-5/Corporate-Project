using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyPlaceTile> enemyTiles;
    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] private int spawnCount = 5; // 何体湧かせるか

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
}
