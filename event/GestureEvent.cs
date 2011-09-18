using UnityEngine;
using System.Collections;

public class GestureEvent : GuiEvent {

	public static string SWIPE	= "swipe";
	public static string ROTATE = "rotate";
	public static string ZOOM	= "zoom";
	public static string PAN	= "pan";
	
	public static float	swipeDeltaThreshold	= 4;
	public static float	swipeCountThreshold	= 3;
	public static float panThreshold	= 0.001f;
	
	public float deltaScale;
	public float deltaRotation;
	public Vector2 deltaPan;
	
	public GestureEvent (string type) {
		eventType	= type;
	}
	
}
