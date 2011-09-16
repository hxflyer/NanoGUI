using UnityEngine;
using System.Collections;


public class GuiManager : MonoBehaviour {
	
	// stage is the full stage of all 2d gui component to stay
	private static Stage _stage;
	
	
	
	private static GuiManager _instance;
	
	public static GuiManager instance {
		
    	get { return _instance; }
    	private set { _instance = value;}
	}
	
	protected static bool _isStageIntialized	= false;
	
	public static Stage stage {
    	get { 
			if(!_isStageIntialized){
				_stage	= new Stage();
				_isStageIntialized = true;
			}
			return _stage; 
		}
    	private set { _stage = value;}
	}
	
	void Start () {
		Debug.Log("start GuiManager");
		_instance	= this.GetComponent("GuiManager") as GuiManager;
	}
	
	
	// Update is called once per frame
	void Update () {
		NanoTween.update();
		//touch event
		_stage.sotreTouches(Input.touches);
		
		if(Input.touches.Length==1){
			_stage.sotreMousePosition(Input.touches[0].position);
		}
		foreach(Touch touch in Input.touches)
    	{
			//Debug.Log(touch.position);
			if (touch.phase == TouchPhase.Began){
				recursionTouchHitTest(_stage,TouchEvent.TOUCH_BEGAN,touch);
			}else if(touch.phase == TouchPhase.Moved){
				recursionTouchHitTest(_stage,TouchEvent.TOUCH_MOVED,touch);
			}else if(touch.phase == TouchPhase.Ended){
				recursionTouchHitTest(_stage,TouchEvent.TOUCH_ENDED,touch);
			}else if(touch.phase == TouchPhase.Canceled){
				recursionTouchHitTest(_stage,TouchEvent.TOUCH_CANCELED,touch);
			}else if(touch.phase == TouchPhase.Stationary){
				recursionTouchHitTest(_stage,TouchEvent.TOUCH_STATIONARY,touch);
			}
			
		}
	}
	
	void OnGUI(){
		
		stage.updateTransform();
		stage.updateTransformInTree();
		stage.updateOriginalSize();
		stage.updateBoundRect();
		stage.render();

		//mouse event
		Debug.Log(Event.current.type);
		//_stage.cleanMousePosition();
		if (Event.current.button == 0 ){
			if(Event.current.type == EventType.MouseMove) {
				
				_stage.sotreMousePosition(Event.current.mousePosition);
				
			}else if (Event.current.type == EventType.MouseDrag) {
				
			_stage.sotreMousePosition(Event.current.mousePosition);
				
			}else if (Event.current.type == EventType.MouseDown) {
				
				_stage.sotreMousePosition(Event.current.mousePosition);
				recursionHitTest(_stage,MouseEvent.MOUSE_DOWN,Event.current.mousePosition);
				
			}else if (Event.current.type == EventType.MouseUp) {

				_stage.sotreMousePosition(Event.current.mousePosition);
				recursionHitTest(_stage,MouseEvent.MOUSE_UP,Event.current.mousePosition);
				
			}
		}
		
		

		
		
		
	}
	
	
	
	bool recursionHitTest(DisplayObject target,string type,Vector2 position){
		
		//Debug.Log(target.id + "/" + target.boundRect);
		
		bool isHit = false;
		
		if(target.boundRect.Contains(position)){
			
			// if target is sprite ,and texture exist, check if position is hit the rendering rect 
			if(target is Sprite && (target as Sprite).texture && (target as Sprite).textureRenderRect.Contains(position)){
				isHit = true;
			}
			
			// go loop the child
			
			if(target is DisplayObjectContainer){
				DisplayObjectContainer	t	= target as DisplayObjectContainer;
				for(int i=t.childList.Count-1;i>=0;i--){
					if(recursionHitTest(t.childList[i] as DisplayObject,type,position)){
						isHit	= true;
						break;
					}
				}
			}
			if(isHit){
				target.dispatchEvent(new MouseEvent(target,type,position,new Vector2(position.x-target.transformInTree.tx,position.y-target.transformInTree.ty)));
			}
		}
		return isHit;
	}
	
	
	
	
	bool recursionTouchHitTest(DisplayObject target,string type,Touch touch){
		
		//Debug.Log(target.id + "/" + target.boundRect);
		
		bool isHit = false;
		
		if(target.boundRect.Contains(touch.position)){
			
			// if target is sprite ,and texture exist, check if position is hit the rendering rect 
			if(target is Sprite && (target as Sprite).texture && (target as Sprite).textureRenderRect.Contains(touch.position)){
				isHit = true;
			}
			
			// go loop the child
			
			if(target is DisplayObjectContainer){
				DisplayObjectContainer	t	= target as DisplayObjectContainer;
				for(int i=t.childList.Count-1;i>=0;i--){
					if(recursionTouchHitTest(t.childList[i] as DisplayObject,type,touch)){
						isHit	= true;
						break;
					}
				}
			}
			
			if(isHit){
				//Debug.Log(target.id +"/"+type);
				target.dispatchEvent(new TouchEvent(target,type,touch));
			}
		}
		return isHit;
	}
}
