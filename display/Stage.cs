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
		id		= "stage";

	}
	
	override public void render(){
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
	
	
	
	
	/********************************
	 * touchs and mouse positions
	 *******************************/
	
	private ArrayList _touchAry	= new ArrayList();
	
	public ArrayList touchAry{
		get {return _touchAry;}
	}
	
	public void sotreTouches(Touch[] touchs){
		
		if(_touchAry!=null && _touchAry.Count>0){
			_touchAry.Clear();
		}
		if(touchs.Length>0){
			_touchAry	= ArrayList.Adapter(touchs);
		}
	}
	
	
	private int _mouseX = 0;
	public int mouseX{
		get {return _mouseX;}
	}
	private int _mouseY = 0;
	public int mouseY{
		get {return _mouseY;}
	}
	public void sotreMousePosition(Vector2 mousePos){
		_mouseX	= (int)mousePos.x;
		_mouseY	= (int)mousePos.y;
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
	
	
	
	override public void updateOriginalSize(){
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
	}
}
