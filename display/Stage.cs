using UnityEngine;
using System.Collections;

public class Stage : DisplayObjectContainer {

	// Use this for initialization
	
	private static Stage _instance;
	public static Stage instance {
		
    	get { return _instance; }
    	private set { _instance = value;}
	}
	
	public Stage (){
		_instance	= this;
		_stage		= this;
		id		= "stage";

	}
	
	override public void render(){
		//GUI.DrawTexture(_boundRectInRree,Resources.Load("frame",typeof(Texture2D)) as Texture2D);
		
		renderChildren();
	}
	
	
	override public void updateTransformInTree(){
		
		if(_isTransformInTreeDirty){
			_transformInTree		= _transform;
			_transformInTreeScale	= new Vector2(1,1);
			setBoundRectDirty();
		}
		
		for(int i=0;i<_childList.Count;i++){
			DisplayObject child = _childList[i] as DisplayObject;
			child.updateTransformInTree();
		}
	}
	
	
	
	override public void addChild(DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Add(child);
		child.parent		= this;
		child.stage			= this;
		setTransformInTreeDirty();
		setBoundRectDirty();
	}
	
	override public void addChildAt(int index, DisplayObject child){
		if(_childList.IndexOf(child)!=-1){
			return;
		}
		if(child.parent!=null){
			child.parent.removeChild(child);
		}
		_childList.Insert(index,child);
		child.parent		= this;
		child.stage			= this;
		setTransformInTreeDirty();
		setBoundRectDirty();
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
	
	
	
	/*override public void updateOriginalSize(){
		if(!_isOriginalSizeDirty){
			return;
		}
		_originalWidth	= 0;
		_originalHeight	= 0;
		Vector2	minChildPos	= new Vector2(999999,999999);
		
		DisplayObject child;
		for(int i=0;i<_childList.Count;i++){
			child	= _childList[i] as DisplayObject;
			
			child.updateOriginalSize();
			if(child.x+child.width>_originalWidth){
				_originalWidth	= child.x+child.width;
			}
			if(child.y+child.height>_originalHeight){
				_originalHeight	= child.y+child.height;
			}
			if(child.x<minChildPos.x){
				minChildPos.x	= child.x;
			}
			if(child.y<minChildPos.y){
				minChildPos.y	= child.y;
			}
		}
		
		_originalWidth	-= minChildPos.x;
		_originalHeight	-= minChildPos.y;
		_width	= _originalWidth *_scaleX;
		_height	= _originalHeight *_scaleY;
		
		setBoundRectDirty();
	}*/
}
