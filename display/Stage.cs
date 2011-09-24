using UnityEngine;
using System.Collections;

public class Stage : DisplayObjectContainer {

	
	// instance
	private static Stage _instance;
	public static Stage instance {
		
    	get { 
			if(_instance==null){
				_instance	= new Stage();
			}
			return _instance;
		}
	}
	
	public Stage (){
		_instance	= this;
		_stage		= this;
		id			= "stage";
		updateTransform();
		updateTransformInTree();
		
	}
	
	private bool _isFirstUpdateComplete	=false;
	public bool isFirstUpdateComplete{
		get { return _isFirstUpdateComplete;}
	}
	
	public override void updateTransform ()
	{
		_isFirstUpdateComplete	= true;
		base.updateTransform ();
	}
	
	override public void render(){
		//GUI.DrawTexture(_boundRectInRree,Resources.Load("frame",typeof(Texture2D)) as Texture2D);
		renderChildren();
	}
	
	
	override public void updateTransformInTree(){
		_transformInTree		= _transform;
	}
	
	
	
	override public void addChild(DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Add(child);
		child.stage			= this;
		child.parent		= this;
	}
	
	override public void addChildAt(int index, DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Insert(index,child);
		child.stage			= this;
		child.parent		= this;
	}
	/********************************
	 * touchs and mouse positions
	 *******************************/
	
	private ArrayList _touchAry	= new ArrayList();
	
	public ArrayList touchAry{
		get {return _touchAry;}
	}
	
	public void updateTouches(Touch[] touchs){
		
		if(_touchAry!=null && _touchAry.Count>0){
			_touchAry.Clear();
		}
		if(touchs.Length>0){
			_touchAry	= ArrayList.Adapter(touchs);
		}
	}
	
	/********************************
	 * mouse position
	 *******************************/
	
	
	private int _mouseX = 0;
	public int mouseX{
		get {return _mouseX;}
	}
	
	private int _mouseY = 0;
	public int mouseY{
		get {return _mouseY;}
	}
	
	private int _lastMouseX = 0;
	public int lastMouseX{
		get {return _lastMouseX;}
	}
	
	private int _lastMouseY = 0;
	public int lastMouseY{
		get {return _lastMouseY;}
	}
	
	public void updateMousePosition(Vector2 mousePos,Vector2 deltaPos){
		_mouseX	= (int)mousePos.x;
		_mouseY	= (int)mousePos.y;
		_lastMouseX = (int)deltaPos.x;
		_lastMouseY = (int)deltaPos.y;
	}
	
	public void cleanMousePosition(){
		_mouseX	= 0;
		_mouseY	= 0;
	}
	
	/********************************
	 * stage size
	 *******************************/
	
	private int _stageWidth		= Screen.width;
	public int stageWidth{
		get{return _stageWidth;}
	}
	
	private int _stageHeight	= Screen.height;
	public int stageHeight{
		get{return _stageHeight;}
	}
	
	
	
	override public void updateBoundRect(){


		_isBoundRectDirty	= false;
		Vector2 minPos	= new Vector2(999999,999999);
		Vector2 maxPos	= new Vector2(-999999,-999999);
		
		
		// loop though children to get the bound area of children
		
		foreach(DisplayObject child in _childList){
			
			Rect childBoundRect	= _transform.getBoundRect(child.boundRect);
			if(childBoundRect.x < minPos.x){
				minPos.x	= childBoundRect.x;
			}
			if(childBoundRect.x+childBoundRect.width > maxPos.x){
				maxPos.x	= childBoundRect.x+childBoundRect.width;
			}
			if(childBoundRect.y < minPos.y){
				minPos.y	= childBoundRect.y;
			}
			if(childBoundRect.y+childBoundRect.height > maxPos.y){
				maxPos.y	= childBoundRect.y+childBoundRect.height;
			}
		}
		
		if(maxPos.x>minPos.x && maxPos.y>minPos.y){
			_originalWidth			= maxPos.x-minPos.x;
				
			_originalHeight			= maxPos.y-minPos.y;
			
			
			// get boundRect, boundRect is related with it's parent
			_boundRect.x			= minPos.x;
			_boundRect.y			= minPos.y;
			_boundRect.width		= maxPos.x-minPos.x;
			_boundRect.height		= maxPos.y-minPos.y;
			_width					= _boundRect.width;
			_height					= _boundRect.height;		
		}else{
			_boundRect.x			= 0;
			_boundRect.y			= 0;
			_boundRect.width		= 0;
			_boundRect.height		= 0;
			_width					= 0;
			_height					= 0;
		}
		
		_boundRectInTree		= _boundRect;
		//Debug.Log(id+ "updateBoundRect" + _boundRect);
	}
}
