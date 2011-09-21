using UnityEngine;
using System.Collections;

public class GestureEvent : GuiEvent {
	
	//onew finger swip though displayobject quickly
	public static string SWIPE	= "swipe";
	public static string SWIPE_LEFT	= "left";
	public static string SWIPE_RIGHT = "right";
	//two finger touch and rotate 
	public static string ROTATE = "rotate";
	public static string ZOOM	= "zoom";
	public static string PAN	= "pan";
	
	
	public static float	swipeDeltaThreshold	= 3;
	public static float	swipeCountThreshold	= 3;
	public static float panThreshold	= 0.001f;
	
	public float deltaScale;
	public float deltaRotation;
	public Vector2 deltaPan;
	public string swipeDirection;
	
	
	public GestureEvent (string type) {
		eventType	= type;
	}
	
}
