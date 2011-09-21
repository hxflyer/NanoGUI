using UnityEngine;
using System.Collections;

public class EventListenTerm{
	
	public EventDispatcher.CallBack function;
	public string eventType;
	
	// Use this for initialization
	public EventListenTerm (string type,EventDispatcher.CallBack func) {
		eventType	= type;
		function	= func;
	}
	
}
