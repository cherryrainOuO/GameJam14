using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;
    public bool isUse = false;

    public void ChangeText(string _text1, string _text2)
    {
        texts[0].text = _text1;
        texts[1].text = _text2;
    }
}
