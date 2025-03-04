using Barebone.Contracts;
using System.Collections.Generic;

namespace Barebone.Managers.Logic;

public class EventQueue
{
    private readonly Queue<IEvent> _eventQueue = new();
    private bool _isProcessing = false;

    public void Enqueue(IEvent gameEvent)
    {
        _eventQueue.Enqueue(gameEvent);
        // ProcessQueue(); // Ensure processing starts when an event is added
    }

    public async void ProcessQueue()
    {
        if (_isProcessing) return;

        _isProcessing = true;
        while (_eventQueue.Count > 0)
        {
            var gameEvent = _eventQueue.Dequeue();
            await gameEvent.Execute();
        }
        _isProcessing = false;
    }
}