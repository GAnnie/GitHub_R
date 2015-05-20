/**
 * 作者：有良策
 * 日期：201304/22
 * 功能：图标列表，展示列表项及相应操作
 */
 
using UnityEngine;
using System.Collections;
using System.IO;


public class ListGUI : MonoBehaviour 
{
	public GameObject[] PrefabList;			// 存储预制物 
    public Transform Root;                  // 实例化对象的根节点
	
	private Texture2D[] _buttonImgs;        // 存储按钮图片(图片要PNG格式)
	private WWW _download;
    private bool _isChangeMode = false;     // 是否改变了选择模式
	private bool _isRefreshDone = true;		// 是否刷新完成

	
	void Awake()
	{
		this.enabled = false;

        // 加载所有预制对象到内存
        GameManager.IconObjects = PrefabList;
	}
	
	IEnumerator Start() 
	{
		GameManager.CurrentObjType = 0;
        GameManager.PrefabListNames = new string[PrefabList.Length];
		_buttonImgs = new Texture2D[PrefabList.Length];
		
		// 将预制物名称存储到全局管理数组中
		for (int i = 0; i < PrefabList.Length; i++)
		{
			GameManager.PrefabListNames[i] = PrefabList[i].name;	
			
			// 启动时加载定位点	
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
	        {
	            GameManager.CurrentResource = GameManager.ResourcesPath + PrefabList[i].name + ".xml";
	        }
			else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
			{
				_download = new WWW(GameManager.ResourcesPath + PrefabList[i].name + ".xml");
				yield return _download;
				GameManager.CurrentResource = _download.text;
			}				
			
			if (GameManager.CurrentResource != "")
            {
                (GetComponent("ResourceLoad") as ResourceLoad).LoadResource();
            }
		}   				
		
		_download = null;
		
		// 加载图标
		yield return StartCoroutine(Refresh());		     
	}
	
	void OnGUI()
	{
        // 打开图标类型列表
        if (GUI.Button(new Rect(15, 15, 60, 30), "新建"))
        {
            _isChangeMode = !_isChangeMode;
			GameManager.ShowInput = false;
			ListControls.Reset();
			
            if (_isChangeMode)
            {
                GameManager.SLModes = GameManager.SelectModes.INSERT;
            }
            else
            {
                GameManager.SLModes = GameManager.SelectModes.NOTHING;
            }
        }

        // 显示图标类型列表
        if (GameManager.SLModes == GameManager.SelectModes.INSERT)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 200, 330), 
                "图标类型", GUI.skin.box);
			
			if (GameManager.ShowInput == false)
			{
				ListControls.IconListBox(new Rect(5, 25, 190, 270), GameManager.PrefabListNames, _buttonImgs);
			}  
			else
			{
				// 显示坐标输入框
				ListControls.ShowInputBox(new Rect(5, 25, 190, 270));
			}
            
			GUILayout.Space(300);
			
			if (GUILayout.Button("刷新"))
			{
				if (_isRefreshDone)
				{
					StartCoroutine(Refresh());
					_isRefreshDone = false;
				}
				
			}
			
            GUILayout.EndArea();
        }

		// 对象实例化完成则进行保存
		if (GameManager.IsSpawn)
		{
			GameManager.CurrentResource =  GameManager.ResourcesPath + GameManager.PrefabListNames[GameManager.CurrentObjType] + ".xml";
       	    (GetComponent("ResourceLoad") as ResourceLoad).SaveResource();
			GameManager.IsSpawn = false;
		}  
	}
	
	// 刷新图标信息
	IEnumerator Refresh()
	{
		for (int i = 0; i < PrefabList.Length; i++)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				_download = new WWW("file:///" + Application.dataPath + "/Picture/" + PrefabList[i].name + ".png");
			}
			else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
			{
				_download = new WWW("Picture/" + PrefabList[i].name + ".png");
			}
			
			yield return _download;
			
//			if (_download.error != null)
//			{
//				if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
//				{
//					_download = new WWW("file:///" + Application.dataPath + "/Picture/Default.png");
//				}
//				else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
//				{
//					_download = new WWW("Picture/Default.png");
//				}
//			}
//			
//			yield return _download;
			
			_buttonImgs[i] = _download.texture as Texture2D;			
		}
		
		_isRefreshDone = true;
		_download = null;
	}
}