using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
	#region Singleton Instance Variables
	/// <summary>
	/// The instance.
	/// </summary>
	private static EventManager instance = null;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static EventManager Instance
	{
		get
		{
			if (instance == null)
				instance = new EventManager();

			return instance;
		}
	}
	#endregion

	/// <summary>
	/// Awake this instance. For main initialation.
	/// </summary>
	private void Awake()
	{
		// Check if instance is set
		if(instance)
		{
			// destroy duplicate
			DestroyImmediate (gameObject);
			return;
		}

		// Mark instance and flag not to be destroyed.
		instance = this;
		DontDestroyOnLoad (gameObject);
	}

	// ------------------------------------------------------------------------------------------------------------------

	[SerializeField]
	/// <summary>
	/// Dictionary containing the event listeners.
	/// </summary>
	private Dictionary<string, List<ListenersData>> ListenerComponents = new Dictionary<string, List<ListenersData>>();

	/// <summary>
	/// Adds the listener.
	/// </summary>
	/// <param name="Sender">Monobehaiviour of listener.</param>
	/// <param name="eventAction">Method to invoke.</param>
	/// <param name="_eventListenerName">Event listener name.</param>
	public void AddListener (Component Sender, Action<EventData> eventAction, string _eventListenerName)
	{
		// Check event name exist if not create it.
		if(!ListenerComponents.ContainsKey (_eventListenerName))
			ListenerComponents.Add (_eventListenerName, new List<ListenersData>());

		// Loop throw all listeners
		for (int i = 0, count = ListenerComponents[_eventListenerName].Count; i < count; i++) {
			
			// Check for not to duplicate listener call
			if (ListenerComponents[_eventListenerName][i].ListenerObject == Sender)
				return;
		}

		// Set new listener data
		ListenersData senderData = new ListenersData ();
		senderData.ListenerObject = Sender;
		senderData.ListenerEvent = eventAction;

		// add data of new listener
		ListenerComponents[_eventListenerName].Add(senderData);

		//Clean list for any duplicates
		RemoveRedundancies();
	}

	/// <summary>
	/// Removes the listener.
	/// </summary>
	/// <param name="_sender">Sender monobehaiviour.</param>
	/// <param name="_eventName">Event name.</param>
	public void RemoveListener (Component _sender, string _eventName)
	{
		// Check if current event exists
		if(!ListenerComponents.ContainsKey(_eventName))
			return;

		// Loop throw all register events
		for(int i = ListenerComponents[_eventName].Count-1; i >= 0; i--) 
		{
				// Remove wanted event
			if(ListenerComponents[_eventName][i].ListenerObject.GetInstanceID () == _sender.GetInstanceID())
				ListenerComponents[_eventName].RemoveAt(i);
		}
	}

	/// <summary>
	/// Lists the events. Mainly for debugging process
	/// </summary>
	public void ListEvents ()
	{
		// Loop throw all listeners
		foreach (KeyValuePair<string, List<ListenersData>> pair in ListenerComponents) {
			
			// Loop throw all events in listener
			for (int i = 0, count = pair.Value.Count; i < count; i++) {

				// Log Event
				Debug.Log (pair.Key + " <--> " + pair.Value[i].ListenerObject.name);
			}
		}
	}

	/// <summary>
	/// Calls the event.
	/// </summary>
	/// <param name="_sender">Sender Monobehaiour.</param>
	/// <param name="_eventName">Event name.</param>
	public void CallEvent (Component _sender, string _eventName)
	{
		// Call Event
		CallEvent (_sender, _eventName, null);
	}

	/// <summary>
	/// Calls the event.
	/// </summary>
	/// <param name="_sender">Sender.</param>
	/// <param name="_eventName">Event name.</param>
	/// <param name="_data">Data.</param> TODO: Here we have some boxing and there will be unboxing at receiving event callback. Refactor this!!
	public void CallEvent (Component _sender, string _eventName, object _data)
	{
		// Dispatch event
		NotifyListener (new EventData (_sender, _eventName, _data));
	}

	/// <summary>
	/// Clears the listeners.
	/// </summary>
	public void ClearListeners()
	{
		// Clear all events listeners from list
		ListenerComponents.Clear();
	}

	/// <summary>
	/// Notifies the listener.
	/// </summary>
	/// <param name="data">Data.</param>
	private void NotifyListener (EventData data)
	{
		// Check if event exists
		if(!ListenerComponents.ContainsKey(data.EventName))
			return;

		// Loop throw all listeners
		for (int i = 0, count = ListenerComponents[data.EventName].Count; i < count; i++) {
			
			// Dispatch Event
			if (ListenerComponents [data.EventName] [i].ListenerEvent != null) {

				// Execute event
				ListenerComponents [data.EventName] [i].ListenerEvent (data);
			}
		}
	}

	/// <summary>
	/// Removes the redundancies.
	/// </summary>
	private void RemoveRedundancies ()
	{
		Dictionary <string, List<ListenersData>> TmpListeners = new Dictionary<string, List<ListenersData>>();

		foreach (KeyValuePair<string, List<ListenersData>> Item in ListenerComponents)
		{
			for (int i = Item.Value.Count - 1; i>=0; i--)
			{
				if (Item.Value[i].ListenerObject == null || Item.Value[i].ListenerEvent == null)
					Item.Value.RemoveAt(i);
			}

			if (Item.Value.Count > 0)
				TmpListeners.Add (Item.Key, Item.Value);
		}

		ListenerComponents = TmpListeners;
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	private void OnDisable ()
	{
		ClearListeners();
	}

	/*
	void OnLevelWasLoaded(int level)
	{
		ClearListeners();
	}
	*/

	/// <summary>
	/// Raises the application quit event.
	/// </summary>
	private void OnApplicationQuit()
	{
		instance = null;
	}
}

/// <summary>
/// Event data.
/// </summary>
public class EventData
{
	/// <summary>
	/// The sender monobehaviour.
	/// </summary>
	public Component Sender;

	/// <summary>
	/// The name of the event.
	/// </summary>
	public string EventName;

	/// <summary>
	/// The data.
	/// </summary>
	public object Data = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="EventData"/> class.
	/// </summary>
	/// <param name="_sender">Sender.</param>
	/// <param name="_eventName">Event name.</param>
	public EventData (Component _sender, string _eventName)
	{
		Sender = _sender;
		EventName = _eventName;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EventData"/> class.
	/// </summary>
	/// <param name="_sender">Sender.</param>
	/// <param name="_eventName">Event name.</param>
	/// <param name="_data">Data.</param>
	public EventData (Component _sender, string _eventName, object _data)
	{
		Sender = _sender;
		EventName = _eventName;
		Data = _data;
	}
}

/// <summary>
/// Listeners data.
/// </summary>
public class ListenersData
{
	/// <summary>
	/// The listener object (MonoBehaviour).
	/// </summary>
	public Component ListenerObject;

	/// <summary>
	/// The listener event.
	/// </summary>
	public Action<EventData> ListenerEvent;
}