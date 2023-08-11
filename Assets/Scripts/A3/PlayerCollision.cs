using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollision : RaycastController
{
    #region Start
    //////////////////////////////////////////////////////////////

    public CollisionInfo collisions;
    [HideInInspector] public Vector2 playerInput; // 이거 처리를 PlayerInput 클래스의 이벤트로 바꾸고 싶음. 

    public override void Start() {
        base.Start();
        collisions.faceDir = 1;
        collisions.isBreakAble = 0;
    }

    //////////////////////////////////////////////////////////////
    #endregion


    #region 이동
    //////////////////////////////////////////////////////////////

    
    public void Move(Vector2 moveAmount, Vector2 input){
        UpdateRaycastOrigins();
        
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        ////
        // moveAmout 조정 (충돌 계산)
        if(moveAmount.y <= 0) DescendSlope(ref moveAmount);
        
        if (moveAmount.x != 0) collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
              
        HorizontalCollisions(ref moveAmount);
        if(moveAmount.y != 0) VerticalCollisions(ref moveAmount);
        ////


        transform.Translate(moveAmount); // 조정된 moveAmout 만큼 이동

    }

    //////////////////////////////////////////////////////////////
    #endregion


    #region 충돌 판단
    //////////////////////////////////////////////////////////////

    //** 수평 충돌 판단 **//
    private void HorizontalCollisions(ref Vector2 moveAmount){ // Vector2 는 구조체(call by value)라서 ref 를 사용하여 call by reference 해버리기.
        float directionX = collisions.faceDir;

        float rayLength = (Mathf.Abs(moveAmount.x) < SKIN_WIDTH) ? 2f * SKIN_WIDTH : Mathf.Abs(moveAmount.x) + SKIN_WIDTH;


        for (int i = 0; i < horizontalRayCount; i++)
        {
            
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask); // 레이 생성

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) // 충돌시
            {

                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.green);

                if(hit.distance == 0){ // 벽과 벽사이에 깔렸을 때 : 압사될 때
                    if (hit.collider.tag != "Through"){

                        print("깔림");
                        continue;
                    }

                    
                }
                

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); // 경사면인지 벽인지 판단
                
                if(i==0 && slopeAngle <= maxSlopeAngle){ // 경사면에 닿았을 때

                    if(collisions.descendingSlope){ // 내려가다가 올라가는 경사면을 만났을 때
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distanceToSlopeStart = 0f;

                    if(slopeAngle != collisions.slopeAngleOld){ // 경사면 오를때 빈틈생기는 오차조정
                        distanceToSlopeStart = hit.distance - SKIN_WIDTH;
                        moveAmount.x -= distanceToSlopeStart * directionX; 
                    }

                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxSlopeAngle){ // 벽에 닿았을 때 or 오를 수 없는 경사면에 닿았을 때

                    moveAmount.x = Mathf.Min(Mathf.Abs(moveAmount.x), (hit.distance - SKIN_WIDTH)) * directionX; // 해당프레임에서 충돌이 일어나지 않았을 시에 대비
                    rayLength = Mathf.Min(Mathf.Abs(moveAmount.x) + SKIN_WIDTH, hit.distance); // 안해도 되는데 hit 건너뛰어서 연산량 줄이려고 하는듯? 

                    if(collisions.climbingSlope) // 경사면 올라가다가 급격한 경사면을 만났을 때
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    

                    collisions.left = (directionX == -1); // left 벽
                    collisions.right = (directionX == 1); // right 벽

                    
                }

                collisions.isBreakAble = 0; // 벽 못부숴
            }
        }
        
    }

    //** 플랫폼 하강 통과 리셋 **//
    private void ResetFallingThroughPlatform() => collisions.fallingThroughPlatform = false;

    //** 수직 충돌 판단 **//
    private void VerticalCollisions(ref Vector2 moveAmount){ // Vector2 는 구조체(call by value)라서 ref 를 사용하여 call by reference 해버리기.
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + SKIN_WIDTH;

        for (int i = 0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask); // 레이 생성

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit){ // 충돌시
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

                if(hit.collider.tag == "Through"){ // 밑에서 위로 통과가능한 플랫폼
                    
                    if(directionY == 1 || hit.distance == 0 || collisions.fallingThroughPlatform)
                        continue;

                    if(playerInput.y == -1 && collisions.below){ // 플랫폼 밑으로 내려가기
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .2f); // .2초간 플랫폼 밑으로 통과중
                        continue;
                    }
                }
                else if(hit.collider.tag == "Break" && collisions.isBreakAble == 1f && directionY == -1){
                    print("break!");
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Default"); // 벽 부수기
                    continue;
                }

                moveAmount.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope) // 경사면 오르고 있을 때
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);

                collisions.below = (directionY == -1); // below 바닥
                collisions.above = (directionY == 1); // above 천장

                collisions.isBreakAble = collisions.above ? 1 : 0; // 천장에 닿았을때 벽 부수기 가능
            }
        }

        if(collisions.climbingSlope){ // 경사면 오르고 있을 때

            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit){
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(slopeAngle != collisions.slopeAngle){

                    moveAmount.x = Mathf.Min(Mathf.Abs(moveAmount.x), (hit.distance - SKIN_WIDTH)) * directionX;

                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }

    }


    //////////////////////////////////////////////////////////////
    #endregion


    #region 경사면
    //////////////////////////////////////////////////////////////

    [SerializeField] private float maxSlopeAngle = 80f; // 오를수 있는 경사면 최대각도

    //** 경사면 오르기 **//
    private void ClimbSlope(ref Vector2 _moveAmount, float _slopeAngle, Vector2 _slopeNormal){
        float moveDistance = Mathf.Abs(_moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(_slopeAngle * Mathf.Deg2Rad) * moveDistance;
        
        if(_moveAmount.y <= climbmoveAmountY){
            ////
            if(_moveAmount.x != 0)
                _moveAmount.y = climbmoveAmountY;
            ////

            _moveAmount.x = Mathf.Cos(_slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = _slopeAngle;
            collisions.slopeNormal = _slopeNormal;
        }
        
    }

    //** 경사면 내려가기 **//
    private void DescendSlope(ref Vector2 _moveAmount){

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(_moveAmount.y) + SKIN_WIDTH, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(_moveAmount.y) + SKIN_WIDTH, collisionMask);
        
        if(maxSlopeHitLeft ^ maxSlopeHitRight){
            SlideDownMaxSlope(maxSlopeHitLeft, ref _moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref _moveAmount);
        }
        

        if(!collisions.slidingDownMaxSlope){
            float directionX = Mathf.Sign(_moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && 
                    slopeAngle <= maxSlopeAngle && 
                    Mathf.Sign(hit.normal.x) == directionX && 
                    hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(_moveAmount.x)) 
                {

                    float moveDistance = Mathf.Abs(_moveAmount.x);
                    float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                    _moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(_moveAmount.x);
                    _moveAmount.y -= descendmoveAmountY;

                    collisions.slopeAngle = slopeAngle;
                    collisions.descendingSlope = true;
                    collisions.below = true;
                    collisions.slopeNormal = hit.normal;  
                }
            }
        }
        
    }

    private void SlideDownMaxSlope(RaycastHit2D _hit, ref Vector2 _moveAmount){
        if(_hit){
            float _slopeAngle = Vector2.Angle(_hit.normal, Vector2.up);
            if(_slopeAngle > maxSlopeAngle){
                _moveAmount.x = _hit.normal.x * (Mathf.Abs(_moveAmount.y) - _hit.distance) / Mathf.Tan(_slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = _slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = _hit.normal;
            }
        }
    }

    //////////////////////////////////////////////////////////////
    #endregion


    public struct CollisionInfo{
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public int isBreakAble;

        public int standingOnPlatform;

        public void Reset(){
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }
    }
}
