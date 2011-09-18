using UnityEngine;
using System.Collections;

public class TouchEvent : GuiEvent {
	
	//A finger touched the screen.
	public static string TOUCH_BEGAN	= "touch began";
	
	//A finger moved on the screen.
	public static string TOUCH_MOVED 	= "touch moved";
	
	//A finger is touching the screen but hasn't moved.
	public static string TOUCH_STATIONARY 	= "touch stationary";
	
	//A finger was lifted from the screen. This is the final phase of a touch.
	public static string TOUCH_ENDED		= "touch ended";
	
	//The system cancelled tracking for the touch, as when (for example) the user puts the device to her face or more than five touches happened simultaneously. This is the final phase of a touch.
	public static string TOUCH_CANCELED	= "touch canceled";
	
	public Touch touch;
	
	public TouchEvent (string type,Touch touch) {
		eventType		= type;
		this.touch		= touch;
	}
	
}
