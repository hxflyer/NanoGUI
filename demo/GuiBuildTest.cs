using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	public Sprite s1;
	public Sprite s2;

	void Start () {
		Texture2D texture = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s	= new Sprite(texture);
		s.id	= "s";
		GuiManager.stage.addChild(s);
		Texture2D texture2 = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s1	= new Sprite(texture2);
		s.addChild(s1);
		s1.x	= 100;
		s1.y	= 100;
		s1.id	= "s1";
		//s.scaleX	= .5f;
		//s.scaleY	= .5f;
		//s.texture	= Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s.addEventListner(GuiEvent.ENTER_FRAME,new EventDispatcher.CallBack(enterFrameHandler));
		//s.addEventListner(TouchEvent.TOUCH_BEGAN,new EventDispatcher.CallBack(toucheHandler));
		s.addEventListner(MouseEvent.MOUSE_DOWN,new EventDispatcher.CallBack(mouseDownHandler));
		
		s.x		= 500;
		s.y		= 300;
		s.rotation	= 40.0f;
		
	}
	
	void enterFrameHandler(GuiEvent e){
		//Debug.Log(Stage.instance.mouseX+"/"+Stage.instance.mouseY);
		if(_isRotate){
			s.rotation ++;
			
		}
		
	}
	
	void toucheHandler(GuiEvent e){
		
		Debug.Log("touched:"+ (e as TouchEvent).touch.position);
	}
	private bool _isRotate	= false;
	void mouseDownHandler(GuiEvent e){
		_isRotate= !_isRotate;
		Debug.Log("clicked:"+ (e as MouseEvent).mouseLocalPosition+"/"+ (e as MouseEvent).mouseGlobalPosition);
	}
	// Update is called once per frame
	void Update () {
		
		
		//bigimg.rotation++;
	}
 
}
