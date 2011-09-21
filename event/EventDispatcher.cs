using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDispatcher  {
	
	public string id = "";
	
	public delegate void CallBack(GuiEvent e);
	
	
	
	// Use this for initialization
	public EventDispatcher () {

		
	}
	protected List<EventListenTerm>		_listenList;
	protected static List<GuiEvent>		_dispatchList;
	
	public void addEventListner(string eventType,CallBack function){
		if(_listenList==null){
			_listenList		= new List<EventListenTerm>();
		}
		_listenList.Add(new EventListenTerm(eventType,function));
	}
	
	public void removeEventListner(string eventType,CallBack function){
		int i=0;
		if(i<_listenList.Count){
			EventListenTerm term = _listenList[i];
			if(term.eventType==eventType && term.function==function){
				_listenList.Remove(term);
			}else{
				i++;
			}
		}
	}
	
	public void dispatchEvent(GuiEvent e){
		if(_dispatchList==null){
			_dispatchList	= new List<GuiEvent>();
		}
		e.target	= this;
		_dispatchList.Add(e);
	}
	
	public static void sendEvents(){
		if(_dispatchList!=null){
			List<GuiEvent> temp	= new List<GuiEvent>(_dispatchList);
			_dispatchList.Clear();
			foreach(GuiEvent e in temp){
				if(e.target._listenList!=null){
					foreach(EventListenTerm listenTerm in e.target._listenList){
						if(listenTerm.eventType == e.eventType){
							listenTerm.function(e);
						}
					}
				}
			}
		}
	}
	
}
