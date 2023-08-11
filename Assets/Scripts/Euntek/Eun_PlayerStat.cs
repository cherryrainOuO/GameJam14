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
        //Todo 카드가 스탯에 적용
    }

    public void SituationFeedBackToStat()
    {
        //Todo 상황에 대한 피드백이 스탯에 적용
    }
}
