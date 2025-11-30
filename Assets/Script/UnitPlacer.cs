using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    public GameObject selectedUnityPrefab;

    private void Update()
    {
        if (selectedUnityPrefab == null) return;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceTile tile = hit.collider.GetComponent<PlaceTile>();

                if(tile != null)
                {
                    tile.PlaceUnit(selectedUnityPrefab);
                }
            }
        }
    }

    public void SelectUnit(GameObject prefab)
    {
        selectedUnityPrefab = prefab;
    }
}
