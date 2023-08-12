using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Eun_CardSystem : MonoBehaviour
{
    //? 모든 카드가 들어 있어요. 
    private List<Kyunho_Card> cards;

    //? 현재 플레이어가 선택할 수 있는 카드예요.
    public List<Kyunho_Card> playerDecks = new List<Kyunho_Card>();

    //[SerializeField] private TextMeshProUGUI[] text;
    private Kyunho_Card duplicationCard = null;
    private int duplicationCount = 1;

    [SerializeField, Range(1, 10)] private int percent = 10;

    private void Start()
    {
        Init();

        FirstShuffle();
    }

    private void Init()
    {
        cards = Resources.LoadAll<Kyunho_Card>("").ToList();

        Debug.Log(cards.Count);
    }

    private void FirstShuffle()
    {
        List<Kyunho_Card> beneficenceCards = cards.Where((a) => a.Type == Kyunho_CardType.Beneficence).ToList();

        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, beneficenceCards.Count);

            cards.Remove(beneficenceCards[rand]);
            playerDecks.Add(beneficenceCards[rand]);
            beneficenceCards.RemoveAt(rand);
        }

        List<Kyunho_Card> malefeasanceCards = cards.Where((a) => a.Type == Kyunho_CardType.Malefeasance).ToList();

        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, malefeasanceCards.Count);

            cards.Remove(malefeasanceCards[rand]);
            playerDecks.Add(malefeasanceCards[rand]);
            malefeasanceCards.RemoveAt(rand);
        }

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

        if (duplicationCard != null)
        {
            bool result = CheckDuplication();

            if (result) //! 중복 제거에 실패함.
            {
                playerDecks.Add(duplicationCard);
                cards.Remove(duplicationCard);

                //Todo

                duplicationCard = null;
            }
            else //! 이번 카드는 중복 제거에 성공
            {
                playerDecks.Add(cards[rand]);
                cards.RemoveAt(rand);
            }

            duplicationCount++;
        }
        else
        {
            playerDecks.Add(cards[rand]);
            cards.RemoveAt(rand);
        }



        //Todo rand 인덱스의 객체를 보이게하기
        //cards[rand].gameObject.SetActive(true);
    }

    private bool CheckDuplication()
    {
        int duplicationPercent = Mathf.Abs(10 / duplicationCount);

        int rand = Random.Range(0, duplicationPercent);

        if (rand == 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    private int CheckMorality()
    {
        //? 뽑을 확률은? 
        int moralityPercent = Mathf.Abs(Eun_PlayerStat.Instance.morality / percent);

        int rand = Random.Range(0, cards.Count);

        while (true)
        {
            if (Eun_PlayerStat.Instance.morality > 0 && cards[rand].Type == Kyunho_CardType.Beneficence)
                break;
            else if (Eun_PlayerStat.Instance.morality <= 0 && cards[rand].Type == Kyunho_CardType.Malefeasance)
                break; //? 성악설 0 일때도 악
            else
            {
                if (Random.Range(0, moralityPercent) == 0)
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

        if (cards.Count == 0)
        {
            Debug.Log("카드리스트 비어서 다시 Init~");
            duplicationCard = _card;
            duplicationCount = 1;
            Init(); //! 근데 이거 playerDeck 에 있는건 다시 빼야하지 않나??
        }


        //Todo 만약 습득한 카드가 있다면 셔플 하지 않기

        // if(습득 카드 있음) playerDecks.Add(습득 카드)
        // else
        Shuffle();
    }

    public void AddRewardCard()
    {
        Eun_PlayerStat.Instance.currentBehaviorCount++;

        if (cards.Count == 0)
        {
            Debug.Log("카드리스트 비어서 다시 Init~");
            Init();
        }

        Shuffle();
    }

}
