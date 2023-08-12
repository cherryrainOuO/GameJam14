using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller
/// </summary>
public class Kyunho_CardEventController : MonoBehaviour
{
    // private Dialogue dialogue;
    private Queue<Kyunho_CardEvent> events;

    private void Awake()
    {
        events = new Queue<Kyunho_CardEvent>();
    }

    public void AddEvents(Kyunho_CardEvent[] cardEvents)
    {
        foreach (var cardEvent in cardEvents)
        {
            events.Enqueue(cardEvent);
        }
    }

    public void Execute()
    {
        while (events.TryDequeue(out var cardEvent))
        {
            Debug.Log(cardEvent.Description);
            var resolver = new Kyunho_RewardResolver(Eun_PlayerStat.Instance);
            resolver.Resolve(cardEvent.Reward);
        }
    }
}