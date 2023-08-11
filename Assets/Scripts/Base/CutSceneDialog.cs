using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class CutSceneDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialog;
    [SerializeField] private string[] dialogStr;

    private int currentDialogIndex = -1;

    private float typingSpeed = 0.1f;
    private bool isTypingEffect = false;

    private Coroutine runningCoroutine = null;

    [SerializeField] private Image skipImage;


    private void Start()
    {

        StartCoroutine(UpdateDialog());
        StartCoroutine(CoroutineForSkip());
    }

    private IEnumerator UpdateDialog()
    { // 외부 스크립트에서 IEnumerator로 접근할 것

        yield return YieldFunctions.WaitForSeconds(1f);
        SetNextDialog();


        while (true)
        {
            if (Input.GetMouseButtonDown(0)) //? 넘어가는 버튼
            {

                if (isTypingEffect)
                {
                    /* 타이핑 효과 넘어가기 */
                    isTypingEffect = false;

                    if (runningCoroutine != null)
                        StopCoroutine(runningCoroutine); // 타이핑 중이면 강제로 정지

                    dialog.text = dialogStr[currentDialogIndex];

                }
                else if (currentDialogIndex < dialogStr.Length - 1)
                {
                    /* 다음 다이얼로그 재생 */

                    yield return StartCoroutine(ReverseEraser());

                    SetNextDialog();
                }
                else
                {
                    yield return StartCoroutine(ReverseEraser());

                    dialog.gameObject.SetActive(false);

                    break;
                }
            }

            yield return null;
        }


        Debug.Log("다이얼로그 끝");
        //Todo LevelManager.Instance.LoadSelectScene();
    }

    private void SetNextDialog()
    {
        currentDialogIndex++; // 다음 화자로

        // 타이핑 효과 재생
        runningCoroutine = StartCoroutine(OnTypingText());

    }

    private IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        while (index - 1 < dialogStr[currentDialogIndex].Length)
        { // 속도에 맞춰서 한글자씩 타이핑 출력
            dialog.text = dialogStr[currentDialogIndex].Substring(0, index);
            index++;

            yield return YieldFunctions.WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;
    }

    private IEnumerator ReverseEraser()
    {
        isTypingEffect = true;

        int index = dialogStr[currentDialogIndex].Length - 1;

        while (index >= 0)
        { // 속도에 맞춰서 한글자씩 삭제
            dialog.text = dialogStr[currentDialogIndex].Substring(0, index);
            index--;

            yield return YieldFunctions.WaitForSeconds(typingSpeed / 20f);
        }

        isTypingEffect = false;

    }

    private IEnumerator CoroutineForSkip() //ToDO 처음부터 실행해놓자.
    {
        float time = 0f;

        while (time <= 1.5f)
        {
            if (Input.GetMouseButton(0))
            {
                time += Time.deltaTime;
                skipImage.color = new Color(skipImage.color.r, skipImage.color.g, skipImage.color.b, time / 1.5f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                skipImage.color = new Color(skipImage.color.r, skipImage.color.g, skipImage.color.b, 0f);
                time = 0f;
            }

            yield return null;
        }

        skipImage.color = new Color(skipImage.color.r, skipImage.color.g, skipImage.color.b, 1f);

        //Todo LevelManager.Instance.LoadSelectScene();

    }

}
