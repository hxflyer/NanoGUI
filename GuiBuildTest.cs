using UnityEngine;
using System.Collections;

public class GuiBuildTest : MonoBehaviour {
	
	
	private MainMenu _mainmMenu;
	private SubMenu _subMenu;
	
	private ArrayList _pageAry	= new ArrayList();
	private Page _lastPage;
	
	void Start () {

		Texture2D texture = Resources.Load("testimg",typeof(Texture2D)) as Texture2D;
		
		_mainmMenu	= new MainMenu();
		GuiManager.stage.addChild(_mainmMenu);
		_mainmMenu.y= 685;
		_mainmMenu.addEventListner(GuiEvent.CHANGE,new EventDispatcher.CallBack(mainMenuChangeHandler));
		
		_subMenu	= new SubMenu();
		GuiManager.stage.addChildAt(0,_subMenu);
		_subMenu.y	= 685;
		_subMenu.addEventListner(GuiEvent.CHANGE,new EventDispatcher.CallBack(subMenuChangeHandler));
		
		//hard code 3 pages
		InteractImagePage page1	= new InteractImagePage(Resources.Load("page/compare",typeof(Texture2D)) as Texture2D);
		InteractImagePage page2	= new InteractImagePage(Resources.Load("page/time_bg",typeof(Texture2D)) as Texture2D);
		InteractImagePage page3	= new InteractImagePage(Resources.Load("page/contentPage_bg",typeof(Texture2D)) as Texture2D);
		page3.buildHardCodeItem();
		_pageAry.Add(page1);
		_pageAry.Add(page2);
		_pageAry.Add(page3);
	}
	
	void mainMenuChangeHandler(GuiEvent e){
		Debug.Log("menu changed");
		if(_mainmMenu.selectedItem!=null && _mainmMenu.selectedItem.listIndex==1){
			NanoTween.to(_subMenu,0.3f,NanoTween.Pack("y",643f,"ease",Ease.easeOutExpo));
		}else{
			_subMenu.unselectItem();
			NanoTween.to(_subMenu,0.3f,NanoTween.Pack("y",685f,"ease",Ease.easeOutExpo));
			hidePage();
		}
	}
	
	void subMenuChangeHandler(GuiEvent e){
		Debug.Log("sub menu changed");
		if(_lastPage!=null){
			_lastPage.hide();
		}

		if(_subMenu.selectedItem!=null){
			if(_lastPage==null){
				Debug.Log(GameObject.Find("Code").GetComponent("SceneScript"));
				(GameObject.Find("Code").GetComponent("SceneScript") as SceneScript).hideScene();
			}
			_lastPage	= _pageAry[_subMenu.selectedItem.listIndex] as Page;
			_lastPage.show();
			
		}
		
	}
	
	void hidePage(){
		if(_lastPage!=null){
			_lastPage.hide();
			_lastPage= null;
		}
		(GameObject.Find("Code").GetComponent("SceneScript") as SceneScript).showScene();
	}
	
	
	void img2MouseDown(GuiEvent e){
		Debug.Log("img 2 clicked");
		NanoTween.to(e.target,1,NanoTween.Pack("delay",0.2f,
                             "x",0.0f,
                             "scaleX",1.5f,
                             "alpha",1.0f,
                             "ease",Ease.easeOutExpo,
                             "onComplete",new NanoTween.CallBack(imgComplete),
                             "onStart",new NanoTween.CallBack(imgStart),
                             "onUpdate",new NanoTween.CallBack(imgUpdate),
                             "onUpdateParams",NanoTween.Pack(e.target)));
	}
	
	
	
	void img2TouchBegan(GuiEvent e){
		Debug.Log("img2TouchBegan");
	}
	void img2TouchEnd(GuiEvent e){
		Debug.Log("img2TouchEnd");
	}
	void img2TouchMove(GuiEvent e){
		Debug.Log("img2TouchMove");
	}
	void imgComplete(object[] args){
		Debug.Log("complete");
	}
	void imgStart(object[] args){
		Debug.Log("start");
	}
	void imgUpdate(object[] args){
		Debug.Log("update");	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(bigimg.textureRenderRect);
		//bigimg.rotation++;
	}
 
}
