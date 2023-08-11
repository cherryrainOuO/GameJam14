using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eun_TurnSystem : MonoBehaviour
{
    [SerializeField] private Eun_BehaviorSystem behaviorSystem;
    [SerializeField] private Eun_CardSystem cardSystem;
    public bool isPlayerTurn { get; private set; }
    public Kyunho_Card currentCard;

    private void Start()
    {
        MainTurnSystem();
    }


    //! UI 에서 실행 해줬으면 좋겠어요.
    public void TurnFinish(Kyunho_Card[] _currentCardSet)
    {
        Eun_PlayerStat.Instance.StatUpdate(_currentCardSet);

        foreach (var i in _currentCardSet)
        {
            cardSystem.RemoveCardFromCardList(i);
        }

        isPlayerTurn = false;
    }


    /*
    private void PlayerBehavior(bool _isCardUse)
    {
        if (_isCardUse) //? 카드사용 or 카드 버리기
        {
            behaviorSystem.UseCard(currentCard);
        }
        else
        {
            behaviorSystem.RemoveCard(currentCard);
        }
    }*/


    private void EnemyBehavior()
    {

    }


    private IEnumerator MainTurnSystem()
    {
        while (true)
        {
            //? 플레이어의 턴 입니다.
            // PlayerBehavior();

            yield return YieldFunctions.WaitUntil(() => !isPlayerTurn);

            //Todo 상대방의 턴 입니다. ~> 이벤트 서비스
            EnemyBehavior();

            yield return YieldFunctions.WaitUntil(() => isPlayerTurn);
        }

        //Todo 게임 오버시 나올 내용
    }
}
