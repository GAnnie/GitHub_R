/**
 * 作者：有良策
 * 日期：201304/22
 * 功能：图标列表，展示列表项及相应操作的管理
 */
 
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class ListControls
{
	private static string _insertPosX = "0";        // 插入位置
    private static string _insertPosY = "0";        // 插入位置
    private static string _insertPosZ = "0";        // 插入位置
	
    private static float _scrollbarValue = 0;       // 滚动条的阀值
    private static float _itemHeight = 30;          // 每个列表项的高度
    private static int _scrollbarItems = 0;         // 当列表项数达一定数目出现滚动条


    // 图标类型列表
    public static void IconListBox(Rect dimensions, string[] listContents, Texture2D[] btnImages)
    {
        GUILayout.BeginArea(dimensions, "", GUI.skin.box);
        _scrollbarItems = (int)dimensions.height / (int)_itemHeight;

        if (listContents.Length > _scrollbarItems)
        {
            _scrollbarValue = GUI.VerticalScrollbar(new Rect(dimensions.width - 20, 2, 80, dimensions.height - 5), 
                _scrollbarValue, 1, 0, (listContents.Length * _itemHeight) + 10);
        }
        else
        {
            _scrollbarValue = 0;
        }

        if (listContents != null)
        {     
            for (int i = 0; i < listContents.Length; i++)
            {
                //在这里插入图标
                GUI.Label(new Rect(15, (0 + i) * _itemHeight, 60, _itemHeight + 5), btnImages[i]);

                if (GUI.Button(new Rect(60, (0 + i) * _itemHeight, dimensions.width - 100, _itemHeight), listContents[i]))
                {
                    GameManager.CurrentObjType = i;
                    GameManager.ShowInput = true;
                }
            }
        }

        GUILayout.EndArea();
    }

    // 位置输入框
    public static void ShowInputBox(Rect dimensions)
    {
        GUI.BeginGroup(dimensions, GUI.skin.box);
		
		GUILayout.Label("位置坐标： ");
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(30);
		GUILayout.Label("X");
		GUILayout.Label("Y");
		GUILayout.Label("Z");
		GUILayout.EndHorizontal();
		
        GUILayout.BeginHorizontal();
		GUILayout.Space(5);
        _insertPosX = GUILayout.TextField(_insertPosX, GUILayout.Width(55));
        _insertPosY = GUILayout.TextField(_insertPosY, GUILayout.Width(55));
        _insertPosZ = GUILayout.TextField(_insertPosZ, GUILayout.Width(55));

        if (IsMatchString(_insertPosX))
        {
            GameManager.InsertPosition.x = float.Parse(_insertPosX);
        }

        if (IsMatchString(_insertPosY))
        {
            GameManager.InsertPosition.y = float.Parse(_insertPosY);
        }
        
        if (IsMatchString(_insertPosZ))
        {
            GameManager.InsertPosition.z = float.Parse(_insertPosZ);
        }

        GUILayout.EndHorizontal();
		
		// 保证输入的数字是 浮点数
		if (!IsMatchString(_insertPosX) || !IsMatchString(_insertPosY) || !IsMatchString(_insertPosZ))
		{
			GUILayout.Label("请确保您输入的是数字... :)");
		}
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("请输入名称：", GUILayout.Width(80));
		GameManager.InsertName = GUILayout.TextField(GameManager.InsertName, GUILayout.Width(95));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("取消", GUILayout.Width(50)))
        {
            GameManager.ShowInput = false;
			Reset();
        }
		
		GUILayout.Space(dimensions.width - 115);
		
		// 确定即保存
		if (GUILayout.Button("确定", GUILayout.Width(50)))
        {      		
			// 判断三个坐标输入框内容是否为浮点数
			if (IsMatchString(_insertPosX) && IsMatchString(_insertPosY) && IsMatchString(_insertPosZ)
				&& GameManager.InsertName != "")
			{
				GameManager.IsInsert = true;          
			}
			else
			{
				GameManager.IsInsert = false;
			}
        }
		
		GUILayout.EndHorizontal();
		
        GUI.EndGroup();
    }

    // 匹配浮点数
    public static bool IsMatchString(string s)
    {
        string r = @"^[-+]?[0-9]*\.?[0-9]+$";

        return Regex.IsMatch(s, r);
    }
	
	// 重置
	public static void Reset()
	{
		_insertPosX = "0";
		_insertPosY = "0";
		_insertPosZ = "0";
		GameManager.InsertName = "";
	}
}
