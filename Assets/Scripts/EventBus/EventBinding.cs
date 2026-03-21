using System;

public interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    private Action<T> onEvent = _ => { }; // store empty delagates so dont need null checks
    private Action onEventNoArgs = () => { };

    Action<T> IEventBinding<T>.OnEvent // explicit interface implementation
    {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBinding<T>.OnEventNoArgs
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    // Constructors
    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
    public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

    // Add or remove to delegate if wanted
    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

    public void Add(Action<T> onEvent) => onEvent += onEvent;
    public void Remove(Action<T> onEvent) => onEvent -= onEvent;


}