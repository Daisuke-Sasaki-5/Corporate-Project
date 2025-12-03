using UnityEngine;

public class HPBarLookAt : MonoBehaviour
{
    private void LateUpdate()
    {
        var cam = Camera.main;
        if (cam == null) return;

        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
