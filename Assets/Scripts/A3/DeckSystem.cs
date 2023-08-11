using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeckSystem : MonoBehaviour
{
    private List<DeckBase> decks;

    // Start is called before the first frame update
    void Start()
    {
        decks = GetComponentsInChildren<DeckBase>(true).ToList();
        Debug.Log(decks.Count);

        Shuffle();
    }

    private void Shuffle()
    {
        foreach (var i in decks) i.gameObject.SetActive(false);

        int rand = Random.Range(0, decks.Count);

        decks[rand].index = rand;
        decks[rand].gameObject.SetActive(true);
    }

    public void RemoveDeck(int _index)
    {
        decks.RemoveAt(_index);

        if (decks.Count == 0)
            decks = GetComponentsInChildren<DeckBase>(true).ToList();

        Shuffle();
    }
}
