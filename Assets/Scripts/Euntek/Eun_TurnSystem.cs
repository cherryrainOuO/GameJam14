using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eun_TurnSystem : MonoBehaviour
{
    [SerializeField] private Eun_BehaviorSystem behaviorSystem;
    public bool isPlayerTurn { get; private set; }
    public Kyunho_Card currentCard;

    private void Start()
    {
        MainTurnSystem();
    }


    //! UI 에서 실행 해줬으면 좋겠어요.
    public void TurnFinish(List<Kyunho_Card> _currentCardSet)
    {
        foreach (var i in _currentCardSet)
        {
            PlayerBehavior(true);
        }

        isPlayerTurn = false;
    }


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
    }


    private void EnemyBehavior()
    {

    }
    private void FeedBack()
    {
        //Todo 플레이어 행동에 대한 피드백
        Eun_PlayerStat.Instance.SituationFeedBackToStat();

        //Todo 카드 습득? -> playerDecks 삭제된 카드 자리에 새로 습득한 카드 넣기
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
            FeedBack();

            yield return YieldFunctions.WaitUntil(() => isPlayerTurn);
        }

        //Todo 게임 오버시 나올 내용
    }
}
