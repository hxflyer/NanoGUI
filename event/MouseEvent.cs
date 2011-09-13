using UnityEngine;
using System.Collections;

public class MouseEvent : GuiEvent {

	public static string MOUSE_DOWN	= "mouse down";
	public static string MOUSE_UP	= "mouse up";
	
	public Vector2 mouseLocalPosition;
	public Vector2 mouseGlobalPosition;
	
	public MouseEvent (EventDispatcher target,string type,Vector2 localPosition,Vector2 globalPosition) {
		
		this.target		= target;
		eventType		= type;
		mouseLocalPosition	= localPosition;
		mouseGlobalPosition	= globalPosition;
	}
	
}
