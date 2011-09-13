using UnityEngine;
using System.Collections;

public class SelectableItem :Sprite {

	// Use this for initialization
	public SelectableItem () {
	
	}
	
	public int listIndex	= -1;
	public SelectableList parentList;
	private bool _isSelected	= false;
	
	public void select(){
		if(_isSelected){
			return;
		}
		_isSelected		= true;
		this.dispatchEvent(new GuiEvent(this,GuiEvent.SELECT));
	}
	
	
	public void unselect(){
		if(!_isSelected){
			return;
		}
		_isSelected		= false;
		this.dispatchEvent(new GuiEvent(this,GuiEvent.UNSELECT));
	}
}
