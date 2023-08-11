using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck3 : DeckBase
{
    protected override void Start()
    {
        base.Start();

        deckName = "물약";
        deckInfo = power + " 만큼 HP 회복";
    }

    protected override void Attack()
    {
        playerSystem.Recovery(power);

        base.Attack();
    }
}
