using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eun_PlayerStat : Singleton<Eun_PlayerStat>
{
    public int hp;
    public int cost;
    public int day;
    [Range(-50, 50)] public int morality;

    [Range(0, 2)] public int actCount;


    public void CardToStat(Kyunho_Card _card)
    {

    }

    public void SituationFeedBack(Action action)
    {
        action();
    }
}
