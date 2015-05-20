/**
 * 作者：有良策
 * 日期：201304/22
 * 功能：这个类是负责实际加载外部xml文件和保存
 */

using UnityEngine;
using System.Collections;
using System.Xml;


public class ResourceLoad : MonoBehaviour 
{
    public Transform Root;                              // 父对象

    private XmlDocument _xmlDoc = new XmlDocument();    // xml文档
    private XmlNode _rootNode;                          // 根节点
    private XmlNode _currentNode;                       // 当前节点
    private GameObject _currentGameObject;

	
    // 加载XML资源
	public void LoadResource()
	{
        _xmlDoc = new XmlDocument();

        // 检查准备加载的内容是否存在
        if (GameManager.CurrentResource != "")
        {
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
	        {
	            _xmlDoc.Load(GameManager.CurrentResource);
	        }
			else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
			{
				_xmlDoc.LoadXml(GameManager.CurrentResource);  // 加载已存在的资源
			}
			
            _rootNode = _xmlDoc.FirstChild;             // 获取根节点

            if (_rootNode.Name == "res")
            {             
                // 如果文件头有任何子元素, 一并处理它们
                if (_rootNode.ChildNodes.Count != 0)
                {
                    int[] currentObjType = new int[_rootNode.ChildNodes.Count];  // 子节点数目
					string[] currentObjName = new string[_rootNode.ChildNodes.Count];
                    Vector3 posVector = Vector3.zero;
                    Quaternion rotQuat = Quaternion.identity;
                    Vector3 rotVector = Vector3.zero;

                    // 遍历子节点进行实例化
                    for (int i = 0; i < _rootNode.ChildNodes.Count; i++)
                    {
                        if (_rootNode.ChildNodes[i].Name == "actor")
                        {
							// 检查对象类型, 类型的编号应该匹配收集到的预制物
                            if (_rootNode.ChildNodes[i].Attributes[0].Name == "type")
                            {
                                if (System.Int32.TryParse(_rootNode.ChildNodes[i].Attributes[0].Value.ToString(), out currentObjType[i]))
                                {
                                    currentObjType[i] = System.Int32.Parse(_rootNode.ChildNodes[i].Attributes[0].Value.ToString());
                                }
                            }
                            else
                            {
                                // 如果没有对象类型, 或以某种方式无效的对象类型, 随机选择一个
                                currentObjType[i] = (int)Random.Range(0, 10);
                            }
							
							// name
							if (_rootNode.ChildNodes[i].Attributes[1].Name == "name")
							{
								if (_rootNode.ChildNodes[i].Attributes[1].Value.ToString() != "")
                                {
                                    currentObjName[i] = _rootNode.ChildNodes[i].Attributes[1].Value.ToString();
                                }
							}
							else
							{
								currentObjName[i] = "未命名";
							}

                            // 这下三段代码检索对象的位置, 如果检索的数据是无效的, 设置它为0
                            if (_rootNode.ChildNodes[i].Attributes[2].Name == "positionX")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[2].Value.ToString(), out posVector.x))
                                {
                                    posVector.x = float.Parse(_rootNode.ChildNodes[i].Attributes[2].Value.ToString());
                                }
                            }
                            else
                            {
                                posVector.x = 0F;
                            }

                            if (_rootNode.ChildNodes[i].Attributes[3].Name == "positionY")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[3].Value.ToString(), out posVector.y))
                                {
                                    posVector.y = float.Parse(_rootNode.ChildNodes[i].Attributes[3].Value.ToString());
                                }
                            }
                            else
                            {
                                posVector.y = 0F;
                            }

