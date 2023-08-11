using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck1 : DeckBase
{

    protected override void Start()
    {
        base.Start();

        deckName = "칼날";
        deckInfo = power + " 만큼 공격";
    }

    protected override void Attack()
    {
        playerSystem.Attack(power);

        base.Attack();
    }

}
