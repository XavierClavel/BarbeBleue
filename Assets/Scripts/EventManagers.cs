using System;
using System.Collections.Generic;

public static class EventManagers
{
    public static readonly EventManager<IInputListener> inputs = new EventManager<IInputListener>();
    public static readonly EventManager<ILocalized> localization = new EventManager<ILocalized>();
    public static readonly EventManager<IPersistentData> persistent = new EventManager<IPersistentData>();
    
    private static Dictionary<Type, object> dict;

    public static EventManager<T> getEventManager<T>()
    {
        return dict.getorPut(typeof(T), new EventManager<T>()) as EventManager<T>;
    }

    public static void registerListener<T>(T value)
    {
        getEventManager<T>().registerListener(value);
    }
    
    public static void unregisterListener<T>(T value)
    {
        getEventManager<T>().unregisterListener(value);
    }
    
    
}