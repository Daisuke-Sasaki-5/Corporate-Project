using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [Header("設定")]
    public bool isEnemy = false; // 敵か味方か
    [Header("個別にアニメーション設定")]
    public string attackAnimName = " "; // アニメーションクリップの名前指定

    private Animator animator;
    private GameObject target;
    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    bool isAttacking = false;

    private UnitStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
    }

    /// <summary>
    /// プレイヤーユニットの動き
    /// </summary>
    void Update()
    {
        // 攻撃中は何もしない
        if (isAttacking) return;

        attackTimer += Time.deltaTime;

        // ターゲット変更すべき状況なら再取得
        if(ShouldChangeTarget())
        {
            target = FindClosestTarget();
        }

        // まだターゲットがいなければターゲットを探す
        if (target == null)
        {
            target = FindClosestTarget();
            if(target == null )
            {
                return;
            }
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        // 射程外ならターゲット近くに移動
        if(distance > stats.AttackRange)
        {
            MoveTowards(target.transform.position);
        }
        else
        {
            // 近くにいるのなら攻撃
            TryAttack();
        }
    }

    /// <summary>
    /// ターゲットを探す処理
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        string targetTag = isEnemy ? "PlayerUnit" : "EnemyUnit";
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        // 自身に最も近い敵を登録
        foreach(var t in targets)
        {
            float dist = Vector3.Distance(transform.position,t.transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }
        return closest;
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
        transform.LookAt(moveTarget);

        Debug.Log($"{gameObject.name} moving towards {moveTarget}");
    }

    /// <summary>
    /// 攻撃に関する処理
    /// </summary>
    void  TryAttack()
    {
        if(attackTimer < attackCooldown) return;

        attackTimer = 0;
        animator.Play(attackAnimName, 0, 0f); // アニメーションの再生

        if(target != null)
        {
            var enemyStats = target.GetComponent<UnitStats>();
            if(enemyStats != null )
            {
                // 三すくみ補正計算
                float multiplier = stats.GetTypeMultiplier(stats.unityType, enemyStats.unityType);

                // 補正をかけた最終ダメージ
                int finalDamage = Mathf.RoundToInt(stats.attackPower * multiplier);

                // 敵にダメージを与える
                enemyStats.TakeDamage(finalDamage);

                Debug.Log($"{gameObject.name}({stats.unityType})→{target.name}({enemyStats.unityType})ダメージ:{finalDamage}");
            }
        }
    }

    // ターゲット変更条件を設定
    bool ShouldChangeTarget()
    {
        if(target == null) return true;

        var stats = target.GetComponent<UnitStats>();

        // ターゲットが死んだとき
        if (stats == null || stats.IsDead) return true;

        // ターゲットが射程外に出た時
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if(dist > stats.AttackRange * 1.5f) return true;

        return false;
    }
}
