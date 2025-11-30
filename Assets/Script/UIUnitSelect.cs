using UnityEngine;

public class UIUnitSelect : MonoBehaviour
{
    public UnitPlacer placer;

    public GameObject swordPrefab;
    public GameObject spearPrefab;
    public GameObject bowPrefab;

    public void PickSword()
    {
        placer.SelectUnit(swordPrefab);
    }

    public void PickSpear()
    {
        placer.SelectUnit(spearPrefab);
    }

    public void PickBow()
    {
       placer.SelectUnit(bowPrefab);
    }
}
