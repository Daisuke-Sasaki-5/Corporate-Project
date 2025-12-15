using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    [Header("戦闘パラメーター")]
    public int baseattackPower = 10; // 攻撃力
    public float AttackRange = 0.5f; // 攻撃範囲
    public float moveSpeed = 1f; // 移動速度
    public int baseMaxhp = 100; // 体力
    public int currentHP;

    [Header("バフ適用後ステータス")]
    public int attackPower;
    public int MaxHP;

    [HideInInspector] public Slider hpslider;

    [Header("ユニットのタイプ")]
    public UnityType unityType;

  
    private void Awake()
    {
        // 初期値を基礎値から作る
        attackPower = baseattackPower;
        MaxHP = baseMaxhp;
        currentHP = MaxHP;
    }

    // ユニットタイプ　剣　槍　弓
    public enum UnityType
    {
        Sword,
        Spear,
        Bow,
    }

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);

        if(hpslider != null)
            hpslider.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public float GetTypeMultiplier(UnityType attacker, UnityType defender)
    {
        // 三すくみ

        // 剣 > 槍 > 弓 > 剣
        if (attacker == UnityType.Sword && defender == UnityType.Spear) return 1.5f;
        if (attacker == UnityType.Spear && defender == UnityType.Bow) return 1.5f;
        if (attacker == UnityType.Bow && defender == UnityType.Sword) return 1.5f;

        // 不利側
        if (attacker == UnityType.Spear && defender == UnityType.Sword) return 0.75f;
        if (attacker == UnityType.Bow && defender == UnityType.Spear) return 0.75f;
        if (attacker == UnityType.Sword && defender == UnityType.Bow) return 0.75f;

        // 同一タイプ、それ以外
        return 1.0f;
    }

    public bool IsDead => currentHP <= 0;
    public void Die()
    {
        Destroy(gameObject);
    }

    public void IntializeStarts()
    {
        UnitManager.instance.ApplyStatsToUnit(this);
    }
}
