using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eun_PlayerStat : Singleton<Eun_PlayerStat>
{
    public int hp;
    public int cost;
    public int day;
    [Range(-50, 50)] public int morality;

    private void Start()
    {

    }
}
