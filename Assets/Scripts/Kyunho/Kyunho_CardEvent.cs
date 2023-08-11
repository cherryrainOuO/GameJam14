using UnityEngine;

[System.Serializable]
public class Kyunho_CardEvent : Kyunho_ICardEvent
{
    [SerializeField] private string description;
    [SerializeField] private Kyunho_Reward[] reward;

    public string Description { get => description; }
    public Kyunho_Reward[] Reward { get => reward;}
}

[System.Serializable]
public class Kyunho_Reward : Kyunho_IReward
{
    [SerializeField] private RewardType rewardType;
    [SerializeField] private int amount;

    public RewardType RewardType { get => rewardType; }
    public int Amount { get => amount; }
}

public enum RewardType { Card,  }

interface Kyunho_IReward
{
    RewardType RewardType { get; }
    int Amount { get; }
}