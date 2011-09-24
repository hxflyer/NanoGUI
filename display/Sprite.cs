using UnityEngine;
using System.Collections;

public class Sprite : DisplayObjectContainer {
	
	
	public Sprite (Texture texture){
		_texture	= texture;
		_originalWidth	= _texture.width;
		_originalHeight	= _texture.height;
		_width			= _texture.width;
		_height			= _texture.height;
		_textureSelfRect.width = _texture.width;
		_textureSelfRect.height= _texture.height;
	}
	
	public Sprite(Texture texture, float textureWidth,float textureHeight){
		_texture	= texture;
		_originalWidth	= textureWidth;
		_originalHeight	= textureHeight;
		_width			= textureWidth;
		_height			= textureHeight;
		_textureSelfRect.width = textureWidth;
		_textureSelfRect.height= textureHeight;
	}
	
	public Sprite (){
		
	}
	
	/***************************************
	 * texture
	 ***************************************/
	protected Texture _texture;
	public Texture texture {
    	get { return _texture; }
    	set { _texture = value;
			_textureSelfRect.width = _texture.width;
			_textureSelfRect.height= _texture.height;
			updateParentBoundRect();
		}
	}
	
	public void setTexutre(Texture texture, float textureWidth,float textureHeight){
		_texture	= texture;
		_textureSelfRect.width = textureWidth;
		_textureSelfRect.height= textureHeight;
		updateParentBoundRect();
	}
	
	protected Rect _textureRenderRect	= new Rect();
	protected Vector2 _texturRenderRotatePivot	= new Vector2();
	protected Rect _textureSelfRect		= new Rect();
	
	public Rect textureRenderRect {
    	get { return _textureRenderRect; }
    	private set { _textureRenderRect = value;}
	}
	
	/***************************************
	 * render
	 ***************************************/
	
	
	override public void render(){
		_alphaInTree	= _alpha * parent.alphaInTree;
		
		if(!_visible && _alphaInTree<=0){
			return;
		}
		
		if(_texture){
			
			_textureRenderRect.x	= _transformInTree.tx;
			_textureRenderRect.y	= _transformInTree.ty;
			_textureRenderRect.width	= _textureSelfRect.width * _transformInTreeScale.x;
			_textureRenderRect.height	= _textureSelfRect.height * _transformInTreeScale.y;
			//Debug.Log(_textureRenderRect);
			GUI.color = new Color( 1, 1, 1, _alphaInTree );
			_texturRenderRotatePivot.x	= _transformInTree.tx;
			_texturRenderRotatePivot.y	= _transformInTree.ty;
			GUIUtility.RotateAroundPivot (_transformInTreeRotation, _texturRenderRotatePivot);
			//Debug.Log(id+" : "+_boundRectInTree);
			GUI.DrawTexture(_textureRenderRect,_texture);
			GUIUtility.RotateAroundPivot (-_transformInTreeRotation, _texturRenderRotatePivot);
			//GUI.DrawTexture(_boundRectInTree,Resources.Load("frame",typeof(Texture2D)) as Texture2D);
		}
		//Debug.Log("render:"+id+"  rect:"+_textureRenderRect);
		renderChildren();
		
	}
	
	
	override public void updateBoundRect(){
		Vector2 minPos = new Vector2();
		Vector2 maxPos = new Vector2();
		
		
		if(_texture!=null){
			
			Rect textureRect	= _transform.getBoundRect(_textureSelfRect);
			minPos.x	= textureRect.x;
			minPos.y	= textureRect.y;
			maxPos.x	= textureRect.width+textureRect.x;
			maxPos.y	= textureRect.height+textureRect.y;
		}else{
			minPos.x	= 999999;
			minPos.y	= 999999;
			maxPos.x	= -999999;
			maxPos.y	= -999999;
		}
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
		// get boundRect, boundRect is related with it's parent
		_boundRect.x			= minPos.x;
		_boundRect.y			= minPos.y;
		_boundRect.width		= maxPos.x-minPos.x;
		_boundRect.height		= maxPos.y-minPos.y;
		
		
		if(_parent!=null && _parent.transformInTree!=null){
			_boundRectInTree		= _parent.transformInTree.getBoundRect(_boundRect);
			
		}		
		
		if(_texture!=null){
			minPos.x	= _textureSelfRect.x;
			minPos.y	= _textureSelfRect.y;
			maxPos.x	= _textureSelfRect.width+_textureSelfRect.x;
			maxPos.y	= _textureSelfRect.height+_textureSelfRect.y;
		}else{
			minPos.x	= 999999;
			minPos.y	= 999999;
			maxPos.x	= -999999;
			maxPos.y	= -999999;
		}
		foreach(DisplayObject child in _childList){
			
			
			if(child.boundRect.x < minPos.x){
				minPos.x	= child.boundRect.x;
			}
			if(child.boundRect.x+child.boundRect.width > maxPos.x){
				maxPos.x	= child.boundRect.x+child.boundRect.width;
			}
			if(child.boundRect.y < minPos.y){
				minPos.y	= child.boundRect.y;
			}
			if(child.boundRect.y+child.boundRect.height > maxPos.y){
				maxPos.y	= child.boundRect.y+child.boundRect.height;
			}
		}
		
		_originalWidth			= maxPos.x-minPos.x;
		_originalHeight			= maxPos.y-minPos.y;
		_width					= _originalWidth*_scaleX;
		_height					= _originalHeight*_scaleY;
		
		
	}
	
	
	/***************************************
	 * hit test
	 ***************************************/
	override public bool hittest(Vector2 vec){

		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		//Debug.Log(newVec);
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		if(_texture && _textureRenderRect.Contains(newVec)){
			return true;
		}
		bool isHit = false;
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hittest(vec)){
				isHit	= true;
				break;
			}
			
		}
		return isHit;
	}
	
	override public bool hitTestMouseDispatch(string type,Vector2 vec){
		
		if(!mouseEnable || !_visible){
			return false;
		}
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		//Debug.Log(id+"/"+ newVec + _selfBoundRect+_textureSelfRect);
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		if(_texture && _textureSelfRect.Contains(newVec)){
			isHit = true;
		}
		
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hitTestMouseDispatch(type,vec)){
				isHit	= true;
				break;
			}
			
		}
		//Debug.Log(id+"  mouse hit test "+ isHit);
		if(isHit){
			//Debug.Log(id+"/"+type);
			this.dispatchEvent(new MouseEvent(type,newVec,vec));
		}
		return isHit;
	}
	
	override public bool hitTestTouchDispatch(string type,Touch touch){
		if(!mouseEnable || !_visible){
			return false;
		}
		Vector2 vec = new Vector2(touch.position.x,Stage.instance.stageHeight- touch.position.y);
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		
		if(!_boundRectInTree.Contains(vec)){
			return false;
		}
		bool isHit = false;
		if(_texture && _textureSelfRect.Contains(newVec)){
			isHit = true;
		}
		
		DisplayObject child;
		for(int i=_childList.Count-1;i>=0;i--){
			child	= _childList[i] as DisplayObject;
			if(child.hitTestTouchDispatch(type,touch)){
				isHit	= true;
				break;
			}
			
		}
		if(isHit){
			_touchList.Add(touch);
			this.dispatchEvent(new TouchEvent(type,touch));
		}
		return isHit;
	}
	
	
	
}