                            if (_rootNode.ChildNodes[i].Attributes[4].Name == "positionZ")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[4].Value.ToString(), out posVector.z))
                                {
                                    posVector.z = float.Parse(_rootNode.ChildNodes[i].Attributes[4].Value.ToString());
                                }
                            }
                            else
                            {
                                posVector.z = 0F;
                            }


                            // 旋转, 如果检索的数据是无效的设置它没有旋转
                            if (_rootNode.ChildNodes[i].Attributes[5].Name == "rotationX")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[5].Value.ToString(), out rotVector.x))
                                {
                                    rotVector.x = float.Parse(_rootNode.ChildNodes[i].Attributes[5].Value.ToString());
                                }
                            }
                            else
                            {
                                rotVector.x = 0F;
                            }

                            if (_rootNode.ChildNodes[i].Attributes[6].Name == "rotationY")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[6].Value.ToString(), out rotVector.y))
                                {
                                    rotVector.y = float.Parse(_rootNode.ChildNodes[i].Attributes[6].Value.ToString());
                                }
                            }
                            else
                            {
                                rotVector.y = 0F;
                            }

                            if (_rootNode.ChildNodes[i].Attributes[7].Name == "rotationZ")
                            {
                                if (float.TryParse(_rootNode.ChildNodes[i].Attributes[7].Value.ToString(), out rotVector.z))
                                {
                                    rotVector.z = float.Parse(_rootNode.ChildNodes[i].Attributes[7].Value.ToString());
                                }
                            }
                            else
                            {
                                rotVector.z = 0F;
                            }

                            rotQuat.eulerAngles = rotVector;
								
							GameObject go = (GameObject)GameManager.IconObjects[currentObjType[i]];
							int counts = 0;
							
							if (Root.transform.childCount != 0)
							{																
								// 在根节点遍历所有对象和创建节点元素与信息
								foreach (Transform child in Root)
						        {					
									if (child.transform.position == posVector)
									{
										counts++;
									}																	
								}
		
								if (counts == 0)
								{
									// 一旦得到所有需要的数据, 实例化对象
				                    _currentGameObject = (GameObject)GameObject.Instantiate(go, posVector, rotQuat);
									_currentGameObject.name = currentObjName[i];
				                    _currentGameObject.transform.parent = Root; 
								}
							}
							else
							{
								_currentGameObject = (GameObject)GameObject.Instantiate(go, posVector, rotQuat);
								_currentGameObject.name = currentObjName[i];
					            _currentGameObject.transform.parent = Root; 
							}
                        }
                    }
                }
            }
        }
	}

    // 保存数据到XML
    public void SaveResource()
    {	
        // 创建头文件
        _xmlDoc = new XmlDocument();
        _xmlDoc.LoadXml("<res></res>");

        XmlNode docRoot = _xmlDoc.DocumentElement;

		if (Root.transform.childCount != 0)
		{
			// 在根节点遍历所有对象和创建节点元素与信息
			foreach (Transform child in Root)
	        {
				if (GameManager.CurrentObjType.ToString() == child.tag)
				{
					XmlElement elem = _xmlDoc.CreateElement("actor");
					
					// 创建节点属性
			        XmlAttribute typeAttr = _xmlDoc.CreateAttribute("type");
					XmlAttribute nameAttr = _xmlDoc.CreateAttribute("name");
			        XmlAttribute posXAttr = _xmlDoc.CreateAttribute("positionX");
			        XmlAttribute posYAttr = _xmlDoc.CreateAttribute("positionY");
			        XmlAttribute posZAttr = _xmlDoc.CreateAttribute("positionZ");
			        XmlAttribute rotXAttr = _xmlDoc.CreateAttribute("rotationX");
			        XmlAttribute rotYAttr = _xmlDoc.CreateAttribute("rotationY");
			        XmlAttribute rotZAttr = _xmlDoc.CreateAttribute("rotationZ");
					
					// 为节点属性赋值
			        typeAttr.Value = child.tag;                                     // 节点类型值为节点对象的标签值
					nameAttr.Value = child.name;
			        posXAttr.Value = child.position.x.ToString();
			        posYAttr.Value = child.position.y.ToString();
			        posZAttr.Value = child.position.z.ToString();
			        rotXAttr.Value = child.rotation.eulerAngles.x.ToString();
			        rotYAttr.Value = child.rotation.eulerAngles.y.ToString();
			        rotZAttr.Value = child.rotation.eulerAngles.z.ToString();
					
					// 将节点属性添加到节点元素中
			        elem.Attributes.Append(typeAttr);
					elem.Attributes.Append(nameAttr);
			        elem.Attributes.Append(posXAttr);
			        elem.Attributes.Append(posYAttr);
			        elem.Attributes.Append(posZAttr);
			        elem.Attributes.Append(rotXAttr);
			        elem.Attributes.Append(rotYAttr);
			        elem.Attributes.Append(rotZAttr);
			        docRoot.AppendChild(elem);
				}
	        }
		}    

        // 最后检查xml文件夹实际上是否存在的, 如果是则将生成的xml数据追加到该文件路径中；否则保存成一个新文件
        if (GameManager.CurrentResource != "")
        {
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
	        {
				Debug.Log(GameManager.CurrentResource);
	            _xmlDoc.Save(GameManager.CurrentResource);
	        }
			else if (Application.platform == RuntimePlatform.WindowsWebPlayer)
			{
				_xmlDoc.Save(GameManager.CurrentResource);
			}          
        }
    }
	
	void OnGUI()
	{
		GUI.Label(new Rect(0, 50, 800, 200), GameManager.ResourcesPath + GameManager.PrefabListNames[GameManager.CurrentObjType] + ".xml");
		GUI.Label(new Rect(0, 250, 800, 200), GameManager.CurrentResource);
	}
}
