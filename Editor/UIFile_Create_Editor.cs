using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UIFile_Create_Editor : EditorWindow
{
    [MenuItem("Tool/UI/Create")]
    public static void Create_UIFile()
    {
        GetWindow<UIFile_Create_Editor>().Show();
    }

    string ui_Name;

    private string[] layerArray = 
    {
        "backGround", "window", "tips", "systemOpen", "guide", "Loading", "http"
    };
	int layerIndex;

    private void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("UI -- 功能名称");
        ui_Name = EditorGUILayout.TextField(ui_Name);
        EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.BeginHorizontal();
        GUILayout.Label("UI -- 默认层级");
        layerIndex = EditorGUILayout.Popup(layerIndex,layerArray);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("创建功能文件"))
        {
            if (ui_Name == String.Empty)
            {
                Debug.Log("请输入名称!");
                return;
            }
            //XXXConfig.lua
            string config_Code_Path = $"{Application.dataPath}/Script/Lua/UI/{ui_Name}/{ui_Name}Config.lua";
            if (!Directory.Exists(Path.GetDirectoryName(config_Code_Path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(config_Code_Path));
            }

            string layerStr = layerArray[layerIndex];
            StreamWriter sw_Config = new StreamWriter(config_Code_Path);
            sw_Config.WriteLine($"local UI{ui_Name}Config = {{");
            sw_Config.WriteLine($"\ttype = UITypeEnum.{ui_Name},");
            sw_Config.WriteLine($"\tprefabName = \"UI_Window_{ui_Name}\",");
            sw_Config.WriteLine($"\tlayer = UILayer.{layerStr},");
            sw_Config.WriteLine($"\tcode_Model = require(\"UI/{ui_Name}/Model/{ui_Name}Model\"),");
            sw_Config.WriteLine($"\tcode_View = require(\"UI/{ui_Name}/View/{ui_Name}View\"),");
            sw_Config.WriteLine($"\tcode_Controll = require(\"UI/{ui_Name}/Controll/{ui_Name}Controll\"),");
            sw_Config.WriteLine($"}}");
            sw_Config.WriteLine($"return UI{ui_Name}Config");
            sw_Config.Flush();
           
            //XXXModel.lua
            string model_Code_Path = $"{Application.dataPath}/Script/Lua/UI/{ui_Name}/Model/{ui_Name}Model.lua";
            if (!Directory.Exists(Path.GetDirectoryName(model_Code_Path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(model_Code_Path));
            }
            StreamWriter sw_Model = new StreamWriter(model_Code_Path);
            sw_Model.WriteLine($"local {ui_Name}Model = BaseClass(\"{ui_Name}Model\")");
            sw_Model.WriteLine($"");
            sw_Model.WriteLine($"function {ui_Name}Model:__init()");
            sw_Model.WriteLine($"\tself:AddListener()");
            sw_Model.WriteLine($"end");
			sw_Model.WriteLine($"function {ui_Name}Model:AddListener()");
            sw_Model.WriteLine($"");
            sw_Model.WriteLine($"end");
            sw_Model.WriteLine($"");
			sw_Model.WriteLine($"function {ui_Name}Model:RemoveListener()");
            sw_Model.WriteLine($"");
            sw_Model.WriteLine($"end");
			sw_Model.WriteLine($"");
            sw_Model.WriteLine($"return {ui_Name}Model");
            sw_Model.Flush();

            //XXXView.lua
            string view_Code_Path = $"{Application.dataPath}/Script/Lua/UI/{ui_Name}/View/{ui_Name}View.lua";
            if (!Directory.Exists(Path.GetDirectoryName(view_Code_Path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(view_Code_Path));
            }
            StreamWriter sw_View = new StreamWriter(view_Code_Path);
            sw_View.WriteLine($"local {ui_Name}View = BaseClass(\"{ui_Name}View\")");
            sw_View.WriteLine($"");
            sw_View.WriteLine($"function {ui_Name}View:__init(prefab)");
			sw_View.WriteLine($"\tself.gameObject = prefab");
            sw_View.WriteLine($"\tself:InitUIData()"); 
			sw_View.WriteLine($"\tself:BindUIEvent()");
            sw_View.WriteLine($"end");
            sw_View.WriteLine($"");
			sw_View.WriteLine($"function {ui_Name}View:InitUIData()");
            sw_View.WriteLine($"");
            sw_View.WriteLine($"end");
			sw_View.WriteLine($"function {ui_Name}View:BindUIEvent()");
            sw_View.WriteLine($"");
            sw_View.WriteLine($"end");
            sw_View.WriteLine($"function {ui_Name}View:OnEnable()");
            sw_View.WriteLine($"");
            sw_View.WriteLine($"end");
            sw_View.WriteLine($"");
            sw_View.WriteLine($"return {ui_Name}View");
            sw_View.Flush();

            //XXXControll.lua
            string controll_Code_Path = $"{Application.dataPath}/Script/Lua/UI/{ui_Name}/Controll/{ui_Name}Controll.lua";
            if (!Directory.Exists(Path.GetDirectoryName(controll_Code_Path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(controll_Code_Path));
            }
            StreamWriter sw_Controll = new StreamWriter(controll_Code_Path);
            sw_Controll.WriteLine($"local {ui_Name}Controll = BaseClass(\"{ui_Name}Controll\")");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"function {ui_Name}Controll:__init()");
            sw_Controll.WriteLine($"\tself:AddListener()");
            sw_Controll.WriteLine($"end");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"function {ui_Name}Controll:AddListener()");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"end");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"function {ui_Name}Controll:RemoveListener()");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"end");
            sw_Controll.WriteLine($"");
            sw_Controll.WriteLine($"return {ui_Name}Controll");
            sw_Controll.Flush();

            string component_Code_Path = $"{Application.dataPath}/Script/Lua/UI/{ui_Name}/Component";
            if (!Directory.Exists(component_Code_Path))
            {
                Directory.CreateDirectory(component_Code_Path);
            }
        }

    }
}
