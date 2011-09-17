using UnityEngine;
using System.Collections;

public class Sprite : DisplayObjectContainer {
	
	
	public Sprite (Texture texture){
		_texture	= texture;
		_originalWidth	= _texture.width;
		_originalHeight	= _texture.height;
		_textureSelfRect.width = _texture.width;
		_textureSelfRect.height= _texture.height;
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
			setBoundRectDirty();
			_textureSelfRect.width = _texture.width;
			_textureSelfRect.height= _texture.height;
			
		}
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
			_textureRenderRect.width	= _texture.width * _transformInTreeScale.x;
			_textureRenderRect.height	= _texture.height * _transformInTreeScale.y;
			GUI.color = new Color( 1, 1, 1, _alphaInTree );
			_texturRenderRotatePivot.x	= _transformInTree.tx;
			_texturRenderRotatePivot.y	= _transformInTree.ty;
			
			GUIUtility.RotateAroundPivot (_transformInTreeRotation, _texturRenderRotatePivot);
			//Debug.Log(id+" : "+_transformInTreeRotation);
			GUI.DrawTexture(_textureRenderRect,_texture);
			GUIUtility.RotateAroundPivot (-_transformInTreeRotation, _texturRenderRotatePivot);
			GUI.DrawTexture(_boundRectInTree,Resources.Load("frame",typeof(Texture2D)) as Texture2D);
			
			Debug.Log(_boundRectInTree);
		}
		//Debug.Log("render:"+id+"  rect:"+_textureRenderRect);
		renderChildren();
		
	}
	
	
	override public void updateBoundRect(){
		
		for(int i=0;i<_childList.Count;i++){
			(_childList[i] as DisplayObject).updateBoundRect();
		}
		if(!_isBoundRectDirty){
			return;
		}
		_isBoundRectDirty	= false;
		Vector2 minPos = new Vector2();
		Vector2 maxPos = new Vector2();
		if(_texture){
			maxPos.x	= _texture.width;
			maxPos.y	= _texture.height;
		}else{
			minPos.x	= 999999;
			minPos.y	= 999999;
			maxPos.x	= -999999;
			maxPos.y	= -999999;
		}
		
		
		// loop though children to get the bound area of children
		
		DisplayObject child;
		
		for(int i=0;i<_childList.Count;i++){
			child	= _childList[i] as DisplayObject;
			
			if(child.boundRect.x<minPos.x){
				minPos.x	= child.boundRect.x;
			}
			if(child.boundRect.x+child.boundRect.width>maxPos.x){
				maxPos.x	= child.boundRect.x+child.boundRect.width;
			}
			if(child.boundRect.y<minPos.y){
				minPos.y	= child.boundRect.y;
			}
			if(child.boundRect.y+child.boundRect.height>maxPos.y){
				maxPos.y	= child.boundRect.y+child.boundRect.height;
			}
		}
		
		_originalWidth			= maxPos.x-minPos.x;
		_originalHeight			= maxPos.y-minPos.y;
		
		
		_selfBoundRect.x		= minPos.x;
		_selfBoundRect.y		= minPos.y;

		_selfBoundRect.width	= _originalWidth;
		_selfBoundRect.height	= _originalHeight;
		
		// get boundRect, boundRect is related with it's parent
		_boundRect				= _transform.getBoundRect(minPos,maxPos);
		_boundRectInRree		= _transformInTree.getBoundRect(minPos,maxPos);
		_width					= _boundRect.width;
		_height					= _boundRect.height;
		//Debug.Log(id+"/"+_boundRectInRree);
	}
	protected Rect _boundRectInTree  = new Rect();
	
	
	
	/***************************************
	 * hit test
	 ***************************************/
	override public bool hittest(Vector2 vec){
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		//Debug.Log(newVec);
		if(!_selfBoundRect.Contains(newVec)){
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
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		Debug.Log(id+"/"+ newVec + _selfBoundRect+_textureSelfRect);
		if(!_selfBoundRect.Contains(newVec)){
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
		if(isHit){
			Debug.Log(id+"/"+type);
			this.dispatchEvent(new MouseEvent(this,type,newVec,vec));
		}
		return isHit;
	}
	
	override public bool hitTestTouchDispatch(string type,Touch touch){
		Vector2 vec = new Vector2(touch.position.x,Stage.instance.stageHeight- touch.position.y);
		Vector2 newVec = transformInTreeInverted.transformVector(vec);
		
		if(!_selfBoundRect.Contains(newVec)){
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
			this.dispatchEvent(new TouchEvent(this,type,touch));
		}
		return isHit;
	}
	
}
