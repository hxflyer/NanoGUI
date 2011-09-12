using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {

	// Use this for initialization
	
	private Texture2D t;
	Sprite	bigimg;
	Sprite	bigimg2;
	void Start () {
		Debug.Log("start GuiBuild");
		t	= Resources.Load("bigimg",typeof(Texture2D)) as Texture2D;
		
		bigimg	= new Sprite(t);

		
		bigimg.id	= "bigimg";

		GuiManager.stage.addChild(bigimg);
		bigimg.alpha= 0.5f;
		bigimg.x	= 350;
		bigimg.y	= 350;
		
		bigimg.scaleX	= 0.5f;
		bigimg.scaleY	= 0.5f;
		
		
		bigimg2	= new Sprite(t);

		bigimg2.id	= "img2";
		bigimg.addChild(bigimg2);
		bigimg2.x	= 150;
		bigimg2.y	= 150;
		bigimg2.scaleX	= 0.2f;
		bigimg2.alpha	= 0.5f;
		
		bigimg2.addEventListner(MouseEvent.MOUSE_DOWN,new EventDispatcher.CallBack(img2MouseDownHandler));
	}
	void img2MouseDownHandler(GuiEvent e){
		Debug.Log("img 2 clicked");
		NanoTween.to(bigimg,1,NanoTween.Pack("delay",0.2f,
		                                     "x",0.0f,
		                                     "scaleX",1.5f,
		                                     "alpha",1.0f,
		                                     "ease",Ease.easeOutExpo,
		                                     NanoTween.ON_COMPLETE,new NanoTween.CallBack(imgTweenCompleteHandler),
		                                     NanoTween.ON_START,new NanoTween.CallBack(imgTweenStartHandler),
		                                     NanoTween.ON_UPDATE,new NanoTween.CallBack(imgTweenUpdateHandler),
		                                     NanoTween.ON_UPDATE_PARAMS,NanoTween.Pack(2,3,4,5,6)));
		
		//Debug.Log( (e as MouseEvent).mouseGlobalPosition);
		//Debug.Log((e as MouseEvent).mouseLocalPosition);
	}
	void imgTweenCompleteHandler(object[] args){
		//Debug.Log("complete");
	}
	void imgTweenStartHandler(object[] args){
		//Debug.Log("start");
		
	}
	void imgTweenUpdateHandler(object[] args){
		//Debug.Log("update");
		
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log(bigimg.textureRenderRect);
		//bigimg.rotation++;
	}
 
}
