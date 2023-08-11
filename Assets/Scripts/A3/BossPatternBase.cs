using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternBase : MonoBehaviour
{
    public Vector2 originPos;
    public Vector2 targetPos;

    protected EnemySystem controller;


    public virtual void Start()
    {
        controller = GetComponentInParent<EnemySystem>();
    }


    public IEnumerator ResetPos()
    {
        float time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime;

            controller.transform.position = Vector2.Lerp(targetPos, originPos, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }

        GameManager3.Instance.isPlayerMove = false;

        time = 0f;
        Vector2 playerTar = controller.player.transform.position;
        Vector2 playerOri = new(-5f, 1.2f);

        while (time <= 1f)
        {
            time += Time.deltaTime;

            controller.player.transform.position = Vector2.Lerp(playerTar, playerOri, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }

        Debug.Log("이동 가능 끝");
    }
}
