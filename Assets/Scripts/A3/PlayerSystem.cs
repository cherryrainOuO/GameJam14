using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSystem : MonoBehaviour
{
    [SerializeField] private EnemySystem enemySystem;

    private int hp = 10;
    private int defense = 1;

    [SerializeField] private Slider playerSlider;
    [SerializeField] private TransitionSys transition;

    [SerializeField] private TextMeshProUGUI hpStr;

    // Start is called before the first frame update
    void Start()
    {
        playerSlider.maxValue = hp;
        hpStr.text = hp + "";
    }


    public void Attack(int _power)
    {
        if (enemySystem.isMiss)
        {
            if (Random.Range(0, 2) == 0)
            {
                Debug.Log("공격 성공");
                enemySystem.Defeat(_power);
                enemySystem.info.text = "에네미 " + _power + " 만큼 공격 받음.\n";
            }
            else
            {
                Debug.Log("회피로 인한 공격 실패");
                enemySystem.info.text = "에네미 회피\n";
            }
        }
        else
        {
            Debug.Log("공격 성공");
            enemySystem.Defeat(_power);
            enemySystem.info.text = "에네미 " + _power + " 만큼 공격 받음.\n";
        }
    }
    public void Defeat(int _defeatHp)
    {
        hp = Mathf.Clamp(hp - (_defeatHp / defense), 0, 10);
        playerSlider.value = hp;
        hpStr.text = hp + "";
        Debug.Log("플레이어 hp : " + hp);

        if (hp <= 0)
        {
            GameManager3.Instance.isStop = true;
            transition.Transiton(1);
        }
    }

    public void Recovery(int _recovery)
    {
        hp = Mathf.Clamp(hp + _recovery, 0, 10);
        playerSlider.value = hp;
        hpStr.text = hp + "";
        Debug.Log("플레이어 hp : " + hp);
        enemySystem.info.text = "플레이어 회복\n";
    }

    public void Defense(int _defense)
    {
        defense = _defense;
        Debug.Log("플레이어 방어력 : " + defense);
    }

}
