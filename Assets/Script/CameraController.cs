using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Vector3 startPosition = new Vector3(0.01f, 6.03f, -4.98f);
    public Vector3 battlePosition = new Vector3(0.01f, 2.85f, -2.59f);

    public Vector3 startRotation = new Vector3(51.753f, 0f, 0f);
    public Vector3 battleRotation = new Vector3(51.753f, 0f, 0f);

    public float moveDuration = 1.5f;

    void Start()
    {
        // 初期位置を明示的にリセット
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);
    }

    // 戦闘開始時カメラを動かす
    public void MoveToBattle()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamera(battlePosition, battleRotation));
    }

    private IEnumerator MoveCamera(Vector3 targetPos, Vector3 targetRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(targetRot);

        float time = 0f;

        while(time < moveDuration)
        {
            time += Time.unscaledDeltaTime;

            float t = time / moveDuration;
            float smoothT =  Mathf.SmoothStep(0f,1f,t); // なめらか化

            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            transform.rotation = Quaternion.Slerp(startRot, endRot, smoothT);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = endRot;
    }

    // カメラ位置リセット
    public void ResetToStart()
    {
        StopAllCoroutines();
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);
    }
}
