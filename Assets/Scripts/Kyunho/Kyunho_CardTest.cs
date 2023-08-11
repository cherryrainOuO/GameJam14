using UnityEngine;

public class Kyunho_CardTest : MonoBehaviour
{
    [SerializeField] private Kyunho_Card[] cards;

    private void Awake()
    {
        foreach (var card in cards)
        {
            Debug.Log(card.Name);
        }
    }
}