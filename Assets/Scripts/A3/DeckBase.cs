using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class DeckBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public int index;
    protected DeckSystem deckSystem;
    private Image deckImage;

    private Vector2 originPos;

    [SerializeField] private InfoSystem infoSystem;
    protected string deckName;
    protected string deckInfo;

    [SerializeField] protected PlayerSystem playerSystem;
    [SerializeField] protected int power = 1;

    [SerializeField] private Material glowMat;
    private Material originMat;

    protected virtual void Start()
    {
        deckSystem = GetComponentInParent<DeckSystem>();
        deckImage = GetComponent<Image>();

        originPos = transform.position;
        originMat = deckImage.material;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager3.Instance.isPlayerTurn && !GameManager3.Instance.isStop)
        {
            if (!GameManager3.Instance.isDrag)
            {
                deckImage.color = Color.cyan;

                if (!infoSystem.isUse)
                {
                    infoSystem.gameObject.SetActive(true);
                    infoSystem.ChangeText(deckName, deckInfo);
                    infoSystem.transform.position = (Vector2)transform.position + Vector2.right * 1.5f + Vector2.down * 1.5f;
                }
            }

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager3.Instance.isPlayerTurn && !GameManager3.Instance.isStop)
        {
            if (!GameManager3.Instance.isDrag)
            {
                deckImage.color = Color.white;
                infoSystem.gameObject.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager3.Instance.isPlayerTurn && !GameManager3.Instance.isStop)
        {
            if (!GameManager3.Instance.isDrag)
            {
                GameManager3.Instance.isDrag = true;
                deckImage.color = Color.red;
                infoSystem.gameObject.SetActive(false);
                infoSystem.isUse = true;

                deckImage.material = glowMat;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager3.Instance.isPlayerTurn && !GameManager3.Instance.isStop)
        {
            deckImage.color = Color.white;
            infoSystem.isUse = false;
            deckImage.material = originMat;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back); // 레이캐스트 충돌판별(input 콜라이더 필요)


            if (hit == true && hit.collider.tag == "AttackSpace")
            {

                Debug.Log("어택 닿았지롱");

                Attack();
            }
            else if (hit == true && hit.collider.tag == "RemoveSpace")
            {
                Debug.Log("리무브 닿았지롱");

                Remove();
            }
            else
            {
                StartCoroutine(CoroutineForBackToPosition());
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager3.Instance.isPlayerTurn && !GameManager3.Instance.isStop)
        {
            Vector2 objPosition = MouseWorldPosition.GetMouseWorldPostion();
            objPosition = new Vector2(Mathf.Clamp(objPosition.x, -7, 7),
                                        Mathf.Clamp(objPosition.y, -4, 4));

            transform.position = objPosition;
        }
    }

    protected virtual void Attack()
    {
        this.gameObject.SetActive(false);
        transform.position = originPos;

        GameManager3.Instance.removeCount--;

        if (GameManager3.Instance.removeCount == 0)
        {
            GameManager3.Instance.removeCount = 2;
            GameManager3.Instance.isPlayerTurn = false;
            GameManager3.Instance.isPlayerMove = true;
        }

        GameManager3.Instance.isDrag = false;

        deckSystem.RemoveDeck(index);
    }
    private void Remove()
    {


        this.gameObject.SetActive(false);
        transform.position = originPos;

        GameManager3.Instance.removeCount--;

        if (GameManager3.Instance.removeCount == 0)
        {
            GameManager3.Instance.removeCount = 2;
            GameManager3.Instance.isPlayerTurn = false;
            GameManager3.Instance.isPlayerMove = true;
        }

        GameManager3.Instance.isDrag = false;

        deckSystem.RemoveDeck(index);
    }

    private IEnumerator CoroutineForBackToPosition()
    {
        Vector2 startPos = transform.position;

        float time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime / .5f;

            transform.position = Vector2.Lerp(startPos, originPos, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }

        transform.position = originPos;

        GameManager3.Instance.isDrag = false;
    }

}
