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
	
	public int stageWidth	= Screen.width;
	public int stageHeight	= Screen.height;
	
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
		
		updateBoundRect();
	}
}
