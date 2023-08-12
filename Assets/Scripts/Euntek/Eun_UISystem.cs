using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Eun_UISystem : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Eun_CardSystem cardSystem;
    [SerializeField] private Eun_TurnSystem turnSystem;

    [SerializeField] private TextMeshProUGUI text;

    private int cardIndex;

    private int currentIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }


    private IEnumerator Animation()
    {
        float time = 0f;

        for (int i = 0; i < 9; i++)
        {
            background.sprite = sprites[i];

            yield return YieldFunctions.WaitForSeconds(.7f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cardIndex--;
            Debug.Log("Index : " + cardIndex);

            if (cardIndex < 0)
            {
                cardIndex = cardSystem.playerDecks.Count - 1;
            }

            text.text = cardSystem.playerDecks[cardIndex].name;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cardIndex++;
            Debug.Log("Index : " + cardIndex);

            if (cardIndex >= cardSystem.playerDecks.Count - 1)
            {
                cardIndex = 0;
            }

            text.text = cardSystem.playerDecks[cardIndex].name;
        }





        if (Input.GetKeyDown(KeyCode.Z))
        {
            var card = cardSystem.playerDecks[cardIndex];
            turnSystem.PlayerBehavior(card, true);
            Debug.Log("UseCard : " + card.Name);

            text.text = cardSystem.playerDecks[cardIndex].name;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            var card = cardSystem.playerDecks[cardIndex];
            turnSystem.PlayerBehavior(card, false);
            Debug.Log("RemoveCard : " + card.Name);

            text.text = cardSystem.playerDecks[cardIndex].name;
        }
    }


}
