using UnityEngine;

/// <summary>
/// @Kyunho
/// 카드 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Object/Card", order = int.MinValue)]
public class Kyunho_Card : ScriptableObject, Kyunho_ICard
{
    [SerializeField] private string cardName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int morality;
    [SerializeField] private int cost;
    [SerializeField] private Kyunho_CardType cardType;
    [SerializeField] private Kyunho_CardEvent[] usingEvent;
    [SerializeField] private Kyunho_CardEvent[] removalEvent;

    public string Name { get => cardName; }
    public Sprite Sprite { get => sprite; }
    public int Morality { get => morality; }
    public int Cost { get => cost; }
    public Kyunho_CardType Type { get => cardType; }
    public Kyunho_CardEvent[] UsingEvent { get => usingEvent; }
    public Kyunho_CardEvent[] RemovalEvent { get => removalEvent; }
}