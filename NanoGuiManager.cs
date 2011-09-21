using UnityEngine;
using System.Collections;

/**
 * VERSION: 0.5
 * DATE: 2011-09-20
 * c#
 * author: huang xiang
 * hxflyer@gmail.com
 * http://www.hxflyer.com
 **/




/**
 * NanoGUI is a OOP 2D GUI framework build for unity3D
 * Because the original GUI API in unity3D really sucks, so I build this OOP GUI framework to make our life easier,
 * to make this more easier,I tried my best to build this GUI framework to similar to Adobe Flash Actionscript 3.0 API
 * I hope my job also make people who use NanoGUI have better life.
 * this is it.
 **/

public class NanoGuiManager : MonoBehaviour {
	
	// stage is the full stage of all 2d gui component to stay
	private static Stage _stage;
	

	private static NanoGuiManager _instance;
	
	public static NanoGuiManager instance
	{
    	get { return _instance; }
	}
	
	
	void Start () {
		Debug.Log("start NanoGuiManager");
		//because NanoGuiManager is a instance of MonoBehaviour, when it add to a GameObject, 
		//"this" pointer will point to the gameobject instead MonoBehaviour itself.
		_instance	= this.GetComponent("NanoGuiManager") as NanoGuiManager;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		/**
		 * update tween
		 * NanoTween is a home made tween class specially enhanced for NanoGUI
		 * a lot of code of NanoTween are copy from iTween
		 * but different from iTween, each instance of nanoTween is just a pure data object
		 * so it's a slightly faster than iTween
		 **/
		NanoTween.update();
		
		/*
		 * go through the rendering tree to update matrix and boundRect
		 **/
		Stage.instance.dispatchEnterFrame();
		
		
		/*
		 * the solo touch position and mouse position are cached in mouseX and mouseY property in rendering tree
		 * this will clean the mouse position every frame, because on touch screen,fingers are not always touch the screen
		 **/
		Stage.instance.cleanMousePosition();
		
		/*
		 * this will go through the rendering tree to update touches
		 **/
		Stage.instance.updateTouches(Input.touches);
		
		/*
		 * solo touch will treat like a mouse move: store the position to rendering tree  
		 **/
		if(Input.touches.Length==1)
		{
			Stage.instance.updateMousePosition(new Vector2(Input.touches[0].position.x,Stage.instance.stageHeight-Input.touches[0].position.y),new Vector2(Input.touches[0].deltaPosition.x,-Input.touches[0].deltaPosition.y));
		}
		
		/*
		  * every frame, the touch instances in Input.touches will send through rendering tree and do hittest
		  * with each display object, the touch instances will be cached in different display objects if hited with touch point
		  * so this hittest-cache loop need to be clear once per frame
		 **/
		Stage.instance.clearTouchs();
		
		//send the touch to do hittest-cache one by one
		foreach(Touch touch in Input.touches)
    	{
			if (touch.phase == TouchPhase.Began){
				Stage.instance.hitTestTouchDispatch(TouchEvent.TOUCH_BEGAN,touch);
			}else if(touch.phase == TouchPhase.Moved){
				Stage.instance.hitTestTouchDispatch(TouchEvent.TOUCH_MOVED,touch);
			}else if(touch.phase == TouchPhase.Ended){
				Stage.instance.hitTestTouchDispatch(TouchEvent.TOUCH_ENDED,touch);
			}else if(touch.phase == TouchPhase.Canceled){
				Stage.instance.hitTestTouchDispatch(TouchEvent.TOUCH_CANCELED,touch);
			}else if(touch.phase == TouchPhase.Stationary){
				Stage.instance.hitTestTouchDispatch(TouchEvent.TOUCH_STATIONARY,touch);
			}
		}
		/*
		 * mutil touch gesture can only be recognize when all touches hittest-cache complete
		 **/
		Stage.instance.updateTouchs();
		
		
		//update events
		//this is ask event dispatcher to dispatch events once per frame
		EventDispatcher.sendEvents();
		//EventDispatcher.clearEvents();
	}
	
	
	//OnGUI is called 4 times per frame, this is depends on user's hardware
	void OnGUI(){
		
		//some user's hardware render OnGUI() later than Update(), this checkup is to makesure render after first update
		//otherwise some matrix in rendering tree will missing
		if(!Stage.instance.isFirstUpdateComplete){
			return;
		}
		
		Stage.instance.render();

		//mouse event
		
		if (Event.current.button == 0 ){
			if(Event.current.type == EventType.MouseMove) {
				
				Stage.instance.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				
			}else if (Event.current.type == EventType.MouseDrag) {
				
				Stage.instance.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				
			}else if (Event.current.type == EventType.MouseDown) {
				
				Stage.instance.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				Stage.instance.hitTestMouseDispatch(MouseEvent.MOUSE_DOWN,Event.current.mousePosition);
				
			}else if (Event.current.type == EventType.MouseUp) {

				Stage.instance.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				Stage.instance.hitTestMouseDispatch(MouseEvent.MOUSE_UP,Event.current.mousePosition);
				
			}
		}
	}
}
