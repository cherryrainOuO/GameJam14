using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySystem : MonoBehaviour
{
    public PlayerSystem player;
    public int hp = 10;
    public bool isMiss = false;

    [SerializeField] private Slider enemySlider;
    public TextMeshProUGUI info;

    [SerializeField] private TransitionSys transition;
    [SerializeField] private TextMeshProUGUI hpStr;

    [Header("보스 패턴")]
    [SerializeField] private BossPatternBase[] patterns;



    // Start is called before the first frame update
    void Start()
    {
        enemySlider.maxValue = hp;
        hpStr.text = hp + "";
        StartCoroutine(CoroutineForEnemy());
    }

    private IEnumerator CoroutineForEnemy()
    {
        while (true)
        {
            player.Defense(1);
            GameManager3.Instance.removeCount = 2;

            int rand1 = Random.Range(0, 4); //TODO 에네미 덱 미리 보여주기 (예상?)
            int rand2 = Random.Range(0, 3);

            yield return new WaitUntil(() => !GameManager3.Instance.isPlayerTurn);
            //? 여기서부터 에네미 턴
            if (GameManager3.Instance.isStop) break;

            isMiss = false;
            EnemyDeck(rand1, rand2);

            yield return new WaitUntil(() => !GameManager3.Instance.isPlayerMove);

            GameManager3.Instance.isPlayerTurn = true;
            //? 여기서부터 다시 플레이어 턴
        }
    }

    private void EnemyDeck(int _index1, int _index2)
    {
        switch (_index1)
        {
            case 0:
                Debug.Log("에네미 0번 : hp회복");
                hp = Mathf.Clamp(hp + 1, 0, 10);
                enemySlider.value = hp;
                hpStr.text = hp + "";

                info.text += "에네미 0번 : hp회복 / 에네미 hp : " + hp + "\n";

                Debug.Log("에네미 hp : " + hp);
                break;
            case 1:
                Debug.Log("에네미 1번 : 일반 공격(1)");
                player.Defeat(1);
                info.text += "에네미 1번 : 일반 공격(1)\n";
                break;
            case 2:
                Debug.Log("에네미 2번 : 강 공격(3)");
                player.Defeat(3);
                info.text += "에네미 2번 : 강 공격(3)\n";
                break;
            case 3:
                Debug.Log("에네미 3번 : 회피 증가");
                isMiss = true;
                info.text += "에네미 3번 : 회피 증가\n";
                break;

        }

        switch (_index2)
        {
            case 0:
                Debug.Log("패턴1");
                (patterns[0] as Boss1Pattern1).PatternStart();
                break;
            case 1:
                Debug.Log("패턴2");
                (patterns[1] as Boss1Pattern2).PatternStart();
                break;
            case 2:
                Debug.Log("패턴3");
                (patterns[2] as Boss1Pattern3).PatternStart();
                break;
        }


    }



    public void Defeat(int _defeatHp)
    {
        hp = Mathf.Clamp(hp - _defeatHp, 0, 10);
        enemySlider.value = hp;
        hpStr.text = hp + "";
        Debug.Log("에네미 hp : " + hp);

        if (hp <= 0)
        {
            GameManager3.Instance.isStop = true;
            transition.Transiton(0);
        }
    }
}
