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

    [Range(0, 2)] public int actCount; //! 행동 제한 개수

    public int egg;

    private int dayMorality = 1; //? 첫날 악으로 시작
    private int daySequence = 1;
    private enum dayMoralitySequence { Beneficence, Malefeasance };

    public void StatUpdate(Kyunho_Card[] _cards)
    {
        //Todo 악행 or 선행 연속 판별용 마일리지

        int mora = 0; //! 선악도 합산 값.
        int count = 0; //! 사용한 카드의 개수

        foreach (var i in _cards)
        {
            count++;
            mora += i.Morality;
        }

        CardToStat(_cards, mora, count);
        SituationFeedBackToStat(count);

        if (hp == 0)
        {
            //todo 게임 오버
        }
        else if (day == 30)
        {
            //todo 게임 클리어
        }
    }

    /// <summary>
    /// Morality 관련 업데이트입니다. 연속적인 선/악행 일수에 따른 가중치가 적용되어 있습니다.
    /// </summary>
    /// <param name="_card"></param>
    private void MoralityUpdate(Kyunho_Card[] _card, int _mora, int _count)
    {
        if (_count == 0) // 버리기만 했을 경우엔 전날 dayMorality 를 그대로 계승
        {

        }
        else if (_mora <= 0) //? 오늘은 악한 날
        {
            if (dayMorality != (int)dayMoralitySequence.Malefeasance) daySequence = 1;
            dayMorality = (int)dayMoralitySequence.Malefeasance;

            morality += (int)(_mora * 1.5 * (1 + daySequence * 0.5));
            daySequence++;
        }
        else //? 오늘은 선한 날
        {
            // 카드 합이 0보다 크면 선
            if (dayMorality != (int)dayMoralitySequence.Beneficence) daySequence = 1;
            dayMorality = (int)dayMoralitySequence.Beneficence;

            morality += (int)(_mora * 1.25 * (1 + daySequence * 0.25));
            daySequence++;
        }
    }


    public void CardToStat(Kyunho_Card[] _card, int _mora, int _count)
    {

        //? morality 업데이트
        MoralityUpdate(_card, _mora, _count);

        //? cost 업데이트

        //? day 업데이트
        day++;

        //Todo 카드가 스탯에 적용
    }

    public void SituationFeedBackToStat(int _count)
    {
        //Todo 상황에 대한 피드백이 스탯에 적용

        //? 달걀 업데이트
        if (_count != 0)
            EggUpdate();

        //? hp 업데이트
        HpUpdate();


    }

    private void HpUpdate()
    {




        if (egg <= 0) hp--; //? 식량이 없다면 hp가 준다.
    }

    private void EggUpdate()
    {
        switch ((int)(day / 7))
        {
            case 0: //? 1주차
                egg++;
                break;
            case 1:
                egg += 2;
                break;
            case 2:
                egg += 2;
                break;
            case 3: //? 4주차
                egg += 2;
                break;

        }
    }
}
