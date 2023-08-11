using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    protected const float SKIN_WIDTH = .015f; // 오차 조정, 땅속으로 가라앉는거 방지, 경사면에서 빈틈생기는거 방지 등등..
    private const float DST_BETWEEN_RAYS = .25f;

    [SerializeField] protected LayerMask collisionMask;

    protected int horizontalRayCount = 4;
    protected int verticalRayCount = 4;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    [HideInInspector] public BoxCollider2D myCollider;

    protected RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }
    public virtual void Start() {
        CalculateRaySpacing();
    }

    /** 레이캐스팅 위치 초기화(업데이트) **/
    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(SKIN_WIDTH * -2f); // 바운드(콜라이더 넓이) 가져오기

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y); // 레이 위치 설정
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /** 레이캐스팅 설정 **/
    protected void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(SKIN_WIDTH * -2f); // 바운드(콜라이더 넓이) 가져오기

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / DST_BETWEEN_RAYS);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / DST_BETWEEN_RAYS);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue); // 레이 개수 설정
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1); // 레이 위치 설정 (가운데로 정렬하기)
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
