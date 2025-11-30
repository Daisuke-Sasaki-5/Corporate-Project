using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("戦闘パラメーター")]
    public int attackPower = 10; // 攻撃力
    public float AttackRange = 0.5f; // 攻撃範囲
    public float moveSpeed = 1f; // 移動速度
    public int hp = 100; // 体力

    [Header("ユニットのタイプ")]
    public UnityType unityType;

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
        hp -= damage;
        if (hp <= 0)
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

    void Die()
    {
        Destroy(gameObject);
    }
}
