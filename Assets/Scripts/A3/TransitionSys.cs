using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSys : MonoBehaviour
{
    [SerializeField] private GameObject[] texts;
    private bool isUse = false;

    private void Start()
    {
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
        float time = 0f;

        while (time <= 1f)
        {
            time += Time.deltaTime / .5f;

            transform.localPosition = Vector2.Lerp(Vector2.zero, Vector2.up * 1000f, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }
    }

    public void Transiton(int _index)
    {
        if (!isUse)
        {
            StartCoroutine(CoroutineForTransition(_index));
            isUse = true;
        }
    }
    private IEnumerator CoroutineForTransition(int _index)
    {
        float time = 0f;

        texts[_index].SetActive(true);
        while (time <= 1f)
        {
            time += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(Vector2.up * 1000f, Vector2.zero, EasingFunctions.easeInCubic(time, 5));
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        GameManager3.Instance.ResetScene();
    }
}
