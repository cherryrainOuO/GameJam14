using UnityEngine;

public enum Kyunho_CardType { Beneficence, Malefeasance }

public interface Kyunho_ICard
{
    string Name { get; }
    Sprite Sprite { get; }
    int Morality { get; }
    int Cost { get; }
    Kyunho_CardType Type { get; }

    Kyunho_CardEvent[] UsingEvent { get; }
    Kyunho_CardEvent[] RemovalEvent { get; }
}