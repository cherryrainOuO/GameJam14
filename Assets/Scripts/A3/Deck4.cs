using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck4 : DeckBase
{
    protected override void Start()
    {
        base.Start();

        deckName = "그림자";
        deckInfo = power + " 만큼의 방어력";
    }

    protected override void Attack()
    {
        playerSystem.Defense(power);

        base.Attack();
    }
}
