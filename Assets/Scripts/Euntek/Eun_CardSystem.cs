using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Eun_CardSystem : MonoBehaviour
{
    //? 모든 카드가 들어 있어요. 
    private List<Kyunho_Card> cards;

    //? 현재 플레이어가 선택할 수 있는 카드예요.
    private List<Kyunho_Card> playerDecks = new List<Kyunho_Card>();


    [SerializeField, Range(1, 10)] private int percent = 10;

    private void Start()
    {
        Init();

        for (int i = 0; i < 3; i++)
            Shuffle();

    }

    private void Init()
    {
        cards = Resources.LoadAll<Kyunho_Card>("").ToList();

        Debug.Log(cards.Count);
    }


    private string ToString(Kyunho_Card _card)
    {
        string str = _card.Name + "\n";
        str += _card.Cost + "\n";
        str += _card.Morality + "\n";

        str += "==============\n";
        return str;
    }

    private void Shuffle()
    {
        //Todo 모든 인덱스 객체 안보이게 하기
        //foreach (var i in cards) i.gameObject.SetActive(false);

        //? 성향에 따라 보여주는 카드 다름
        int rand = CheckMorality();

        playerDecks.Add(cards[rand]);

        //Todo rand 인덱스의 객체를 보이게하기
        //cards[rand].gameObject.SetActive(true);
    }

    private int CheckMorality()
    {
        //? 뽑을 확률은? 
        int morality = Mathf.Abs(Eun_PlayerStat.Instance.morality / percent);

        int rand = Random.Range(0, cards.Count);

        while (true)
        {
            if (Eun_PlayerStat.Instance.morality >= 0 && cards[rand].Type == Kyunho_CardType.Beneficence)
                break;
            else if (Eun_PlayerStat.Instance.morality < 0 && cards[rand].Type == Kyunho_CardType.Malefeasance)
                break;
            else
            {
                if (Random.Range(0, morality) == 0)
                    break;

                rand = Random.Range(0, cards.Count);
            }
        }

        return rand;
    }


    /// <summary>
    /// 카드를 버렸을 시, 만약 리스트에 카드가 없다면 다시 불러옵니다.
    /// </summary>
    /// <param name="_index"></param>
    public void RemoveCardFromCardList(Kyunho_Card _card)
    {
        //Todo 기존 카드는 어떻게 할 것 인지?
        playerDecks.Remove(_card);

        Shuffle();
    }

}
