using PssOn00152;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


namespace GameMaker
{

    public static class BlackboardUtils
    {
        private static bool IsErrorPath(string path)
        {
            if (path.StartsWith("@") || path.StartsWith("./") || path.StartsWith("/"))
                return false;
            return true;
        }

        public static void SetValue<T>(object blackboard, string path, T value)
        {
            Variable<T> vVal = FindVariable<T>(blackboard, path);
            vVal.value = value;
        }
        public static void SetValue<T>(string path, T value)
        {
            if (IsErrorPath(path))
                DebugUtils.LogError($"path format error : {path}");
            
            SetValue<T>(null, path, value);
        }

        public static void SetValue<T>(string path, object value)
        {
            if (IsErrorPath(path))
                DebugUtils.LogError($"path format error : {path}");

            SetValue<T>(null, path, (T)value);
        }


        public static T GetValue<T>(object blackboard, string path)
        {
            Variable<T> vVal = FindVariable<T>(blackboard, path);
            return vVal.value;
        }
        public static T GetValue<T>(string path)
        {
            if (IsErrorPath(path))
                DebugUtils.LogError($"path format error : {path}");
            return GetValue<T>(null, path);
        }

        public static Variable<T> FindVariable<T>(string path)
        {
            if (IsErrorPath(path))
                DebugUtils.LogError($"path format error : {path}");
            return FindVariable<T>(null, path);
        }

        /// <summary>
        /// 子类、基类pubilc的方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object Invoke(string path, object[] parameters)
        {
            return Invoke(null, path, parameters);
        }
        public static object Invoke(object blackboard, string path, object[] parameters)
        {
            string pth = path;
            object currentObj = blackboard;
            
            if (pth.StartsWith("/"))
            {
                currentObj = MainBlackboard.Instance;
                pth = pth.Substring(1);
            }
            else if (pth.StartsWith("./"))
            {
                // "@contentBlackboard"
                // "@content"

                /*
                currentObj = BlackboardTag.GetBlackboard02("@content");
                pth = pth.Replace("./", "");
                */
                currentObj = ContentBlackboard.Instance;
                pth = pth.Substring(2);
            }
            else if (pth.StartsWith("@"))
            {
                //@turn
                //@cunstomData

                List<string> lst = new List<string>(pth.Split('/'));
                currentObj = BlackboardTag.GetBlackboard02(lst[0]);
                lst.RemoveAt(0);
                pth = String.Join("/", lst);
            }

            if (currentObj == null)
            {
                DebugUtils.LogError($"blackboard is null in {path}");
                return null;
            }

            string[] fieldPath = pth.Split('/');

            if (fieldPath.Length == 0) {

                DebugUtils.LogError($"cant not find methon name  in {path}");
                return null;
            }

            if (string.IsNullOrEmpty(fieldPath[0]))
            {
                DebugUtils.LogError($"cant not find methon name : {fieldPath[0]} in{path}");
                return null;
            }

            Type currentType = currentObj.GetType();
            MethodInfo methodInfo = currentType.GetMethod(fieldPath[0]); //只能是public的方法

            //MethodInfo methodInfo = currentType.GetMethod(fieldPath[0],BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //MethodInfo methodInfo = currentType.GetMethod(fieldPath[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy| BindingFlags.Instance);
            //  BindingFlags.Instance : 非静态成员（即实例成员）
            // BindingFlags.FlattenHierarchy：
            /*if (methodInfo == null)
            {
                MethodInfo[] myMethodInfos = currentType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                methodInfo = Array.Find(myMethodInfos, m => m.Name == "MyMethod");

                //methodInfo = currentType.GetMethod(fieldPath[0],BindingFlags.NonPublic | BindingFlags.Instance);
            }*/


            //methodInfo.Invoke(currentObj, new object[] { "Hello World" });
            return methodInfo.Invoke(currentObj, parameters);
        }


        public static bool IsBlackboard(string path)
        {
            string pth = path;

            if (string.IsNullOrEmpty(pth))
                return false;

            if (path == "/")
            {
                return MainBlackboard.Instance != null;
            }
            else if (pth == "./")
            {
                return BlackboardTag.GetBlackboard02("@content") != null;
            }
            else if (pth.StartsWith("@") && pth.EndsWith("/") && 1 == pth.Count(c => c == '/'))  // @turn/
            {
                List<string> lst = new List<string>(pth.Split('/'));
                KeyValuePair<Type, object> res = BlackboardTag.GetBlackboard(lst[0]);
                return res.Value != null;
            }
            /*else if (pth.StartsWith("@") && 0 == pth.Count(c => c == '/'))  // @turn
            {
                KeyValuePair<Type, object> res = BlackboardTag.GetBlackboard(pth);
                return res.Value != null;
            }*/

            DebugUtils.LogError($"{path} is err : format not allowed");
            return false;
        }





