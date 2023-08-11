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

    private void AddEvent(Kyunho_CardEvent cardEvent)
    {
        events.Enqueue(cardEvent);
    }

    private void Execute()
    {
        var cardEvent = events.Dequeue();

        //Need implement dialogue with cardEvent.Description;
    }
}