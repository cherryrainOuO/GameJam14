using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck2 : DeckBase
{

    protected override void Start()
    {
        base.Start();

        deckName = "칼날2";
        deckInfo = "HP를 사용하여 " + power + " 만큼 공격";
    }

    protected override void Attack()
    {
        playerSystem.Attack(power);
        playerSystem.Defeat(power);

        base.Attack();
    }
}
