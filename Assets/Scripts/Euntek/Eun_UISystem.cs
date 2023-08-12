using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Eun_UISystem : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Eun_CardSystem cardSystem;

    [SerializeField] private TextMeshProUGUI text;

    private int currentIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }


    private IEnumerator Animation()
    {
        float time = 0f;

        for (int i = 0; i < 9; i++)
        {
            background.sprite = sprites[i];

            yield return YieldFunctions.WaitForSeconds(.7f);
        }
    }

    private void Update()
    {

    }
}
