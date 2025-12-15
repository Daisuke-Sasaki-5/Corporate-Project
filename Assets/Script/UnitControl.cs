using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    public enum UnitState
    {
        Advance, // 前進フェーズ
        Battle // 戦闘フェーズ
    }


    [Header("設定")]
    public bool isEnemy = false; // 敵か味方か
    public UnitState state = UnitState.Advance;

    [Header("個別にアニメーション設定")]
    public string attackAnimName = " "; // アニメーションクリップの名前指定

    private Animator animator;
    private GameObject target;
    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    private UnitStats stats;

    private Vector3 addvanceDirection;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();

        // 敵と味方で進む向きを変える
        addvanceDirection = isEnemy ? Vector3.back : Vector3.forward;
    }

    /// <summary>
    /// プレイヤーユニットの動き
    /// </summary>
    void Update()
    {
        attackTimer += Time.deltaTime;

        switch (state)
        {
            case UnitState.Advance:
                UpdateAdvance();
                break;
            case UnitState.Battle:
                UpdateBattle();
                break;
        }
    }

    private void LateUpdate()
    {
        var r = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, r.y, 0);
    }

    /// <summary>
    /// 前進フェーズ
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpdateAdvance()
    {
        // まっすぐ前進
        transform.position += addvanceDirection * stats.moveSpeed * Time.deltaTime;

        // ==== 探索処理 ====
        GameObject nearest = FindClosestTarget();
        if(nearest != null)
        {
            float dist = Vector3.Distance(transform.position, nearest.transform.position);
            if(dist < 6f)
            {
                // ターゲット発見 → 戦闘フェーズへ移行
                state = UnitState.Battle;
                target = nearest;
                return;
            }

            // 敵に少しだけ寄る
            Vector3 dir = (nearest.transform.position - transform.position).normalized;
            addvanceDirection = Vector3.Lerp(addvanceDirection, dir, 0.3f);

            transform.rotation = Quaternion.LookRotation(addvanceDirection);
        }

        if(!isAttacking)
        {
            transform.rotation = Quaternion.LookRotation(addvanceDirection);
        }
    }

    /// <summary>
    /// 戦闘フェーズ
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpdateBattle()
    {
        target = null;

        // ターゲットがいない、もしくは死亡していたら再検索
        if (target == null || IsDeadTarget(target))
        {
            target = FindClosestTarget();
            if (target == null) return; // いなければ何もしない
        }

        float distance = Vector3.Distance(transform.position,target.transform.position);

        // 射程外 → 近づく
        if(distance > stats.AttackRange)
        {
            MoveTowards(target.transform.position);
        }
        else
        {
            // 射程内 → 攻撃
            Debug.Log($"[ATTACK] {gameObject.name} AP:{stats.attackPower}");
            TryAttack();
        }
    }


    private bool IsDeadTarget(GameObject t)
    {
        var s = t.GetComponent<UnitStats>();
        return (s == null || s.IsDead);
    }

    private GameObject FindClosestTarget()
    {
        string targetTag = isEnemy ? "PlayerUnit" : "EnemyUnit";
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject best = null;
        float bestScore = Mathf.Infinity;

        foreach (var t in targets)
        {
            Vector3 toTarget = (t.transform.position - transform.position);
            float dist = toTarget.magnitude;

            // 方向の類似度
            float dot = Vector3.Dot(addvanceDirection.normalized, toTarget.normalized);

            // 正面補正
            float frontBonus = Mathf.Clamp(dot,0,1f) * 2f;

            float randomBias = UnityEngine.Random.Range(0f,0.3f);

            // スコア：距離 - 正面補正
            float score = dist - frontBonus + randomBias;

            if(score < bestScore)
            {
                bestScore = score;
                best = t;
            }
        }
        return best;
    }

    /// <summary>
    /// 移動に関する処理
    /// </summary>
    /// <param name="targetPos"></param>
    void MoveTowards(Vector3 targetPos)
    {
        // ==== わずかにランダムでずれた位置を目標にする
        Vector3 offSet = new Vector3(Mathf.Sin((gameObject.GetHashCode() % 10) * 0.3f) * 0.5f, 0, Mathf.Cos((gameObject.GetHashCode() % 10) * 0.3f) * 0.5f);
        Vector3 moveTarget = targetPos + offSet;

        Vector3 dir = (moveTarget - transform.position).normalized;
        transform.position += dir * stats.moveSpeed * Time.deltaTime;
        if(!isAttacking)
        {
            transform.LookAt(moveTarget);
        }
        //Debug.Log($"{gameObject.name} moving towards {moveTarget}");
    }

    /// <summary>
    /// 攻撃に関する処理
    /// </summary>
    void TryAttack()
    {
        if (attackTimer < attackCooldown) return;

        attackTimer = 0;
        animator.Play(attackAnimName, 0, 0f); // アニメーションの再生

        // 攻撃中扱いにする(短時間)
        StartCoroutine(AttackRotationLock(0.4f));

        if (target != null)
        {
            var enemyStats = target.GetComponent<UnitStats>();
            if (enemyStats != null)
            {
                // 三すくみ補正計算
                float multiplier = stats.GetTypeMultiplier(stats.unityType, enemyStats.unityType);

                // 補正をかけた最終ダメージ
                int finalDamage = Mathf.RoundToInt(stats.attackPower * multiplier);
                Debug.Log(stats.attackPower);

                // 敵にダメージを与える
                enemyStats.TakeDamage(finalDamage);

                Debug.Log($"{gameObject.name}({stats.unityType})→{target.name}({enemyStats.unityType})ダメージ:{finalDamage}");
            }
        }
    }

    private IEnumerator AttackRotationLock(float v)
    {
        isAttacking = true;
        yield return new WaitForSeconds(v);
        isAttacking = false;
    }
}
