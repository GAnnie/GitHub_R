/**
 * 作者：有良策
 * 日期：2013/04/23
 * 功能：XML资源路径的设置
 */
 
using UnityEngine;
using System.Collections;
using System.IO;

public class ResourceSetter : MonoBehaviour 
{	
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            GameManager.ResourcesPath = Application.dataPath + "/Xml/";
        }
		else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			GameManager.ResourcesPath = "Xml/";
		}
		
		
		if (GameManager.ResourcesPath != null)
		{
			(GetComponent("ListGUI") as ListGUI).enabled = true;
		}		
    }
}
