using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	public Sprite s1;
	public Sprite s2;
	public Sprite s3;
	public Sprite s4;
	public Sprite s5;
	
	void Start () {
		Texture2D texture = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s	= new Sprite(texture);
		s.id	= "s";
		Stage.instance.addChild(s);
		
		s.addEventListner(GestureEvent.SWIPE,new EventDispatcher.CallBack(swipeFrameHandler));
		s.addEventListner(GuiEvent.ENTER_FRAME,new EventDispatcher.CallBack(enterFrameHandler));
		
		s.addEventListner(GestureEvent.PAN,new EventDispatcher.CallBack(panHandler));
		s.addEventListner(GestureEvent.ROTATE,new EventDispatcher.CallBack(rotateHandler));
		s.addEventListner(GestureEvent.RESIZE,new EventDispatcher.CallBack(zoomHandler));
	}
	void swipeFrameHandler(GuiEvent e){
		Debug.Log("swipe");
	}
	
	void enterFrameHandler(GuiEvent e){
		//Debug.Log(Stage.instance.mouseX+"/"+Stage.instance.mouseY);

	}
	void zoomHandler(GuiEvent e){
		s.scaleX*= (e as GestureEvent).deltaScale;
		s.scaleY*= (e as GestureEvent).deltaScale;
		//Debug.Log("zoom" + (e as GestureEvent).deltaScale);
	}
	void rotateHandler(GuiEvent e){
		s.rotation+=(e as GestureEvent).deltaRotation;
		//Debug.Log("rotate"+ (e as GestureEvent).deltaRotation);
	}
	
	void swipeHandler(GuiEvent e){
		Debug.Log("s swiped");
	}
	void panHandler(GuiEvent e){
		//Debug.Log("s pan");
		s.x	+= (e as GestureEvent).deltaPan.x;
		s.y	+= (e as GestureEvent).deltaPan.y;
	}
	void toucheHandler(GuiEvent e){
		//Debug.Log("touched:"+ (e as TouchEvent).touch.position);
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
