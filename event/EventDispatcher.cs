using UnityEngine;
using System.Collections;

public class EventDispatcher : Object {

	public delegate void EventHandlerFunction(GuiEvent e);
	// Use this for initialization
	public EventDispatcher () {
	
	}
	protected ArrayList	eventHandlerList	= new ArrayList();
	
	public void addEventListner(string eventType,EventHandlerFunction function){
		EventTerm eventTerm = new EventTerm(eventType,function);
		eventHandlerList.Add(eventTerm);
	}
	
	public void dispatchEvent(GuiEvent e){
		EventTerm term;
		for(int i=0;i<eventHandlerList.Count;i++){
			term	= eventHandlerList[i] as EventTerm;
			if(term.eventType == e.eventType){
				term.function(e);
			}
		}
	}
}
