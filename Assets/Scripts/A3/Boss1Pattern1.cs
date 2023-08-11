using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss1Pattern1 : BossPatternBase
{
    private Laser laser; //! 로컬웨이포인트 = 레이저 + 2(처음과 마지막)

    public override void Start()
    {
        base.Start();

        laser = GetComponentInChildren<Laser>();
    }


    #region 파트
    //////////////////////////////////////////////////////////////
    public void PatternStart()
    {
        StartCoroutine(PatternPart1());
    }

    ///<summary> 파트1 : 웨이포인트를 따라서 이동 </summary>
    private IEnumerator PatternPart1()
    {
        float time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime;

            controller.transform.position = Vector2.Lerp(originPos, targetPos, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }

        StartCoroutine(PatternPart2());
    }


    ///<summary> 파트2 : 레이저 발사 </summary>

    private IEnumerator PatternPart2()
    {
        float time = 0f;
        while (time <= 1.5f)
        {
            time += Time.deltaTime;
            Vector2 unit = (controller.player.transform.position - transform.position).normalized;

            laser.transform.right = unit;

            laser.startPos = controller.transform.position;
            laser.targetPos = controller.transform.position + Vector3.right * 20f;
            laser.activate = 2; //! 자동 OnOff

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(ResetPos());
    }

    //////////////////////////////////////////////////////////////
    #endregion
}
