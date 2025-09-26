using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * 
 * ����Ҫ��Ƥ�Ľڵ����֣�����excel��
 * д�������ͨ��excel���������ڵ㡣
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
    /// ������
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
    /// ��ͼƬ
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
    /// <param name="goDefault">Ϊ��֧��u3d��������GO����Find("GoName")</param>
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
        // ��ǰ�����ĸ�������
        Scene currentScene = SceneManager.GetActiveScene();
        rootObjects.AddRange(currentScene.GetRootGameObjects());

        // ���� DontDestroyOnLoad ������ ��activeΪtrue�Ķ���
        rootObjects.AddRange(FindActiveObjectInDontDestroyOnLoad());

        // �޷�������  DontDestroyOnLoad ������ ��activeΪfalse�Ķ���
        // �����ҵ�  DontDestroyOnLoad ������ ��activeΪfalse �����йҽű�SkinTag�Ķ���

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

        //�Ƴ����г��������Ķ���

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }

        //�Ƴ�������Ϊnul1�Ķ���
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
