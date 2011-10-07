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
		s.x		= 300;
		s.y		= 300;
		s1	= new Sprite(texture);
		s1.id	= "s1";
		s.addChild(s1);
		s1.scaleX	= .5f;
		s1.x	= 150;
		s1.y	= 50;
		//s1.width	= 50;
		//s1.height	= 50;
		Debug.Log(s1.width+"/"+s1.height);
		s2	= new SelectableItem(texture);
		s2.id	= "s2";
		s1.addChild(s2);
		s2.x	= 250;
		s2.y	= 100;
		
		s3	= new Sprite(texture);
		s2.addChild(s3);
		s3.scaleX	= .3f;
		s3.scaleY	= .3f;
		s.addEventListner(MouseEvent.MOUSE_DOWN,new EventDispatcher.CallBack(clickHandler));
		s.addEventListner(GuiEvent.ENTER_FRAME,new EventDispatcher.CallBack(enterFrameHandler));
	}
	private bool _isRotate	= false;
	void clickHandler(GuiEvent e){
		_isRotate= !_isRotate;
		//s.rotation+=20;
	}
	void enterFrameHandler(GuiEvent e){
		if(_isRotate){
			s2.rotation++;
			//s.y++;
		}

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
