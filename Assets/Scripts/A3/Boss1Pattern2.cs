using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss1Pattern2 : BossPatternBase
{
    [SerializeField] private Transform laserSet;
    private Laser[] lasers;

    public override void Start()
    {
        base.Start();

        laserSet.SetParent(null);
        lasers = laserSet.GetComponentsInChildren<Laser>();

        lasers[0].transform.position = new Vector2(-5f, 10f);
        lasers[1].transform.position = new Vector2(0f, 10f);
        lasers[2].transform.position = new Vector2(5f, 10f);

        foreach (var i in lasers)
        {
            i.startPos = i.transform.position;
            i.targetPos = i.transform.position + Vector3.down * 9f;
        }

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

    ///<summary> 파트2 : 짱큰 레이저 발사 </summary>
    private IEnumerator PatternPart2()
    {

        lasers[0].activate = 2; //! 자동 OnOff
        lasers[2].activate = 2; //! 자동 OnOff
        yield return new WaitForSeconds(1f);

        lasers[1].activate = 2; //! 자동 OnOff

        yield return new WaitForSeconds(1f);

        lasers[0].activate = 2; //! 자동 OnOff
        lasers[2].activate = 2; //! 자동 OnOff

        yield return new WaitForSeconds(1f);

        StartCoroutine(ResetPos());
    }


    //////////////////////////////////////////////////////////////
    #endregion
}