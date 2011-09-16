using UnityEngine;
using System.Collections;

public class EventDispatcher  {
	
	public string id = "";
	
	public delegate void CallBack(GuiEvent e);
	
	
	
	// Use this for initialization
	public EventDispatcher () {

		
	}
	protected ArrayList	eventHandlerList;
	
	public void addEventListner(string eventType,CallBack function){
		if(eventHandlerList==null){
			eventHandlerList	= new ArrayList();
		}
		EventTerm eventTerm = new EventTerm(eventType,function);
		eventHandlerList.Add(eventTerm);
	}
	
	public void removeEventListner(string eventType,CallBack function){
		int i=0;
		if(i<eventHandlerList.Count){
			EventTerm term = eventHandlerList[i] as EventTerm;
			if(term.eventType==eventType && term.function==function){
				eventHandlerList.Remove(term);
			}else{
				i++;
			}
		}
	}
	
	public void dispatchEvent(GuiEvent e){
		if(eventHandlerList==null){
			return;
		}
		foreach(EventTerm term in eventHandlerList){
			if(term.eventType == e.eventType){
				term.function(e);
			}
		}
	}
}
