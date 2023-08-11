using UnityEngine;

[System.Serializable]
public class Kyunho_CardEvent : Kyunho_ICardEvent
{
    [SerializeField] private string description;
    [SerializeField] private Kyunho_Reward[] reward;

    public string Description { get => description; }
    public Kyunho_Reward[] Reward { get => reward; }
}