using System.Collections.Generic;
using UnityEngine;

public class Kyunho_RewardResolver
{
    private Dictionary<Kyunho_RewardType, Kyunho_PlayerStatController> rewardMap;

    public Kyunho_RewardResolver(Eun_PlayerStat playerStat)
    {
        rewardMap = new Dictionary<Kyunho_RewardType, Kyunho_PlayerStatController>()
        {
            { Kyunho_RewardType.HP, new Kyunho_PlayerHPController(playerStat) },
            { Kyunho_RewardType.Egg, new Kyunho_PlayerEggController(playerStat) },
            { Kyunho_RewardType.Morality, new Kyunho_PlayerMoralityController(playerStat) }
        };
    }

    public void Resolve(Kyunho_IReward[] rewards)
    {
        foreach (var reward in rewards)
        {
            if (rewardMap.TryGetValue(reward.RewardType, out var value))
            {
                value.Execute(reward.Amount);
            }
        }
    }
}

public abstract class Kyunho_PlayerStatController
{
    protected Eun_PlayerStat player;

    public Kyunho_PlayerStatController(Eun_PlayerStat playerStat)
    {
        player = playerStat;
    }

    public abstract void Execute(int value);
}

public class Kyunho_PlayerMoralityController : Kyunho_PlayerStatController
{
    public Kyunho_PlayerMoralityController(Eun_PlayerStat playerStat) : base(playerStat)
    {
    }

    public override void Execute(int value)
    {
        player.morality += value;
        Debug.Log("Morality + " + value);
    }
}

public class Kyunho_PlayerHPController : Kyunho_PlayerStatController
{
    public Kyunho_PlayerHPController(Eun_PlayerStat playerStat) : base(playerStat)
    {
    }

    public override void Execute(int value)
    {
        player.hp += value;
        Debug.Log("HP + " + value);
    }
}

public class Kyunho_PlayerEggController : Kyunho_PlayerStatController
{
    public Kyunho_PlayerEggController(Eun_PlayerStat playerStat) : base(playerStat)
    {
    }

    public override void Execute(int value)
    {
        player.egg += value;
        Debug.Log("Egg + " + value);
    }
}

public class Kyunho_PlayerCardController : Kyunho_PlayerStatController
{
    public Kyunho_PlayerCardController(Eun_PlayerStat playerStat) : base(playerStat)
    {
    }

    public override void Execute(int value)
    {
        Debug.Log("Card + " + value);
    }
}