using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class EventManager : MonoBehaviour
{
    public float UpdateEvent;
    [Header("<size=10>write in secound<size>")]
    [Range(0, 100)]
    public int EventChance;

    public List<Event> Events = new List<Event>();

    [System.Serializable]
    public class Event
    {
        public float EventDuration;
        public UnityEvent StartEvent;
        public UnityEvent EndEvent;
    }
    private bool isEventTimer;
    private float EventTimer;
    private Event VariantEvent;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (UpdateEvent >= time && !isEventTimer)
        {
            if (EventChance == Random.RandomRange(0, 100))
            {
                VariantEvent = Events[Random.RandomRange(0, Events.Count - 1)];
                VariantEvent.StartEvent.Invoke();
                isEventTimer = true;
            }
            time = 0f;
        }
        else if (EventTimer >= time && isEventTimer)
        {
            VariantEvent.EndEvent.Invoke();
            isEventTimer = false;
        }
    }
}
