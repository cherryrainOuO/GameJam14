using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider2D))]
public class Laser : MonoBehaviour
{
    private string ID => transform.parent.name + gameObject.name;

    [HideInInspector] public int activate = 0;

    [HideInInspector] public Vector2 startPos;
    [HideInInspector] public Vector2 targetPos;

    [SerializeField] private float laserSize = .5f;
    [SerializeField] private float laserAlphaFullSpeed = 2f;
    [SerializeField] private float laserAlphaZeroSpeed = .2f;

    private int automaticSwitch = 0;

    private LineRenderer lineRenderer;
    private BoxCollider2D lineCollider;

    private Color laserAlphaZero;
    private Color laserReadyAlphaHalf;
    private Color laserAlphaFull;

    private float time = 0f;

    [Header("머티리얼")]
    [SerializeField] private Material laserMat;
    private Material originMat;


    private void Start()
    {
        ColorUtility.TryParseHtmlString("#FFFFFF00", out laserAlphaZero);
        ColorUtility.TryParseHtmlString("#DE312F80", out laserReadyAlphaHalf);
        ColorUtility.TryParseHtmlString("#FFFFFFFF", out laserAlphaFull);

        lineRenderer = GetComponent<LineRenderer>();
        lineCollider = GetComponent<BoxCollider2D>();

        lineRenderer.startWidth = 0f;
        lineRenderer.endWidth = 0f;
        lineRenderer.startColor = laserAlphaZero;
        lineRenderer.endColor = laserAlphaZero;

        originMat = lineRenderer.material;

    }

    void Update()
    {
        if (!GameManager3.Instance.isStop)
        {
            switch (activate)
            {

                case 1:
                    LaserOn();
                    break;

                case -1:
                    LaserOff();
                    break;

                case 2:
                    AutomaticLaser();
                    break;

            }
        }
    }

    private void LaserOn()
    {
        lineRenderer.SetPosition(0, startPos - (Vector2)transform.position);
        lineRenderer.SetPosition(1, targetPos - (Vector2)transform.position);

        lineRenderer.startColor = laserReadyAlphaHalf;
        lineRenderer.endColor = laserReadyAlphaHalf;

        lineCollider.offset = (targetPos - startPos) / 2f;
        lineCollider.size = new Vector2(Mathf.Abs(targetPos.x - startPos.x), Mathf.Abs(targetPos.y - startPos.y));
        lineCollider.size += (lineCollider.size.x < .1f) ? Vector2.right * laserSize : Vector2.up * laserSize; //! float형 비교 주의!

        if (time < 1f)
        {
            time += Time.deltaTime / laserAlphaFullSpeed;
            lineRenderer.startWidth = EasingFunctions.easeOutCubic(time, 3) * (laserSize - .1f);
            lineRenderer.endWidth = EasingFunctions.easeOutCubic(time, 3) * (laserSize - .1f);
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Spike");

            lineRenderer.startWidth = laserSize;
            lineRenderer.endWidth = laserSize;
            lineRenderer.startColor = laserAlphaFull;
            lineRenderer.endColor = laserAlphaFull;
            activate = 0;
        }
    }

    private void LaserOff()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime / laserAlphaZeroSpeed;
            lineRenderer.startWidth = EasingFunctions.easeInCubic(time, 3) * (laserSize - .1f);
            lineRenderer.endWidth = EasingFunctions.easeInCubic(time, 3) * (laserSize - .1f);

            if (time < 0.5f) this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            lineRenderer.startWidth = 0f;
            lineRenderer.endWidth = 0f;
            lineRenderer.startColor = laserAlphaZero;
            lineRenderer.endColor = laserAlphaZero;

            lineCollider.offset = Vector2.zero;
            lineCollider.size = Vector2.one; // 콜라이더 정리

            activate = 0;
        }
    }

    private void AutomaticLaser()
    {
        if (automaticSwitch == 0)
        {
            lineRenderer.SetPosition(0, startPos - (Vector2)transform.position);
            lineRenderer.SetPosition(1, targetPos - (Vector2)transform.position);
            //! 라인렌더러 월드공간 사용 X 

            lineRenderer.startColor = laserReadyAlphaHalf;
            lineRenderer.endColor = laserReadyAlphaHalf;

            lineCollider.offset = (targetPos - startPos) / 2f;
            lineCollider.size = new Vector2(Mathf.Abs(targetPos.x - startPos.x), Mathf.Abs(targetPos.y - startPos.y));
            lineCollider.size += (lineCollider.size.x < .1f) ? Vector2.right * laserSize : Vector2.up * laserSize; //! float형 비교 주의!

            if (time < 1f)
            {
                time += Time.deltaTime / laserAlphaFullSpeed;
                lineRenderer.startWidth = EasingFunctions.easeOutCubic(time, 3) * (laserSize - .1f);
                lineRenderer.endWidth = EasingFunctions.easeOutCubic(time, 3) * (laserSize - .1f);
            }
            else
            {
                this.gameObject.layer = LayerMask.NameToLayer("Spike");

                lineRenderer.startWidth = laserSize;
                lineRenderer.endWidth = laserSize;
                lineRenderer.startColor = laserAlphaFull;
                lineRenderer.endColor = laserAlphaFull;

                lineRenderer.material = laserMat;

                automaticSwitch = 1;
            }
        }
        else if (automaticSwitch == 1)
        {
            if (time > 0f)
            {
                time -= Time.deltaTime / laserAlphaZeroSpeed;
                lineRenderer.startWidth = EasingFunctions.easeInCubic(time, 3) * (laserSize - .1f);
                lineRenderer.endWidth = EasingFunctions.easeInCubic(time, 3) * (laserSize - .1f);

                if (time < 0.5f) this.gameObject.layer = LayerMask.NameToLayer("Default"); // 오차 살짝 허용
            }
            else
            {
                lineRenderer.startWidth = 0f;
                lineRenderer.endWidth = 0f;
                lineRenderer.startColor = laserAlphaZero;
                lineRenderer.endColor = laserAlphaZero;

                lineCollider.offset = Vector2.zero;
                lineCollider.size = Vector2.one; // 콜라이더 정리

                lineRenderer.material = originMat;

                activate = 0;



                automaticSwitch = 0;
            }
        }
    }

}
