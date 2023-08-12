using System.Collections.Generic;
using UnityEngine;

public class Kyunho_CardTest : MonoBehaviour
{
    [SerializeField] private List<Kyunho_Card> cards;
    [SerializeField] private Kyunho_CardEventController cardEventController;
    private Kyunho_CardResolver cardResolver;
    private int cardIndex;

    private void Awake()
    {
        cardResolver = new Kyunho_CardResolver(cardEventController);
        foreach (var card in cards)
        {
            Debug.Log(card.Name);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cardIndex--;
            Debug.Log("Index : " + cardIndex);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cardIndex++;
            Debug.Log("Index : " + cardIndex);
        }

        if (cardIndex < 0)
        {
            cardIndex = cards.Count - 1;
        }

        if (cardIndex >= cards.Count - 1)
        {
            cardIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var card = cards[cardIndex];
            cardResolver.UseCard(card);
            cards.Remove(card);
            Debug.Log("UseCard : " + card.Name);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            var card = cards[cardIndex];
            cardResolver.RemoveCard(card);
            cards.Remove(card);
            Debug.Log("RemoveCard : " + card.Name);
        }
    }
}