using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("設定")]
    public bool isEnemy = false; // 敵か味方か
    [Header("個別にアニメーション設定")]
    public string attackAnimName = " "; // アニメーションクリップの名前指定

    private Animator animator;
    private GameObject target;
    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    private UnitStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
    }


    void Update()
    {
        attackTimer += Time.deltaTime;

        if (target == null)
        {
            target = FindClosestTarget();
            if (target == null) return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > stats.AttackRange)
        {
            MoveTowards(target.transform.position);
        }
        else
        {
            TryAttack();
        }
    }

    /// <summary>
    /// ターゲットを探す処理
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("PlayerUnit");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
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
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * stats.moveSpeed * Time.deltaTime;
        transform.LookAt(targetPos);
    }

    /// <summary>
    /// 攻撃に関する処理
    /// </summary>
    void TryAttack()
    {
        if (attackTimer < attackCooldown) return;

        attackTimer = 0;
        animator.Play(attackAnimName, 0, 0f); // アニメーションの再生

        if (target != null)
        {
            var unitStats = target.GetComponent<UnitStats>();
            if (unitStats != null)
            {
                // 三すくみ補正計算
                float multiplier = stats.GetTypeMultiplier(stats.unityType, unitStats.unityType);

                // 補正をかけた最終ダメージ
                int finalDamage = Mathf.RoundToInt(stats.attackPower * multiplier);

                // 敵にダメージを与える
                unitStats.TakeDamage(stats.attackPower);

                Debug.Log($"{gameObject.name}({stats.unityType})→{target.name}({unitStats.unityType})ダメージ:{finalDamage}");
            }
        }
    }
}
