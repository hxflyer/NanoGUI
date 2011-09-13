using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {

	void Start () {

		Texture2D texture = Resources.Load("testimg",typeof(Texture2D)) as Texture2D;
		
		Sprite img1	= new Sprite(texture);

		GuiManager.stage.addChild(img1);
		
		/*img1.alpha		= 0.5f;
		img1.x			= 350.0f;
		img1.scaleX		= 0.5f;
		img1.id			= "img1";
		
		Sprite img2		= new Sprite(texture);

		img1.addChild(img2);
		img2.y			= 150.0f;
		img2.scaleX		= 0.5f;
		img2.scaleY		= 0.5f;
		img2.alpha		= 0.5f;
		img2.id			= "img2";
		img2.addEventListner(MouseEvent.MOUSE_DOWN,
		                     new EventDispatcher.CallBack(img2MouseDown));*/
		
		
	}
	
	
	void img2MouseDown(GuiEvent e){
		Debug.Log("img 2 clicked");
		NanoTween.to(e.target,1,NanoTween.Pack("delay",0.2f,
                             "x",0.0f,
                             "scaleX",1.5f,
                             "alpha",1.0f,
                             "ease",Ease.easeOutExpo,
                             "onComplete",new NanoTween.CallBack(imgComplete),
                             "onStart",new NanoTween.CallBack(imgStart),
                             "onUpdate",new NanoTween.CallBack(imgUpdate),
                             "onUpdateParams",NanoTween.Pack(e.target)));
	}
	
	
	void imgComplete(object[] args){
		Debug.Log("complete");
	}
	void imgStart(object[] args){
		Debug.Log("start");
		
	}
	void imgUpdate(object[] args){
		Debug.Log("update");	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(bigimg.textureRenderRect);
		//bigimg.rotation++;
	}
 
}
