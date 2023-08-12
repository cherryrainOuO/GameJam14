public class Kyunho_CardResolver
{
    private Kyunho_CardEventController cardEventController;
    public Kyunho_CardResolver(Kyunho_CardEventController cardEventController)
    {
        this.cardEventController = cardEventController;
    }

    public void UseCard(Kyunho_ICard card)
    {
        cardEventController.AddEvents(card.UsingEvent);
        cardEventController.Execute();
    }

    public void RemoveCard(Kyunho_ICard card)
    {
        cardEventController.AddEvents(card.RemovalEvent);
        cardEventController.Execute();
    }
}