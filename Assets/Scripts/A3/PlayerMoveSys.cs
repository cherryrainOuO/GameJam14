using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerCollision), typeof(PlayerInput))]
public class PlayerMoveSys : MonoBehaviour
{
    #region Start / Update
    //////////////////////////////////////////////////////////////

    private PlayerCollision controller;
    private PlayerInput playerInput;

    private void Start()
    {
        controller = GetComponent<PlayerCollision>();
        playerInput = GetComponent<PlayerInput>();



        // 플레이어인풋의 이벤트에 플레이어 클래스의 메소드를 등록
        playerInput.SetDirectionalInputEvent += SetDirectionalInput;

        playerInput.OnJumpInputDownEvent += OnJumpInputDown;
        playerInput.OnJumpInputUpEvent += OnJumpInputUp;
        playerInput.OnShiftInputDownEvent += OnShiftInputDown;
        playerInput.OnShiftInputUpEvent += OnShiftInputUp;

    }
    private void Update()
    {
        if (!GameManager3.Instance.isStop && GameManager3.Instance.isPlayerMove)
        {
            CalculateMove();
            CoyoteTime();
            JumpCountCheck();
        }


    }

    //////////////////////////////////////////////////////////////
    #endregion


    #region 이동
    //////////////////////////////////////////////////////////////

    [Header("이동")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField][Range(0, 0.1f)] private float accelerationGround = .03f; // 평지 가속도

    private Vector2 velocity;
    private float velocityXSmoothing; // ref 를 사용해서 Mathf.SmoothDamp 에서 자동으로 변환되는 변수 (지우지마)

    private Vector2 directionalInput;

    /// <summary> 이동 계산 </summary>
    private void CalculateMove()
    {
        float targetVelocityX = directionalInput.x * (isShift ? shiftSpeed : moveSpeed);

        velocity.x =
                     Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                        (controller.collisions.below) ? accelerationGround : accelerationAirborne);
        // timeToWallUnstick : 벽에서 반대방향으로 멀리 뛰기 할때 허용하는 코요테 타임

        Vector2 accel = new Vector2(0, gravity);
        Vector2 verletMove = (controller.collisions.standingOnPlatform == 1) ? velocity * Time.deltaTime :
                                            (velocity + accel * Time.deltaTime * .5f) * Time.deltaTime;

