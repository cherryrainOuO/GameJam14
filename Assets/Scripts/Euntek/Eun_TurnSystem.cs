using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eun_TurnSystem : MonoBehaviour
{
    [SerializeField] private Eun_CardSystem cardSystem;
    [SerializeField] private Kyunho_CardEventController cardEventController;
    public bool isPlayerTurn { get; private set; }


    private int mora = 0; //! 선악도 합산 값.
    private int count = 0; //! 사용한 카드의 개수

    private void Start()
    {
        MainTurnSystem();
    }



    //! UI 에서 실행 해줬으면 좋겠어요.
    public void PlayerBehavior(Kyunho_Card _currentCard, bool _isCardUse)
    {
        Eun_PlayerStat.Instance.behaviorCount--;

        if (_isCardUse)
        {
            count++;
            mora += _currentCard.Morality;
        }

        cardSystem.RemoveCardFromCardList(_currentCard); //? 카드 삭제후 뽑기

        if (_isCardUse) //? 이벤트 실행
        {
            cardEventController.AddEvents(_currentCard.UsingEvent);
            cardEventController.Execute();
            //! 만약 보상으로 카드를 얻는다면 Eun_PlayerStat.Instance.currentBehaviorCount를 해주세요;
        }
        else
        {
            cardEventController.AddEvents(_currentCard.RemovalEvent);
            cardEventController.Execute();
        }
    }

    /// <summary>
    /// Morality 관련 업데이트입니다. 연속적인 선/악행 일수에 따른 가중치가 적용되어 있습니다.
    /// </summary>
    /// <param name="_card"></param>
    private void MoralityUpdate()
    {
        if (count == 0) // 버리기만 했을 경우엔 전날 dayMorality 를 그대로 계승
        {

        }
        else if (mora <= 0) //? 오늘은 악한 날
        {
            if (Eun_PlayerStat.Instance.dayMorality != (int)Kyunho_CardType.Malefeasance) Eun_PlayerStat.Instance.daySequence = 1;
            Eun_PlayerStat.Instance.dayMorality = (int)Kyunho_CardType.Malefeasance;

            Eun_PlayerStat.Instance.morality += (int)(mora * 1.5 * (1 + Eun_PlayerStat.Instance.daySequence * 0.5));
            Eun_PlayerStat.Instance.daySequence++;
        }
        else //? 오늘은 선한 날
        {
            // 카드 합이 0보다 크면 선
            if (Eun_PlayerStat.Instance.dayMorality != (int)Kyunho_CardType.Beneficence) Eun_PlayerStat.Instance.daySequence = 1;
            Eun_PlayerStat.Instance.dayMorality = (int)Kyunho_CardType.Beneficence;

            Eun_PlayerStat.Instance.morality += (int)(mora * 1.25 * (1 + Eun_PlayerStat.Instance.daySequence * 0.25));
            Eun_PlayerStat.Instance.daySequence++;
        }
    }

    private IEnumerator MainTurnSystem()
    {
        while (true)
        {
            //? 플레이어의 턴 입니다.

            yield return YieldFunctions.WaitUntil(() => Eun_PlayerStat.Instance.behaviorCount == 0);

            //? morality 업데이트
            MoralityUpdate();

            //? day 업데이트
            Eun_PlayerStat.Instance.day++;

            if (Eun_PlayerStat.Instance.hp == 0)
            {
                //todo 게임 오버

                break;
            }
            else if (Eun_PlayerStat.Instance.day == 30)
            {
                //todo 게임 클리어

                break;
            }

            Eun_PlayerStat.Instance.behaviorCount = Eun_PlayerStat.Instance.currentBehaviorCount;
            Eun_PlayerStat.Instance.currentBehaviorCount = 2;

            mora = 0;
            count = 0;
        }

    }
}
