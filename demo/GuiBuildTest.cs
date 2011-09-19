using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	public Sprite s1;
	public Sprite s2;

	void Start () {
		Texture2D texture = Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s	= new Sprite(texture,100,100);
		s.id	= "s";
		GuiManager.stage.addChild(s);
		//s.width	= 100;
		//s.height = 100;
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
