using UnityEngine;
using System.Collections;

public class SelectableItem :Sprite {

	// Use this for initialization
	
	public SelectableItem (Texture texture):base(texture){
		
	}
	public SelectableItem(Texture texture, float textureWidth,float textureHeight):base(texture,textureWidth,textureHeight){
	}
	public SelectableItem () {
	
	}
	
	public int listIndex	= -1;
	public SelectableList parentList;
	private bool _isSelected	= false;
	
	public bool isSelected{
		get {return _isSelected;}
	}
	public void select(){
		if(_isSelected){
			return;
		}
		_isSelected		= true;
		this.dispatchEvent(new GuiEvent(GuiEvent.SELECT));
	}
	
	
	public void unselect(){
		if(!_isSelected){
			return;
		}
		_isSelected		= false;
		this.dispatchEvent(new GuiEvent(GuiEvent.UNSELECT));
	}
}
