using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * 
 * 把需要换皮的节点名字，生成excel。
 * 写个插件，通过excel来检查各个节点。
 */
public class SkinTag : MonoBehaviour
{
    public static List<GameObject> skins = new List<GameObject>();

    //public string nickname;
    public static void Add(GameObject go)
    {
        if (!skins.Contains(go))
        {
            skins.Add(go);
        }
    }
    public static void Remove(GameObject go)
    {
        if (skins.Contains(go))
        {
            skins.Remove(go);
        }
    }

    void Awake()
    {
        Add(this.gameObject);
    }

    private void OnDestroy()
    {
        Remove(this.gameObject);
    }


    /// <summary>
    /// 改文字
    /// </summary>
    /// <param name="txt"></param>
    public void SetText(string txt)
    {
        Text txtComp = transform.GetComponent<Text>();
        if (txtComp != null)
        {
            txtComp.text = txt;
            return;
        }

        var tmpComp = transform.GetComponent<TextMeshProUGUI>();
        if (tmpComp != null)
        {
            tmpComp.text = txt;
            return;
        }
    }



    /// <summary>
    /// 改图片
    /// </summary>
    /// <param name="txt"></param>
    public void SetImage(Sprite spr)
    {
        Image imgComp = transform.GetComponent<Image>();
        if (imgComp != null)
        {
            imgComp.sprite = spr;
            return;
        }
    }

}


public class SkinHelper : MonoBehaviour
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="goDefault">为了支持u3d界面拖入GO，或Find("GoName")</param>
    /// <returns></returns>
    public static GameObject Find(string name ,GameObject goDefault = null)
    {
        if(goDefault != null)
        {
            goDefault.name = name;
            if (goDefault.GetComponent<SkinTag>() == null)
                goDefault.AddComponent<SkinTag>();
            return goDefault;
        }

        GameObject targetObject = null;
        for (int i=0; i< SkinTag.skins.Count; i++)
        {
            if (SkinTag.skins[i].name == name)
            {
                targetObject = SkinTag.skins[i];
                return targetObject;
            }
        }

        targetObject = GameObject.Find(name);
        if (targetObject != null)
        {
            if (targetObject.GetComponent<SkinTag>() == null)
                targetObject.AddComponent<SkinTag>();
            return targetObject;
        }

        List<GameObject> rootObjects = new List<GameObject> { };
        // 当前场景的根基对象
        Scene currentScene = SceneManager.GetActiveScene();
        rootObjects.AddRange(currentScene.GetRootGameObjects());

        // 查找 DontDestroyOnLoad 场景中 且active为true的对象
        rootObjects.AddRange(FindActiveObjectInDontDestroyOnLoad());

        // 无法查找在  DontDestroyOnLoad 场景中 且active为false的对象
        // 可以找到  DontDestroyOnLoad 场景中 且active为false 但是有挂脚本SkinTag的对象

        for (int i=0;i<rootObjects.Count;i++)
        {
            GameObject goCur = _FindNodeByName(rootObjects[i].transform, name);
            if (goCur != null)
            {
                targetObject = goCur;
                break;
            }
        }

        if (targetObject != null)
        {
            if (targetObject.GetComponent<SkinTag>() == null)
                targetObject.AddComponent<SkinTag>();
            return targetObject;
        }

        return null;
    }



    static GameObject _FindNodeByName(Transform parent, string objectName)
    {
        if (parent.name == objectName)
        {
            return parent.gameObject;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            GameObject foundObject = _FindNodeByName(child, objectName);
            if (foundObject != null)
            {
                return foundObject;
            }
        }

        return null;
    }



    public static GameObject[] FindActiveObjectInDontDestroyOnLoad()
    {
        var allGameObjects = new List<GameObject>();

        allGameObjects.AddRange(UnityEngine.Object.FindObjectsOfType<GameObject>());

        //移除所有场景包含的对象

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }

        //移除父级不为nul1的对象
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }

        /*
        foreach (GameObject rootObject in allGameObjects)
        {
            //TraverseChildren(rootObject.transform);
            Debug.Log(rootObject.transform.name + $"{rootObject.transform.parent}");
        }*/

        return allGameObjects.ToArray();
    }
}
