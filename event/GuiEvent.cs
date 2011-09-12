using UnityEngine;
using System.Collections;

public class GuiEvent : Object {


	public string eventType;
	public EventDispatcher dispatchTarget;
	
	public GuiEvent (){
	}
	public GuiEvent (EventDispatcher target,string type) {
		dispatchTarget = target;
		eventType	= type;
	}
	
	
}