        public static Variable<T> FindVariable<T>(object blackboard, string path)
        {

            string pth = path;
            object currentObj = blackboard;

            //string bbName = "";

            if (string.IsNullOrEmpty(pth))
                return null;

            if (pth == "/")
            {
                return new Variable<T>(null, "/", MainBlackboard.Instance);
            }
            else if (pth.StartsWith("/"))
            {
                currentObj = MainBlackboard.Instance;
                pth = pth.Substring(1);
            }
            else if (pth == "./")
            {
                //return new Variable<T>(null, "./", ContentBlackboard.Instance);

                return new Variable<T>(null, "./", BlackboardTag.GetBlackboard02("@content"));
            }
            else if (pth.StartsWith("./"))
            {
                // "@contentBlackboard"
                // "@content"

                currentObj = BlackboardTag.GetBlackboard02("@content");
                pth = pth.Replace("./", "");

                /*currentObj = ContentBlackboard.Instance;
                pth = pth.Substring(2);*/
            }
            else if (pth.StartsWith("@") && pth.EndsWith("/") && 1 == pth.Count(c => c == '/'))
            {
                List<string> lst = new List<string>(pth.Split('/'));
                KeyValuePair<Type, object> res = BlackboardTag.GetBlackboard(lst[0]);
                if (res.Key != null && res.Value != null)
                {
                    currentObj = res.Value;
                }
                return new Variable<T>(null, pth, currentObj);

                /*List<string> lst = new List<string>(pth.Split('/'));
                currentObj = BlackboardTag.GetBlackboard02(lst[0]);
                return new Variable<T>(null, pth, currentObj);*/

            }
            else if (pth.StartsWith("@"))
            {
                //@turn
                //@cunstomData

                List<string> lst = new List<string>(pth.Split('/'));
                currentObj = BlackboardTag.GetBlackboard02(lst[0]);
                lst.RemoveAt(0);
                pth = String.Join("/", lst);
            }

            if (currentObj == null) 
                return null;
            

            string[] fieldPath = pth.Split('/');

            Type currentType = currentObj.GetType();

            object parentObj = currentObj;
            foreach (var part in fieldPath)
            {



                #region 索引获取对象
                /*
                bool isIndex = false;
                int index = -1;
                if (part.StartsWith("__"))
                {
                    try
                    {
                        //index = int.Parse(part.Replace("__", ""));
                        index = int.Parse(part.Substring(2));
                        isIndex = index >= 0;
                    }
                    catch
                    {
                        isIndex = false;
                        index = -1;
                    }
                }
                if (isIndex)
                {                  
                    if (index < ((IList)currentObj).Count)
                    {
                        currentObj = ((IList)currentObj)[index];
                        currentType = currentObj.GetType();
                        continue;
                    }
                    else
                    {
                        DebugUtil.LogError($"[BlackboardUtil] Invalid path part : {part}  at {obj}[{path}]");
                        return null;
                    }
                }*/

                #endregion


                #region 索引获取对象  、 键值获取对象

                if (part.StartsWith("__"))
                {
                    try  //  @content/ListXXX/__1/x5
                    {
                        int index = -1;
                        //index = int.Parse(part.Replace("__", ""));
                        index = int.Parse(part.Substring(2));
                        if (index < ((IList)currentObj).Count)
                        {
                            currentObj = ((IList)currentObj)[index];
                            currentType = currentObj.GetType();
                            continue;
                        }
                        else
                        {
                            DebugUtils.LogError($"[BlackboardUtil] Invalid path part : {part}  at {blackboard}[{path}]");
                            return null;
                        }
                    }
                    catch{
                    }

                    try  //  @content/DicXXX/__arg/x5
                    {
                        if ( ((IDictionary)currentObj).Contains(part.Substring(2)))
                        {
                            currentObj = ((IDictionary)currentObj)[part.Substring(2)];
                            currentType = currentObj.GetType();
                            continue;
                        }
                        else
                        {
                            DebugUtils.LogError($"[BlackboardUtil] Invalid path part : {part}  at {blackboard}[{path}]");
                            return null;
                        }
                    }
                    catch {
                    }
                }

                #endregion




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
                        DebugUtils.LogError($"[BlackboardUtil] Invalid path part : {part}  at {blackboard}[{path}]");
                        return null;
                    }
                }

