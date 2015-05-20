/**
 * 作者：有良策
 * 日期：2013/04/22
 * 功能：全局管理类，用来存储全局静态变量
 */
 
using UnityEngine;
using System.Collections;


public class GameManager
{
    public enum SelectModes
    {
        NOTHING = 0,
        INSERT = 1
    }

    public static SelectModes SLModes = SelectModes.NOTHING;
    public static UnityEngine.Object[] IconObjects;             // 存储后面需要加载的预制物对象
	public static string[] PrefabListNames;      				// 存储预制物名称作为按钮名称
    public static int CurrentObjType = 0;                       // 当前选择的对象类型
    public static Vector3 InsertPosition;                       // 图标的插入位置
    public static bool IsInsert = false;                        // 是否在场景中插入图标对象
    public static bool ShowInput = false;                       // 显示位置输入框
	public static bool IsSpawn = false;							// 对象是否实例化完成
    public static string ResourcesPath;                         // 资源路径
    public static string CurrentResource;                       // 当前需要加载进来的资源
    public static bool IsLoadDialogActive = false;              // 是否激活加载对话框
    public static Transform SelectedObject = null;              // 被选择的对象
	public static string InsertName = "";						// 插入的对象名称
}
