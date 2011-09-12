using UnityEngine;
using System.Collections;

public class EventTerm : Object {
	
	public EventDispatcher.CallBack function;
	public string eventType;
	
	// Use this for initialization
	public EventTerm (string type,EventDispatcher.CallBack func) {
		eventType	= type;
		function	= func;
	}
	
}
