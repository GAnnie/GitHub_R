/**
 * 作者：有良策
 * 日期：201304/22
 * 功能：在指定位置实例化图标
 */
 
using UnityEngine;
using System.Collections;

public class InsertObject : MonoBehaviour 
{
    public Transform Root;

    private GameObject _currentGameObject;


    void FixedUpdate()
	{
        if (GameManager.IsInsert)
        {
		    int _counts = 0;	// 计数器，用来计算符合条件的对象数目
			
			if (Root.transform.childCount != 0)
	        {			
	            foreach (Transform child in Root)
	            {					
					if (child.tag == GameManager.CurrentObjType.ToString()
						&& child.gameObject.transform.position == GameManager.InsertPosition)
					{
						_counts++;					
					}
	            }
				
				if (_counts == 0)
				{
                    _currentGameObject = (GameObject)GameObject.Instantiate((GameObject)GameManager.IconObjects[GameManager.CurrentObjType],
            			GameManager.InsertPosition, Quaternion.identity);
					
					_currentGameObject.name = GameManager.InsertName;
                    _currentGameObject.transform.parent = Root;  
					GameManager.IsSpawn = true;
				}
	        }  
			else
			{
				_currentGameObject = (GameObject)GameObject.Instantiate((GameObject)GameManager.IconObjects[GameManager.CurrentObjType],
                	GameManager.InsertPosition, Quaternion.identity);
				
				_currentGameObject.name = GameManager.InsertName;
            	_currentGameObject.transform.parent = Root;
				GameManager.IsSpawn = true;
			}		  
        }
		
		GameManager.IsInsert = false;
	}
}
