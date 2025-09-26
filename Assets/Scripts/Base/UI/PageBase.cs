using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMaker;
using System.Threading.Tasks;
using System;
using Sirenix.OdinInspector;

namespace Game
{
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(Canvas))]
    public  class PageBase : MonoBehaviour
    {
        /*
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        */


        private Dictionary<string, GameObject> effDic = new Dictionary<string, GameObject>();
        protected bool isRemove = false;
        protected bool isOpen = false;


        //protected new string name;
        public PageName pageName;

        //是否全屏幕显示
        public bool isFullScreen;
        //在哪一个层级
        public UILayer layer;

        /// <summary> 页面在那个节点 </summary>
        public PageType pageType = PageType.BasePage;
        public List<LayerController> layerInfos = new List<LayerController>();
        public int pageNumb = 0;
        /// <summary> 是否缓存 </summary>
        public bool isCache = true;
        /// <summary> 是否单列 </summary>
        //public bool isInstance = true;


        private RectTransform rt => transform.GetComponent<RectTransform>();

        private Canvas canvas;

        /*protected  void Awake()
        {
            rt = transform.GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }
            if (gameObject.GetComponent<GraphicRaycaster>() == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }


            //canvas.overrideSorting = true;
            //canvas.sortingOrder = (int)layer;

            //canvas.overrideSorting = true;
            //canvas.sortingLayerName = "Base";
            //canvas.sortingOrder = 0;
           // gameObject.AddComponent<GraphicRaycaster>();
        }*/

        /*protected  void Start()
        {
            //canvas.overrideSorting = true;
            //canvas.sortingOrder = (int)layer;
            //gameObject.AddComponent<GraphicRaycaster>();

            canvas.overrideSorting = true;
            canvas.sortingLayerName = "Base"; //"Background"; //
            canvas.sortingOrder = 0;
        }*/

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

        public virtual void OnOpen(PageName name, EventData data)
        {
            //this.name = name; //Enum.GetName(typeof(PageName), name); 

            pageName = name;

            isOpen = true;
            SetActive(true);
        }

        EventData response;
        public virtual void OnClose(EventData data = null)
        {
            response = data;
            isRemove = true;
            isOpen = false;
            SetActive(false);
            //Destroy(gameObject);
            //if (UIManager.Instance.panelDict.ContainsKey(name))
            //{
            //    UIManager.Instance.panelDict.Remove(name);
            //}
        }


        /// <summary>
        /// 页面置顶
        /// </summary>
        public virtual void OnTop()
        {

        }


        public async Task<EventData> OnOpenAsync(PageName name, EventData data)
        {
            OnOpen(name,data);

            /*
            await WaitUntil(() =>
            {
                return isOpen == false;
            });*/

            await WaitUntil(() => isOpen == false );

            return response;
        }


        public bool IsOpen()
        {
            return isOpen;
        }

        //offLayer 偏移可以正负   特效的制作有点乱 layer有时候是正数有时候是负数,所以需要加偏移做调整
        /*protected virtual GameObject AddEff(Transform parent, string effName, int offLayer = 0)
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
                p = ResMgr1001.Instance.Load<GameObject>(path);
                //先修改所有子节点layer
                ModifySortingOrderRecursive(p.transform, offLayer);
                //再修改父节点
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

        //加载一个GameObject
        protected virtual GameObject AddGameObject(Transform parent, string resPaht)
        {
            GameObject g;
            string path = "Games/" + resPaht;
            g = ResMgr1001.Instance.Load<GameObject>(path);
            g.transform.SetParent(parent);
            return g;
        }*/

        protected void ModifySortingOrderRecursive(Transform tran, int offLayer)
        {
            Renderer renderer;
            // 遍历所有直接子对象
            foreach (Transform child in tran)
            {
                renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    int order = renderer.sortingOrder;
                    renderer.sortingOrder = order + (int)layer + offLayer;
                }

                // 如果子对象还有自己的子对象，递归调用
                if (child.childCount > 0)
                {
                    ModifySortingOrderRecursive(child, offLayer);
                }
            }
        }

        //最后修改父节点的layer
        protected void ModifySortingOrderParent(Transform tran, int offLayer)
        {
            Renderer renderer = tran.GetComponent<Renderer>();
            if (renderer != null)
            {
                int order = renderer.sortingOrder;
                renderer.sortingOrder = order + (int)layer + offLayer;
            }
        }




        private static async Task WaitUntil(Func<bool> condition)
        {
            while (!condition())
            {
                //await Task.Delay(1000); // 每秒检查一次
                await Task.Delay(300); // 每300ms检查一次
            }
        }



        #region 测试异步Task


        public async Task AsyncOpen01(string arg)
        {

            var conditionIsMet = false;
            var startTime = DateTime.Now;

            await WaitUntil(() =>
            {
                // 这是你的条件，当它为true时继续往下走
                return conditionIsMet || (DateTime.Now - startTime).TotalSeconds > 10;
            });

            Debug.Log($"async 1 task arg = {arg}");
        }

        public async Task<string> AsyncOpen02(string arg)
        {
            var conditionIsMet = false;
            var startTime = DateTime.Now;
            await WaitUntil(() =>
            {
                // 这是你的条件，当它为true时继续往下走
                return conditionIsMet || (DateTime.Now - startTime).TotalSeconds > 10;
            });
            string res = $"async 2 task arg = {arg}";
            return res;
        }




        [Button]
        async void TestAsyncFunc(string arg = "i am async")
        {
            string res = await AsyncOpen02(arg);
            Debug.Log(res);
        }

        #endregion
    }

}
