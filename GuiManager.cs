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
		
		_stage.cleanMousePosition();
		//touch event
		_stage.updateTouches(Input.touches);
		
		if(Input.touches.Length==1){
			_stage.updateMousePosition(new Vector2(Input.touches[0].position.x,stage.height-Input.touches[0].position.y),new Vector2(Input.touches[0].deltaPosition.x,-Input.touches[0].deltaPosition.y));
		}
		foreach(Touch touch in Input.touches)
    	{
			//Debug.Log(touch.position);
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
	}
	
	void OnGUI(){
		
		stage.updateTransform();
		stage.updateTransformInTree();
		stage.updateBoundRect();
		stage.render();

		//mouse event
		
		if (Event.current.button == 0 ){
			if(Event.current.type == EventType.MouseMove) {
				
				_stage.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				
			}else if (Event.current.type == EventType.MouseDrag) {
				
			_stage.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				
			}else if (Event.current.type == EventType.MouseDown) {
				
				_stage.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				_stage.hitTestMouseDispatch(MouseEvent.MOUSE_DOWN,Event.current.mousePosition);
				
			}else if (Event.current.type == EventType.MouseUp) {

				_stage.updateMousePosition(Event.current.mousePosition,Event.current.delta);
				_stage.hitTestMouseDispatch(MouseEvent.MOUSE_UP,Event.current.mousePosition);
				
			}
		}
		
		

		
		
		
	}
	
	
	
	/*bool recursionHitTest(DisplayObject target,string type,Vector2 position){
		
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
	
	
	
	
	bool hitTestEventDispatch(DisplayObject target,string type,Touch touch){
		
		//Debug.Log(target.id + "/" + target.boundRect);
		
		bool isHit = false;
		
		if(target.hittest(new Vector2(touch.position.x,stage.height-touch.position.y))){
			
			// if target is sprite ,and texture exist, check if position is hit the rendering rect 
			if(target is Sprite && (target as Sprite).texture && (target as Sprite).textureRenderRect.Contains(new Vector2(touch.position.x,stage.height-touch.position.y))){
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
	}*/
}
