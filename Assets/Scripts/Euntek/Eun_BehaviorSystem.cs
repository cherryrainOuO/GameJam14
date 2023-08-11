using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eun_BehaviorSystem : MonoBehaviour
{
    [SerializeField] private Eun_CardSystem cardSystem;

    /// <summary>
    /// 카드 사용시에 사용할 내용입니다. 
    /// </summary>
    /// <param name="_card"></param>
    public void UseCard(Kyunho_Card _card)
    {
        //TODO 행동 취소 가능할지 여부?

        Eun_PlayerStat.Instance.CardToStat(_card);
        cardSystem.RemoveCard(_card);
    }

    /// <summary>
    /// 카드 버릴때 나오는 내용입니다.
    /// </summary>
    /// <param name="_card"></param>
    public void RemoveCard(Kyunho_Card _card)
    {
        cardSystem.RemoveCard(_card);
    }
}
