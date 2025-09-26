using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOFindTag : MonoBehaviour
{
    public static List<GameObject> gos = new List<GameObject>();
    public static void Add(GameObject go)
    {
        if (!gos.Contains(go))
        {
            gos.Add(go);
        }
    }
    public static void Remove(GameObject go)
    {
        if (gos.Contains(go))
        {
            gos.Remove(go);
        }
    }
    /*public static GameObject Find(string path)
    {
        GameObject go = GameObject.Find(path);
        if (go != null)
        {
            gos.Add(go);
        }

        string[] parts = path.Split('/');
        string rootName = parts[0];
        string subPht = null;
        if (parts.Length > 1)
        {
            subPht = string.Join("/", parts, 1, parts.Length - 1);
        }

        if(subPht != null)
        {
            foreach (GameObject go1 in gos)
            {
                if (go1.name == rootName)
                {
                    Transform targrt = go1.transform.Find(subPht);
                    if (targrt != null)
                        return targrt.gameObject;
                }
            }
        }
        else
        {
            foreach (GameObject go1 in gos)
            {
                if (go1.name == rootName)
                    return go1;
            }
        }
        return null;
    }*/



    // Start is called before the first frame update
    void Awake()
    {
        Add(this.gameObject);
    }

    private void OnDestroy()
    {
        Remove(this.gameObject);
    }
}




public static class GOFind
{

    /// <summary>
    /// ���Ҽ����δ����Ķ���
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <remarks>
    /// * �����ڵ�active=false,���ӽڵ㣬Ҳ���Ҳ����ġ�
    /// </remarks>
    public static  GameObject FindObjectIncludeInactive0009(string path)
    {
        GameObject targetObject = GameObject.Find(path);
        if (targetObject != null)
        {
            if(targetObject.GetComponent<GOFindTag>() == null)
                targetObject.AddComponent<GOFindTag>();
            return targetObject;
        }


        // ���� ��ǰ������ ��activeΪ true �� false �Ķ���
        string[] parts = path.Split('/');
        string rootName = parts[0];
        string subPth = null;
        if (parts.Length > 1)
        {
            subPth = string.Join("/", parts, 1, parts.Length - 1);
        }


        List<GameObject> rootObjects = new List<GameObject> { };
        // ��ǰ�����ĸ�������
        Scene currentScene = SceneManager.GetActiveScene();
        rootObjects.AddRange(currentScene.GetRootGameObjects());

        // ���� DontDestroyOnLoad ������ ��activeΪtrue�Ķ���
        rootObjects.AddRange(FindActiveObjectInDontDestroyOnLoad());

        // �޷�������  DontDestroyOnLoad ������ ��activeΪfalse�Ķ���

        // ��ǰ���ҹ��Ĵ���ǵĶ���֧�� active = false��[���DontDestroyOnLoad�� active������Ϊfalse�Ķ���]
        rootObjects.AddRange(GOFindTag.gos);

   
        foreach (GameObject rootObject in rootObjects)
        {
            GameObject foundObject = _FindNodeByName(rootObject.transform, rootName);
            if (foundObject != null)
            {
                if(subPth != null)
                {
                    targetObject = foundObject.transform.Find(subPth)?.gameObject;
                }
                else
                {
                    targetObject = foundObject;
                }
                break;
            }
        }

        if (targetObject != null)
        {
            if (targetObject.GetComponent<GOFindTag>() == null)
                targetObject.AddComponent<GOFindTag>();
            return targetObject;
        }  


        //targetObject = _FindObject(rootObjects, rootName, subPht);

        return null;
    }




    public static GameObject FindObjectIncludeInactive(string path)
    {

        GameObject targetObject = _FindObject(GOFindTag.gos, path);

        if (targetObject != null)
            return targetObject;

        targetObject = GameObject.Find(path);
        if (targetObject != null)
        {
            if (targetObject.GetComponent<GOFindTag>() == null)
                targetObject.AddComponent<GOFindTag>();
            return targetObject;
        }


        List<GameObject> rootObjects = new List<GameObject> { };
        // ��ǰ�����ĸ�������
        Scene currentScene = SceneManager.GetActiveScene();
        rootObjects.AddRange(currentScene.GetRootGameObjects());

        // ���� DontDestroyOnLoad ������ ��activeΪtrue�Ķ���
        rootObjects.AddRange(FindActiveObjectInDontDestroyOnLoad());

        // �޷�������  DontDestroyOnLoad ������ ��activeΪfalse�Ķ���

        // ��ǰ���ҹ��Ĵ���ǵĶ���֧�� active = false��[���DontDestroyOnLoad�� active������Ϊfalse�Ķ���]
        //rootObjects.AddRange(GOFindTag.gos);

        targetObject = _FindObject(rootObjects, path);

        if (targetObject != null)
        {
            if (targetObject.GetComponent<GOFindTag>() == null)
                targetObject.AddComponent<GOFindTag>();
            return targetObject;
        }

        //targetObject = _FindObject(rootObjects, rootName, subPht);

        return null;
    }


    static GameObject _FindObject(List<GameObject> gos, string path)
    {

        string[] parts = path.Split('/');
        string rootName = parts[0];
        string subPth = null;
        if (parts.Length > 1)
        {
            subPth = string.Join("/", parts, 1, parts.Length - 1);
        }


        GameObject targetObject = null;

        foreach (GameObject rootObject in gos)
        {
            GameObject foundObject = _FindNodeByName(rootObject.transform, rootName);
            if (foundObject != null)
            {
                if (subPth != null)
                {
                    targetObject = foundObject.transform.Find(subPth)?.gameObject;
                }
                else
                {
                    targetObject = foundObject;
                }
                break;
            }
        }

        if (targetObject != null)
        {
            if (targetObject.GetComponent<GOFindTag>() == null)
                targetObject.AddComponent<GOFindTag>();
            return targetObject;
        }
        return null;
    }



    private static GameObject[] FindActiveObjectInDontDestroyOnLoad()
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


    public static void Test11()
    {
        // ��ȡ���и�����
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            Debug.Log("Found object: " + obj.name + " (Scene: " + obj.scene.name + ")");
        }
    }


}

