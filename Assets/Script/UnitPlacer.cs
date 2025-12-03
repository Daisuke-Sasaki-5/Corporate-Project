using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitPlacer : MonoBehaviour
{
    public GameObject selectedUnityPrefab;

    private GameObject previewObject; // プレビュー用
    private PlaceTile currentTile; // 現在ホバー中のタイル

    bool placed = false;

    private void Update()
    {
        UpdatePreview(); // 常にプレビュー更新

        // ==== 右クリックで削除 ====
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceTile tile = hit.collider.GetComponent<PlaceTile>();
                if(tile != null)
                {
                    // placeUnit → UnitStats → タイプ取得
                    UnitStats stats = tile.placedUnit.GetComponent<UnitStats>();

                    if(stats != null)
                    {
                        // GameManager側のカウント減らす
                        GameManager.instance.RemovePlaceUnit(stats.unityType);
                        GameManager.instance.currentTotalPlaced--;
                    }

                    // ユニット削除
                    tile.RemoveUnit();

                    // プレビュー中なら削除
                    DestroyPreview();
                    selectedUnityPrefab = null;
                }
            }
        }

        // ==== 左クリックで配置 ====
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedUnityPrefab == null) return;

            // Prefabからユニットタイプ取得
            UnitStats stats = selectedUnityPrefab.GetComponent<UnitStats>();
            if(stats != null)
            {
                // 上限チェック
                if(!GameManager.instance.CanPlaceUnit(stats.unityType))
                {
                    Debug.Log("上限に達しています");
                    return; // 配置しない
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceTile tile = hit.collider.GetComponent<PlaceTile>();

                if(tile != null)
                {
                    bool placed = tile.PlaceUnit(selectedUnityPrefab);
                    if (placed)
                    {
                        // 配置成功 → 選択解除とカウント増加

                        GameManager.instance.AddPlacedUnit(stats.unityType);

                        GameManager.instance.currentTotalPlaced++;

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
