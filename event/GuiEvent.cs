using UnityEngine;
using System.Collections;

public class GuiEvent {


	public string eventType;
	public EventDispatcher target;
	
	public GuiEvent (){
	}
	public GuiEvent (EventDispatcher target,string type) {
		this.target = target;
		eventType	= type;
	}
	
	
}
