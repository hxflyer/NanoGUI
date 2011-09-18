using UnityEngine;
using System.Collections;

public class GuiEvent {

	public static string SELECT		= "select";
	public static string UNSELECT	= "unselect";
	public static string CHANGE		= "change";
	public static string COMPLETE	= "complete";
	public static string RESIZE		= "resize";
	public static string ENTER_FRAME = "enterframe";
	
	public string eventType;
	public EventDispatcher target;
	
	public GuiEvent (){
	}
	public GuiEvent (string type) {
		eventType	= type;
	}
	
	
}
