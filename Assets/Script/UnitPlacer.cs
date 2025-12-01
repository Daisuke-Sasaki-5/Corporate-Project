using System;
using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    public GameObject selectedUnityPrefab;

    private GameObject previewObject; // プレビュー用
    private PlaceTile currentTile; // 現在ホバー中のタイル

    private void Update()
    {
        UpdatePreview(); // 常にプレビュー更新

        if (selectedUnityPrefab == null) return;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceTile tile = hit.collider.GetComponent<PlaceTile>();

                if(tile != null)
                {
                    bool placed = tile.PlaceUnit(selectedUnityPrefab);
                    if (placed)
                    {
                        // 配置成功 → 選択解除
                        selectedUnityPrefab = null;
                    }

                }
            }
        }
    }

    private void UpdatePreview()
    {
        if(selectedUnityPrefab == null)
        {
            DestroyPreview();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            PlaceTile tile = hit.collider.GetComponent<PlaceTile>();

            if(tile != null && !tile.isOccupied)
            {
                // タイルが変わったらプレビュー再生成
                if(tile != currentTile)
                {
                    DestroyPreview();
                    currentTile = tile;

                    Vector3 spwanPos = tile.transform.position;
                    spwanPos.y = 0f;

                    // プレビュー再生成
                    previewObject = Instantiate(selectedUnityPrefab, spwanPos, Quaternion.identity);
                }
                return;
            }
        }

        // タイル上でなければプレビュー削除
        DestroyPreview();
        currentTile = null;
    }
    
    // ==== プレビュー削除 ====
    private void DestroyPreview()
    {
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void SelectUnit(GameObject prefab)
    {
        selectedUnityPrefab = prefab;
    }
}