        controller.Move(verletMove, directionalInput);

        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime; // 경사진곳에선 빠르게 확 내려감
            else
                velocity.y = 0f; // 순간적으로 y 속도 확 느려짐

        }
        else if (controller.collisions.above) velocity.y = -2f; // 천장에 머리 콩 찍기 

        velocity.y += gravity * Time.deltaTime;

    }

    /// <summary> 이동키 입력 이벤트 </summary>
    private void SetDirectionalInput(Vector2 input) => directionalInput = input;

    //////////////////////////////////////////////////////////////
    #endregion

    [Space(20)]

    #region 점프
    //////////////////////////////////////////////////////////////

    [Header("점프")]
    //[SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float minJumpHeight = .2f;
    [SerializeField][Range(0, 1)] private float timeToJumpApex = .4f; // 체공시간? 늘어날수록 중력이 약해짐, 물속 구현할때 사용할만 한듯
    [SerializeField][Range(0, 0.5f)] private float accelerationAirborne = .2f; // 공중 가속도


    private float gravity => -(2f * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    private float maxJumpVelocity => Mathf.Abs(gravity) * timeToJumpApex;
    private float maxDoubleJumpVelocity => maxJumpVelocity / 1.5f;
    private float minJumpVelocity => Mathf.Sqrt(2f * Mathf.Abs(gravity) * minJumpHeight);

    private int currentJumpCount = 0;

    [Space]

    [SerializeField][Range(0, 0.1f)] private float coyoteTime = .07f;
    [SerializeField][Range(0, 0.1f)] private float jumpBufferTime = .1f;

    private float coyoteTimeCounter; // controller.collisions.below 대신 이걸 사용할 것
    private float jumpBufferCounter;

    private Coroutine runningCoroutine = null; // 점프 버퍼 타임용 코루틴


    /// <summary> 점프 카운트 체크 </summary>
    private void JumpCountCheck()
    {
        if (coyoteTimeCounter > 0f) currentJumpCount = 0; // 땅에 있을때(코요테 타임 적용) or 벽점프중
        else if (currentJumpCount == 0)
            currentJumpCount = (GameManager3.Instance.playerJumpCount == 1)
                                ? GameManager3.Instance.playerJumpCount
                                : GameManager3.Instance.playerJumpCount - 1; // 땅에 있다가 공중으로 넘어갈때
    }

    /// <summary> 코요테 타임 </summary>
    private void CoyoteTime()
    {
        if (controller.collisions.below) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        //print(coyoteTimeCounter);
    }

    /// <summary> 버퍼 타임 </summary>
    private IEnumerator CoroutineForJumpBuffering()
    {
        Vector3 pos = transform.position;

        while (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;

            if (jumpBufferCounter > 0f)
            { // 점프 버퍼가 존재할 때


                if (coyoteTimeCounter > 0f)
                { // 땅에 있을 때 (코요테 타임 적용)
                    //print(jumpBufferCounter);
                    //Debug.DrawRay(pos, Vector3.right, Color.green, 10f); 

                    if (controller.collisions.slidingDownMaxSlope)
                    {
                        if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                        { // not jumping against max slope
                            velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                            velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                        }
                    }
                    else
                    {
                        velocity.y = isShift ? minJumpVelocity : maxJumpVelocity; // 가변점프 최대
                    }

                    currentJumpCount++;
                    jumpBufferCounter = 0f;
                }
            }

            yield return null;
        }
    }

    /// <summary> 점프시에 일어나는 이벤트 </summary>
    private void OnJumpInputDown()
    {
        if (runningCoroutine != null) StopCoroutine(runningCoroutine);

        if (currentJumpCount < GameManager3.Instance.playerJumpCount)
        { // 점프카운트 제한

            controller.collisions.isBreakAble = 0; // 벽 부수기 불가능


            if (coyoteTimeCounter > 0f)
            { // 땅에 있을 때 (코요테 타임 적용)
                if (controller.collisions.slidingDownMaxSlope)
                {
                    if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                    { // not jumping against max slope
                        velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                        velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                    }
                }
                else
                {
                    velocity.y = isShift ? minJumpVelocity : maxJumpVelocity; // 가변점프 최대
                }

            }
            else
            { // 공중에 있을 때
                velocity.y = isShift ? minJumpVelocity : maxDoubleJumpVelocity; // 2단점프
            }

            currentJumpCount++;

        }
        else // 점프 버퍼는 점프 카운트를 모두 소비한 상태에만 발동됨. --> currentJumpCount >= maxJumpCount
        {
            jumpBufferCounter = jumpBufferTime;
            runningCoroutine = StartCoroutine(CoroutineForJumpBuffering());
        }

        //print(currentJumpCount);


        isShift = false;
    }

    /// <summary> 점프 버튼 뗐을때 일어나는 이벤트 </summary>
    private void OnJumpInputUp()
    {

        if (runningCoroutine != null) StopCoroutine(runningCoroutine);

        if (velocity.y > minJumpVelocity) velocity.y = minJumpVelocity; // 가변점프 최소


        coyoteTimeCounter = 0f;
    }

    //////////////////////////////////////////////////////////////
    #endregion

    [Space(20)]

    #region 쉬프트(미세) 이동
    //////////////////////////////////////////////////////////////

    [Header("쉬프트 이동")]
    [SerializeField] private float shiftSpeed = 2f;

    private bool isShift;

    private void OnShiftInputDown()
    {
        print("쉬프트시작");

        isShift = true;
    }
    private void OnShiftInputUp()
    {
        print("쉬프트끝");

        isShift = false;
    }

    //////////////////////////////////////////////////////////////
    #endregion

    #region 충돌
    [SerializeField] private PlayerSystem playerSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            Debug.Log("아야!!!");
            StartCoroutine(CoroutineForDefeat());
        }


    }

    private IEnumerator CoroutineForDefeat()
    {
        playerSystem.Defeat(1);

        yield return new WaitForSeconds(1f);
    }
    #endregion
}

