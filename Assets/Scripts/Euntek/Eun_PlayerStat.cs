using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Eun_PlayerStat : Singleton<Eun_PlayerStat>
{
    public int hp;
    public int day;
    [Range(-50, 50)] public int morality;

    [Range(0, 4)] public int behaviorCount = 2; //! 행동력
    public int currentBehaviorCount = 2; //! 행동력 temp

    public int egg;

    public int dayMorality = 1; //? 첫날 악으로 시작
    public int daySequence = 1;

}
