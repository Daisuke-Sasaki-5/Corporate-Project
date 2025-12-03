using UnityEngine;

public enum RewardType
{
    AddUnit,
    AddMaxUnit, // •ºm‚Ì”z’u”‘‰Á
    AllAttackUp, // ‘S•ºm‚ÌUŒ‚—Í
    AllHPUp, // ‘S•ºm‚ÌHP
    UnitAttackUp, // “Á’è•ºm‚ÌUŒ‚—Í
    UnitHPUp, // “Á’è•ºm‚ÌHP
}

[CreateAssetMenu(fileName = "RewardData",menuName = "Game/RewardData")]
public class RewardData : ScriptableObject
{
    public RewardType rewardType;

    public int value; // ã¸’l
    public UnitStats.UnityType unityType; // “Á’è•ºí—p“r

    public string displayName;
}
