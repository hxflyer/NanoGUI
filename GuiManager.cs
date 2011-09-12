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
	}
	
	void OnGUI(){
		
		stage.updateTransform();
		stage.updateTransformInTree();
		stage.updateOriginalSize();
		stage.updateBoundRect();
		stage.render();
		
		
		//mouse event
		
		if (Event.current.button == 0 && Event.current.type == EventType.MouseDown) {
			recursionHitTest(_stage,MouseEvent.MOUSE_DOWN,Event.current.mousePosition);
		}
		if (Event.current.button == 0 && Event.current.type == EventType.MouseUp) {
			recursionHitTest(_stage,MouseEvent.MOUSE_UP,Event.current.mousePosition);
		}
	}
	
	
	
	bool recursionHitTest(DisplayObject target,string type,Vector2 position){
		
		
		bool isHit = false;
		
		if(target.boundRect.Contains(position)){
			
			// if target is sprite ,and texture exist, check if position is hit the rendering rect 
			if(target is Sprite && (target as Sprite).texture && (target as Sprite).textureRenderRect.Contains(position)){
				isHit = true;
			}
			
			// go loop the child
			
			if(target is DisplayObjectContainer){
				DisplayObjectContainer	t	= (target as DisplayObjectContainer);
				for(int i=t.childList.Count-1;i>=0;i--){
					if(recursionHitTest(t.childList[i] as DisplayObject,type,position)){
						break;
					}
				}
			}
			
			
			if(isHit){
				//Debug.Log("hit : "+ MouseEvent.MOUSE_DOWN +"  "+  target.id + "  "+position);
				target.dispatchEvent(new MouseEvent(target,type,position,new Vector2(position.x-target.transformInTree.tx,position.y-target.transformInTree.ty)));
			}
		}
		return isHit;
	}
	
}
