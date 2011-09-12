using UnityEngine;
using System.Collections;

public class EventTerm : Object {
	
	public EventDispatcher.EventHandlerFunction function;
	public string eventType;
	
	// Use this for initialization
	public EventTerm (string type,EventDispatcher.EventHandlerFunction func) {
		eventType	= type;
		function	= func;
	}
	
}
