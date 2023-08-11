using UnityEngine;

public enum Kyunho_RewardType { Card, DeckCard, HP, Egg, Morality }

public interface Kyunho_IReward
{
    Kyunho_RewardType RewardType { get; }
    int Amount { get; }
}

[System.Serializable]
public class Kyunho_Reward : Kyunho_IReward
{
    [SerializeField] private Kyunho_RewardType rewardType;
    [SerializeField] private int amount;

    public Kyunho_RewardType RewardType { get => rewardType; }
    public int Amount { get => amount; }
}