                /*
                FieldInfo fieldInfo = currentType.GetField(part, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo == null)
                {
                    DebugUtil.LogError($"Field not found : {part}");
                    return null;
                }
                parentObj = currentObj;
                currentObj = fieldInfo.GetValue(currentObj);
                currentType = fieldInfo.FieldType;
                */
            }

            return new Variable<T>(parentObj, fieldPath[fieldPath.Length - 1], null);
        }
    }



#if !false
    public class Variable<T>
    {
        public object parent;
        public string targetName;
        public object target;

        public Variable(object parent, string targetName, object target)
        {
            this.parent = parent;
            this.targetName = targetName;
            this.target = target;
        }

        public T value
        {
            get
            {

                if (target != null)
                {
                    return (T)target;
                }
                /*
                Type currentType = parent.GetType();
                FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                object fildObj = fieldInfo.GetValue(parent);
                return (T)fildObj;
                */
                Type currentType = parent.GetType();
                PropertyInfo property = currentType.GetProperty(targetName);
                if (property != null) //属性
                {
                    object resObj = property.GetValue(parent);
                    return (T)resObj;
                }
                else
                {
                    FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    object resObj = fieldInfo.GetValue(parent);
                    return (T)resObj;
                }
            }
            set
            {
                if (target != null)
                {
                    target = value;
                }

                Type currentType = parent.GetType();
                PropertyInfo property = currentType.GetProperty(targetName);
                if (property != null) //属性
                {
                    property.SetValue(parent, (T)value);
                }
                else
                {
                    FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    //object fildObj = fieldInfo.GetValue(parent);
                    fieldInfo.SetValue(parent, (T)value);
                }
            }
        }
    }
#endif

}






namespace GameMaker
{
#if false
    public class Variable
    {
        public object parent;
        public string targetName;
        public object target;

        public Variable(object parent, string targetName, object target)
        {
            this.parent = parent;
            this.targetName = targetName;
            this.target = target;
        }

        public virtual object value
        {
            get
            {

                if (target != null)
                {
                    return target;
                }
                /*
                Type currentType = parent.GetType();
                FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                object fildObj = fieldInfo.GetValue(parent);
                return (T)fildObj;
                */
                Type currentType = parent.GetType();
                PropertyInfo property = currentType.GetProperty(targetName);
                if (property != null) //属性
                {
                    object resObj = property.GetValue(parent);
                    return resObj;
                }
                else
                {
                    FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    object resObj = fieldInfo.GetValue(parent);
                    return resObj;
                }
            }
            set
            {
                if (target != null)
                {
                    target = value;
                }

                Type currentType = parent.GetType();
                PropertyInfo property = currentType.GetProperty(targetName);
                if (property != null) //属性
                {
                    property.SetValue(parent, value);
                }
                else
                {
                    FieldInfo fieldInfo = currentType.GetField(targetName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    //object fildObj = fieldInfo.GetValue(parent);
                    fieldInfo.SetValue(parent, value);
                }
            }
        }
    }


    public class Variable<T> : Variable
    {
        new public T value
        {
            get
            {
                return (T)base.value;
            }
            set
            {
                base.value = value;
            }
        }

        public Variable(object parent, string targetName, object target) : base(parent, targetName, target)
        {

        }
    }

#endif
}







#if UNITY_EDITOR && false
public class MyClass
{
    public int MyField;
    public int MyProperty { get; set; }
}

class Program
{
    static void Main()
    {
        Type type = typeof(MyClass);

        // 获取所有公共字段
        FieldInfo[] fields = type.GetFields();
        foreach (var field in fields)
        {
            Console.WriteLine(field.Name); // 输出 MyField
        }

        // 获取所有公共属性
        PropertyInfo[] properties = type.GetProperties();
        foreach (var property in properties)
        {
            Console.WriteLine(property.Name); // 输出 MyProperty
        }

        // 获取单个字段
        FieldInfo myField = type.GetField("MyField");
        Console.WriteLine(myField.Name); // 输出 MyField

        // 获取单个属性
        PropertyInfo myProperty = type.GetProperty("MyProperty");
        Console.WriteLine(myProperty.Name); // 输出 MyProperty
    }
}

#endif

