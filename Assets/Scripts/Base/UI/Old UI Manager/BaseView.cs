using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UIConst;

namespace Game
{
    public class BaseView : MonoBehaviour
    {
        private Dictionary<string, GameObject> effDic = new Dictionary<string, GameObject>();
        protected bool isRemove = false;
        protected bool isOpen = false;
        protected new string name;
        //�Ƿ�ȫ��Ļ��ʾ
        public bool isFullScreen;
        //����һ���㼶
        public UILayer layer;

        private RectTransform rt;

        private Canvas canvas;

        protected virtual void Awake()
        {
            rt = transform.GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }
        }

        protected virtual void Start()
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = (int)layer;
            gameObject.AddComponent<GraphicRaycaster>();
        }
        public void ResetScaleAndPos()
        {
            if (isFullScreen)
            {
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                rt.anchorMax = new Vector2(1, 1);
                rt.anchorMin = new Vector2(0, 0);
                rt.pivot = new Vector2(0.5f, 0.5f);
            }
            rt.localScale = new Vector3(1, 1, 1);
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localEulerAngles = new Vector3(0, 0, 0);
        }
        protected virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void OpenPanel(string name)
        {
            this.name = name;
            isOpen = true;
            SetActive(true);
        }

        public virtual void ClosePanel()
        {
            isRemove = true;
            isOpen = false;
            SetActive(false);
            //Destroy(gameObject);
            //if (UIManager.Instance.panelDict.ContainsKey(name))
            //{
            //    UIManager.Instance.panelDict.Remove(name);
            //}
        }

        public bool PanelIsOpen()
        {
            return isOpen;
        }

        //offLayer ƫ�ƿ�������   ��Ч�������е��� layer��ʱ����������ʱ���Ǹ���,������Ҫ��ƫ��������
        protected virtual GameObject AddEff(Transform parent, string effName, int offLayer = 0)
        {
            GameObject p;
            if (effDic.ContainsKey(effName))
            {
                effDic.TryGetValue(effName, out p);
                p.SetActive(false);
                p.SetActive(true);
                return p;
            }
            else
            {
                string path = "Games/Eff/" + effName;
                //p = ResMgr1001.Instance.Load<GameObject>(path);
                p = new GameObject(path);

                //���޸������ӽڵ�layer
                ModifySortingOrderRecursive(p.transform, offLayer);
                //���޸ĸ��ڵ�
                ModifySortingOrderParent(p.transform, offLayer);
                Vector3 scale = p.transform.localScale;
                Vector3 localPos = p.transform.localPosition;
                p.transform.SetParent(parent);
                p.transform.localScale = scale;
                p.transform.localPosition = localPos;
                effDic.Add(effName, p);
                return p;
            }
        }

        //����һ��GameObject
        protected virtual GameObject AddGameObject(Transform parent, string resPaht)
        {
            GameObject g;
            string path = "Games/" + resPaht;
            //g = ResMgr1001.Instance.Load<GameObject>(path);
            g = new GameObject(path);
            g.transform.SetParent(parent);
            return g;
        }

        protected void ModifySortingOrderRecursive(Transform tran, int offLayer)
        {
            Renderer renderer;
            // ��������ֱ���Ӷ���
            foreach (Transform child in tran)
            {
                renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    int order = renderer.sortingOrder;
                    renderer.sortingOrder = order + (int)layer + offLayer;
                }

                // ����Ӷ������Լ����Ӷ��󣬵ݹ����
                if (child.childCount > 0)
                {
                    ModifySortingOrderRecursive(child, offLayer);
                }
            }
        }

        //����޸ĸ��ڵ��layer
        protected void ModifySortingOrderParent(Transform tran, int offLayer)
        {
            Renderer renderer = tran.GetComponent<Renderer>();
            if (renderer != null)
            {
                int order = renderer.sortingOrder;
                renderer.sortingOrder = order + (int)layer + offLayer;
            }
        }
    }


    public class BaseView<T> : BaseView where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var founds = FindObjectsOfType(typeof(T));
                    if (founds.Length > 1)
                    {
                        Debug.LogError("[Weak Singleton] Singlton '" + typeof(T) +
                            "' should never be more than 1!");
                        return null;
                    }
                    else if (founds.Length > 0)
                    {
                        _instance = (T)founds[0];
                    }
                }
                return _instance;
            }
        }
        protected virtual void OnDisable()
        {
            _instance = null;
        }
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }


}