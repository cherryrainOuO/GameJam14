using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss1Pattern3 : BossPatternBase
{
    [SerializeField] private Transform spikeAxis;
    [SerializeField][Range(10, 15)] private float rotateSpeed = 10f;
    [SerializeField] private bool reverse = false;

    private Quaternion startAngle;
    private Quaternion targetAngle;

    private Vector3 startScale;
    private Vector3 targetScale;


    public override void Start()
    {
        base.Start();

        startAngle = (!reverse) ? Quaternion.Euler(0f, 0f, 30f) : Quaternion.Euler(0f, 0f, 30f);
        targetAngle = (!reverse) ? Quaternion.Euler(0f, 0f, 130f) : Quaternion.Euler(0f, 0f, -60f);

        startScale = new Vector3(1f, 1f, 1f);
        targetScale = new Vector3(7f, 7f, 1f);

    }

    #region 파트
    //////////////////////////////////////////////////////////////


    public void PatternStart()
    {
        spikeAxis.gameObject.SetActive(true);
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

    private IEnumerator PatternPart2()
    {
        float time = 0f;
        spikeAxis.transform.SetParent(null);

        while (time <= 1f)
        {

            time += Time.deltaTime * rotateSpeed / 12f;
            float easedtime = EasingFunctions.easeInCubic(time, 5); // easeInCubic

            spikeAxis.transform.localScale = Vector3.Lerp(startScale, targetScale, easedtime);

            spikeAxis.rotation = startAngle;

            yield return null;
        }

        time = 0f;

        //? 내려찍기
        spikeAxis.transform.localScale = targetScale;

        while (time <= 1f)
        {
            time += Time.deltaTime * rotateSpeed / 12f;
            float easedtime = EasingFunctions.easeInCubic(time, 5); // easeInCubic

            Quaternion newAngle = Quaternion.Lerp(startAngle, targetAngle, easedtime);

            spikeAxis.rotation = newAngle;
            yield return null;
        }


        time = 0f;

        //? 다시 위로 들어올리기
        spikeAxis.transform.localScale = targetScale;

        while (time <= 1f)
        {
            time += Time.deltaTime * rotateSpeed / 15f;
            float easedtime = EasingFunctions.easeInOutCirc(time, 5); // easeInOutCirc

            Quaternion newAngle = Quaternion.Lerp(targetAngle, startAngle, easedtime);

            spikeAxis.rotation = newAngle;
            yield return null;
        }


        time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime * rotateSpeed / 12f;
            float easedtime = EasingFunctions.easeOutCubic(time, 5); // easeInCubic

            spikeAxis.transform.localScale = Vector3.Lerp(targetScale, startScale, easedtime);
            spikeAxis.rotation = startAngle;

            yield return null;
        }

        //? 종료

        spikeAxis.rotation = Quaternion.Euler(0f, 0f, 0f);
        spikeAxis.transform.localScale = startScale;
        spikeAxis.transform.SetParent(controller.transform);

        StartCoroutine(ResetPos());
        spikeAxis.gameObject.SetActive(false);
    }

}

//////////////////////////////////////////////////////////////
#endregion

