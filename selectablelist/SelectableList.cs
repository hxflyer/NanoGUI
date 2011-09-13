using UnityEngine;
using System.Collections;

public class SelectableList :SelectableItem {

	protected ArrayList	_itemAry	= new ArrayList();
	
	public SelectableList () {
	
	}
	
	public void addItem(SelectableItem item){
		item.listIndex	= _itemAry.Count;
		item.parentList	= this;
		_itemAry.Add(item);
		addChild(item);
	}
	public void addItemAt(SelectableItem item,int index){
		item.listIndex	= index;
		item.parentList	= this;
		_itemAry.Insert(index,item);
		addChild(item);
	}
	
	public void removeItem(SelectableItem item){
		int itemIndex	= _itemAry.IndexOf(item);
		item.listIndex	= -1;
		_itemAry.Remove(item);
		removeChild(item);
		item.parentList	= null;
		for(int i=itemIndex;i<_itemAry.Count;i++){
			(_itemAry[i] as SelectableItem).listIndex = i;
		}
		
	}
	
	public void removeItemAt(int itemIndex){
		
		_itemAry.RemoveAt(itemIndex);
		for(int i=itemIndex;i<_itemAry.Count;i++){
			(_itemAry[i] as SelectableItem).listIndex = i;
		}
	}
	
	protected SelectableItem _selectedItem;
	
	public SelectableItem selectedItem{
		get {return _selectedItem;}
	}
	
	public void selectItem(SelectableItem item){
		if(_selectedItem==item){
			return;
		}
		
		if(_selectedItem!=null){
			_selectedItem.unselect();
		}
		_selectedItem	= item;
		_selectedItem.select();
		this.dispatchEvent(new GuiEvent(this,GuiEvent.CHANGE));
	}
	
	public void unselectItem(){
		if(_selectedItem!=null){
			_selectedItem.unselect();
			_selectedItem	= null;
			this.dispatchEvent(new GuiEvent(this,GuiEvent.CHANGE));
		}
	}
}
