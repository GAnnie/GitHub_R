/**
 * 作者：有良策
 * 日期：2013/04/24
 * 功能：选择对象（删除）
 */
 
using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour 
{
    private RaycastHit _selectionHitInfo;
	private Transform _lastObj;		       // 上次选中的对象
	
	
	void Start()
	{
		_lastObj = gameObject.transform;
	}
	
	void FixedUpdate()
	{   		
        Ray selectionRay = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(selectionRay, out _selectionHitInfo))
        {
            if (_selectionHitInfo.transform != null)
            {
				GameManager.SelectedObject = _selectionHitInfo.transform;
				_selectionHitInfo.transform.renderer.material.color = Color.red;
										
				if (_lastObj != null && _selectionHitInfo.transform.name != _lastObj.name)
				{
					_lastObj.renderer.material.color = Color.white;			
				}  
				
				_lastObj = _selectionHitInfo.transform;
            }	
			else
			{
				GameManager.SelectedObject = null;
			}
        }
        else
        {
			if (_lastObj != null)
			{
				_lastObj.renderer.material.color = Color.white;       
			}
        }

		// 删除选择的对象
		if (GameManager.SelectedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
				GameManager.CurrentObjType = System.Int32.Parse(GameManager.SelectedObject.tag);
				
                Destroy(GameManager.SelectedObject.gameObject);
                GameManager.SelectedObject = null;
				
				Debug.Log(GameManager.CurrentObjType);								
            }
        } 
		
		// 删除的对象从XML文件中移除
		if (Input.GetKeyUp(KeyCode.Delete))
		{			
			GameManager.CurrentResource = GameManager.ResourcesPath 
				+ GameManager.PrefabListNames[GameManager.CurrentObjType] + ".xml";
   	    	(GetComponent("ResourceLoad") as ResourceLoad).SaveResource();	
		}
	}
}