using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	public Sprite s;
	public Sprite s1;
	public Sprite s2;

	void Start () {
		Texture2D texture = Resources.Load("testimg",typeof(Texture2D)) as Texture2D;
		s	= new Sprite(texture);
		GuiManager.stage.addChild(s);
		Texture2D texture2 = Resources.Load("testimg2",typeof(Texture2D)) as Texture2D;
		s1	= new Sprite(texture2);
		s.addChild(s1);
		s1.x	= 100;
		s1.y	= 100;
		s.texture	= Resources.Load("testimg3",typeof(Texture2D)) as Texture2D;
		s.addEventListner(GuiEvent.ENTER_FRAME,new EventDispatcher.CallBack(enterFrameHandler));
	}
	
	void enterFrameHandler(GuiEvent e){
		Debug.Log(Stage.instance.mouseX+"/"+Stage.instance.mouseY);
	}
	// Update is called once per frame
	void Update () {
		
		
		//bigimg.rotation++;
	}
 
}
