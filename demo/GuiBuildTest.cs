using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	public Sprite s1;
	public Sprite s2;

	void Start () {
		Texture2D texture = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s	= new Sprite();
		s.id	= "s";
		GuiManager.stage.addChild(s);
		Texture2D texture2 = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s1	= new Sprite(texture2);
		s.addChild(s1);
		//s.scaleX	= .3f;
		//s.scaleY	= .3f;
		
		s1.x	= -s1.width/2;
		s1.y	= -s1.height/2;
		Debug.Log(s1.width);
		s1.id	= "s1";
		/*s2	= new Sprite(texture2);
		s1.addChild(s2);
		s2.x	= 200;
		s2.y	= 400;
		s2.id	= "s2";*/
		
		//s.scaleX	= .5f;
		//s.scaleY	= .5f;
		//s.texture	= Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		//s.addEventListner(GuiEvent.ENTER_FRAME,new EventDispatcher.CallBack(enterFrameHandler));
		s.addEventListner(TouchEvent.TOUCH_BEGAN,new EventDispatcher.CallBack(toucheHandler));
		//s.addEventListner(MouseEvent.MOUSE_DOWN,new EventDispatcher.CallBack(mouseDownHandler));
		
		s.addEventListner(GestureEvent.SWIPE,new EventDispatcher.CallBack(swipeHandler));
		s.addEventListner(GestureEvent.ROTATE,new EventDispatcher.CallBack(rotateHandler));
		s.addEventListner(GestureEvent.ZOOM,new EventDispatcher.CallBack(zoomHandler));
		s.addEventListner(GestureEvent.PAN,new EventDispatcher.CallBack(panHandler));
		s.x		= 200;
		s.y		= 200;
		//s.rotation	= 40.0f;
		
	}
	
	void enterFrameHandler(GuiEvent e){
		//Debug.Log(Stage.instance.mouseX+"/"+Stage.instance.mouseY);
		//if(_isRotate){
			//s.rotation ++;
			
		//}
		
	}
	void zoomHandler(GuiEvent e){
		s.scaleX*= (e as GestureEvent).deltaScale;
		s.scaleY*= (e as GestureEvent).deltaScale;
		Debug.Log("zoom" + (e as GestureEvent).deltaScale);
	}
	void rotateHandler(GuiEvent e){
		s.rotation+=(e as GestureEvent).deltaRotation;
		Debug.Log("rotate"+ (e as GestureEvent).deltaRotation);
	}
	
	void swipeHandler(GuiEvent e){
		Debug.Log("s swiped");
	}
	void panHandler(GuiEvent e){
		Debug.Log("s pan");
		s.x	+= (e as GestureEvent).deltaPan.x;
		s.y	+= (e as GestureEvent).deltaPan.y;
	}
	void toucheHandler(GuiEvent e){
		Debug.Log("touched:"+ (e as TouchEvent).touch.position);
	}
	
	/*private bool _isRotate	= false;
	void mouseDownHandler(GuiEvent e){
		_isRotate= !_isRotate;
		Debug.Log("clicked:"+ (e as MouseEvent).mouseLocalPosition+"/"+ (e as MouseEvent).mouseGlobalPosition);
	}*/
	// Update is called once per frame
	void Update () {
		
		
		//bigimg.rotation++;
	}
 
}
