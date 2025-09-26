using GameMaker;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BlackboardTag : MonoBehaviour
{
    public String mark = "@customData"; //key

    public String className = "PssOn00152.ConstData"; // value


    [Button]
    void TestGetClassObject()
    {
        //Object[] comp =GameObject.FindObjectsOfType(typeof(T));
        BlackboardTag[] bbTags = GameObject.FindObjectsOfType<BlackboardTag>();
        UnityEngine.Object[] comp1 = GameObject.FindObjectsOfType(Type.GetType(className));

        if (comp1.Length > 0)
        {
            DebugUtils.Log((comp1[0] as MonoBehaviour).name);
            //Type type = Type.GetType(comp[i].className)
            //Object[] comp =GameObject.FindObjectsOfType(typeof(T));
            //return FindVariable<object>(Type.GetType(comp[0].className), comp[0] );
        }
    }



    public static Variable<T> FindVariable<T>(Type tp, object obj, string path)
    {
        object currentObj = obj;
        Type currentType = tp;

        string[] fieldPath = path.Split('/');

        object parentObj = currentObj;
        foreach (var part in fieldPath)
        {
            parentObj = currentObj;
            PropertyInfo property = currentType.GetProperty(part);
            if (property != null) //属性
            {
                currentObj = property.GetValue(currentObj);
                currentType = property.PropertyType;
            }
            else // 字段
            {
                FieldInfo field = currentType.GetField(part);
                if (field != null)
                {
                    currentObj = field.GetValue(currentObj);
                    currentType = field.FieldType;
                }
                else
                {
                    DebugUtils.LogError($"[BlackboardTag] Invalid path part : {part}  at {currentType.Name}[{path}]");
                    return  null;
                }
            }
        }

        Variable<T> vVal = new Variable<T>(parentObj, fieldPath[fieldPath.Length - 1], null);

        DebugUtils.Log($"{currentType.Name}[{path}] = {vVal.value}");

        return vVal;
    }


    public Variable<T> GetClassOwerObject<T>(string path)
    {
        //Type type = Type.GetType(comp[i].className)
        //Object[] comp =GameObject.FindObjectsOfType(typeof(T));

        Type currentType = Type.GetType(className); //拿到类型
        object currentObj = this.gameObject.GetComponent(className); //拿到组件

        DebugUtils.Log($"game object = {(currentObj as MonoBehaviour).name}");

        return FindVariable<T>(currentType, currentObj, path);
    }

    [Button]
    public Variable<object> GetClassOwerObject (string path){
        return GetClassOwerObject<object>(path);
    }



    [Button]
    public static KeyValuePair<Type,object> GetBlackboard(string mark = "@cunstomData")
    {
        //Object[] comp =GameObject.FindObjectsOfType(typeof(T));
        BlackboardTag[] bbTags = GameObject.FindObjectsOfType<BlackboardTag>();

        for (int i=0; i<bbTags.Length;i++)
        {
            if (bbTags[i].mark == mark)
            {
                UnityEngine.Object[] comp1 = GameObject.FindObjectsOfType(Type.GetType(bbTags[i].className));

                if (comp1.Length > 0)
                {

                    MonoBehaviour mon = comp1[0] as MonoBehaviour;
                    //DebugUtil.Log(mon.name);
                    object currentObj = mon.gameObject.GetComponent(bbTags[i].className); //拿到组件
                    Type currentType = Type.GetType(bbTags[i].className); //拿到类型
                    return new KeyValuePair<Type, object>(currentType,currentObj) ;
                }
            }
        }
        return new KeyValuePair<Type, object>();
    }


    public static object GetBlackboard02(string mark = "@cunstomData")
    {
        //Object[] comp =GameObject.FindObjectsOfType(typeof(T));
        BlackboardTag[] bbTags = GameObject.FindObjectsOfType<BlackboardTag>();

        for (int i = 0; i < bbTags.Length; i++)
        {
            if (bbTags[i].mark == mark)
            {
                Type currentType = Type.GetType(bbTags[i].className);
                //DebugUtil.Log($"@1 mark = {mark} Name = {compType.Name}");
                UnityEngine.Object[] comp1 = GameObject.FindObjectsOfType(currentType);
                if (comp1.Length > 0)
                {
                    //DebugUtil.Log($"@2 mark = {mark}  className = {bbTags[i].className}");

                    MonoBehaviour mon = comp1[0] as MonoBehaviour;
                    //DebugUtil.Log(mon.name); 

                    //object currentObj1 = mon.gameObject.GetComponent(bbTags[i].className); //拿到组件(/这个打包后会报错 currentObj1为null)
                    //DebugUtil.Log($"@3 mark = {mark} {currentObj1}");  //这个打包后会报错 currentObj1为null

                    object currentObj = mon.gameObject.GetComponent(currentType); //拿到组件
                    //DebugUtil.Log($"@4 mark = {mark} {currentObj}");

                    //Type currentType = currentObj.GetType();
                    //Type currentType = Type.GetType(bbTags[i].className); //拿到类型

                    return currentObj;
                }
            }
        }
        return null;
    }




    [Button]
    void TestGet()
    {
        DebugUtils.Log($"line = {BlackboardUtils.GetValue<int>(null,"@customData/Line")}"); ;
    }
}
