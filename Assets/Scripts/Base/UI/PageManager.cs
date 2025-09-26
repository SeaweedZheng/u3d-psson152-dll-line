using System.Collections.Generic;
using UnityEngine;
using GameMaker;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;
using System.Collections;

namespace Game
{

    public enum PageType
    {
        BasePage,
        PopupPage,
        OverlayPage,
    }



    public partial class PageManager : MonoSingleton<PageManager>
    {
        private static PageManager _instance;
        private GameObject _goPageRoot, _goPageStore;


        /// <summary> Ԥ�Ƽ������ֵ� </summary>
        private Dictionary<PageName, GameObject> prefabDict;


        /// <summary> ����Ļ����ֵ� </summary>
        public Dictionary<PageName, PageBase> pageCacheDict;


        /// <summary> ҳ��ҵĽڵ� </summary>
        private Dictionary<PageType, GameObject> pageNodes;

        /*
        public static PageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PageManager();
                }
                return _instance;
            }
        }
        private PageManager()
        {
            InitDicts();
        }*/


        const string MARK_BUNDLE_PAGE_MANAGER = "MARK_BUNDLE_PAGE_MANAGER";

        private void Awake()
        {
            InitDicts();
        }



        public GameObject goPageRoot
        {
            get
            {
                if (_goPageRoot == null)
                {
                    _goPageRoot = GameObject.Find("Page Root");//.transform;
                    return _goPageRoot;
                };
                return _goPageRoot;
            }
        }



        public GameObject goPageStore
        {
            get
            {
                if (_goPageStore == null)
                {
                    //_goPageStore = GameObject.Find("Page Root/_Store");//.transform;
                    GameObject go;
                    Transform tfmStore = transform.Find("_Store");
                    if (tfmStore != null) {
                        go = tfmStore.gameObject;
                    }
                    else
                    {
                        go = new GameObject();
                        go.name = "_Store";
                        go.transform.SetParent(transform);
                    }
                    _goPageStore = go;
                };
                return _goPageStore;
            }
        }


        private Transform tfmPageRoot => goPageRoot.transform;
        private Transform tfmPageStore => goPageStore.transform;




        private void InitDicts()
        {
            prefabDict = new Dictionary<PageName, GameObject>();
            pageCacheDict = new Dictionary<PageName, PageBase>();

            pageNodes = new Dictionary<PageType, GameObject>();
            pageNodes.Add(PageType.BasePage, tfmPageRoot.Find("Base").gameObject);
            pageNodes.Add(PageType.PopupPage, tfmPageRoot.Find("Popup").gameObject);
            pageNodes.Add(PageType.OverlayPage, tfmPageRoot.Find("Overlay").gameObject);

        }

        public bool IsHasPopupOrOverlayPage()
        {
            return pageNodes[PageType.PopupPage].transform.childCount > 0 ||
                pageNodes[PageType.OverlayPage].transform.childCount > 0;
        }

        List<GameObject> pagesStack = new List<GameObject>();


        public int IndexOf(PageBase basePage)
        {
            if (pagesStack.Contains(basePage.gameObject))
                return pagesStack.IndexOf(basePage.gameObject);
            return -1;
        }

        public int IndexOf(PageName pageName)
        {
            for(int i = 0; i < pagesStack.Count; i++)
            {
                if (pagesStack[i].GetComponent<PageBase>().pageName == pageName)
                    return i;                
            }
            return -1;
        }

        /// <summary>
        /// �Ƿ����ҳ
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public bool IsTop(PageName pageName) => IndexOf(pageName) == 0;

        /// <summary>
        /// �Ƿ����ҳ
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public bool IsTop(PageBase basePage) => IndexOf(basePage) == 0;

        public PageBase OpenPage(PageName pageName, EventData data = null)
        {
            string name = Enum.GetName(typeof(PageName), pageName);

            PageBase cmpPage = null;
            // ����Ƿ��Ѵ�
            if (pageCacheDict.TryGetValue(pageName, out cmpPage))
            {
                if (!cmpPage.IsOpen())
                {
                    InputPageStack(cmpPage);
                    cmpPage.OnOpen(pageName,  data);
                }
                return cmpPage;
            }


            // ���·���Ƿ�����
            string path = "";
            if (!UIConst.Instance.pathDict.TryGetValue(pageName, out path))
            {
                DebugUtils.LogError("�������ƴ��󣬻�δ����·��: " + pageName);
                return null;
            }

            /*
            GameObject pagePrefab = null;
            if (!prefabDict.TryGetValue(name, out pagePrefab))
            {
                string realPath = path;
                pagePrefab = ResMgr1001.Instance.Load<GameObject>(realPath);
                prefabDict.Add(name, pagePrefab);
            }
            */
            //@@# GameObject pagePrefab  = ResMgr1001.Instance.Load<GameObject>(path);
            GameObject pagePrefab = ResourceManager.Instance.LoadAssetAtPath<GameObject>(path, MARK_BUNDLE_PAGE_MANAGER);

            // �򿪽���
            GameObject goPage = pagePrefab;
            goPage.transform.SetParent(tfmPageRoot);
            cmpPage = goPage.GetComponent<PageBase>();
    
            if (cmpPage.isCache)
                pageCacheDict.Add(pageName, cmpPage);

            AddPageCanvas(cmpPage);

            InputPageStack(cmpPage);
            cmpPage.OnOpen(pageName, data);
            return cmpPage;
        }


        public async Task<EventData> OpenPageAsync(PageName pageName, EventData data = null)
        {
            string name = Enum.GetName(typeof(PageName), pageName);

            PageBase cmpPage = null;
            // ����Ƿ��Ѵ�
            if (pageCacheDict.TryGetValue(pageName, out cmpPage))
            {
                if (!cmpPage.IsOpen())
                {
                    InputPageStack(cmpPage);
                    //EventData res0 = await cmpPage.OpenAsync(name, data);
                    //return res0;
                    return await cmpPage.OnOpenAsync(pageName, data);
                }
                return new EventData("IsOpen");
            }


            // ���·���Ƿ�����
            string path = "";
            if (!UIConst.Instance.pathDict.TryGetValue(pageName, out path))
            {
                DebugUtils.LogError("�������ƴ��󣬻�δ����·��: " + pageName);
                return null;
            }

            /*
            GameObject pagePrefab = null;
            if (!prefabDict.TryGetValue(name, out pagePrefab))
            {
                string realPath = path;
                pagePrefab = ResMgr1001.Instance.Load<GameObject>(realPath);
                prefabDict.Add(name, pagePrefab);
            }
            */
            //@@# GameObject pagePrefab = ResMgr1001.Instance.Load<GameObject>(path);
            GameObject pagePrefab = ResourceManager.Instance.LoadAssetAtPath<GameObject>(path, MARK_BUNDLE_PAGE_MANAGER);

            // �򿪽���
            GameObject goPage = pagePrefab;
            goPage.transform.SetParent(tfmPageRoot);
            cmpPage = goPage.GetComponent<PageBase>();

            if (cmpPage.isCache)
                pageCacheDict.Add(pageName, cmpPage);

            AddPageCanvas(cmpPage);

            InputPageStack(cmpPage);
            //EventData res1 = await cmpPage.OpenAsync(name, data);
            //return res1;

            return await cmpPage.OnOpenAsync(pageName, data);
        }

        public async void OpenPageAsync(PageName pageName, EventData data, Action<EventData> responseCallback)
        {
            EventData res = await OpenPageAsync(pageName, data);
            responseCallback?.Invoke(res);
        }

        /*
        public IEnumerator OpenPageIetor(PageName pageName, EventData data, Action<EventData> cb)
        {
            bool isNext = false;
            EventData res = null;
            var Func = async () =>
            {
                res = await OpenPageAsync(pageName, data);
                isNext = true;
            };
            Func();
            yield return new WaitUntil(()=> isNext==true);
            cb?.Invoke(res);
        }*/


        private void AddPageCanvas(PageBase compBasePage)
        {
            GameObject goBasePage = compBasePage.gameObject;

            //if (goBasePage.GetComponent<GraphicRaycaster>() == null)
                //goBasePage.AddComponent<GraphicRaycaster>();

            Canvas canvas = goBasePage.GetComponent<Canvas>();
            switch (compBasePage.pageType)
            {
                case PageType.BasePage:
                case PageType.PopupPage:
                case PageType.OverlayPage:
                    canvas.overrideSorting = true;
                    //canvas.sortingLayerName = "Base"; 
                    canvas.sortingLayerID = SortingLayer.NameToID("Base");
                    canvas.sortingOrder = 0;
                    break;
            }

        }


        Dictionary<PageType, int> pageNumbs = new Dictionary<PageType, int>()
        {
            [PageType.BasePage] = -1,
            [PageType.PopupPage] = -1,
            [PageType.OverlayPage] = -1,
        };

        public void InputPageStack(PageBase basePage)
        {
            /*GameObject root;
            if (pageNodes.TryGetValue(basePage.pageType, out root))
            {
                basePage.transform.SetParent(root.transform);
                basePage.ResetScaleAndPos();
            }*/
            
            basePage.transform.SetParent(pageNodes[basePage.pageType].transform);
            basePage.ResetScaleAndPos();

            //int pageNumb = basePanel.transform.GetSiblingIndex();

            pageNumbs[basePage.pageType]++;
            int pageNumb = pageNumbs[basePage.pageType];
            basePage.pageNumb = pageNumb;

            int newPageVal = LayerController.GetPageValue(basePage.pageType, basePage.pageNumb);

            int indexStackPage = pagesStack.Count;
            for (int i = 0; i < pagesStack.Count; i++)
            {
                PageBase tempBp = pagesStack[i].gameObject.GetComponent<PageBase>();
                //int pageNumb = tempBV.transform.GetSiblingIndex()

                int tempPageVal = LayerController.GetPageValue(tempBp.pageType, tempBp.pageNumb);
                if (newPageVal > tempPageVal)
                {
                    indexStackPage = i;

                    DebugUtils.Log($"new page index ={indexStackPage} newPageVal = {newPageVal}  tempPageVal = {tempPageVal}");
                    break;
                }
            }

            pagesStack.Insert(indexStackPage, basePage.gameObject);

            PageBase toStackPage = null;

            //if (indexStackPage == 0) basePage.OnTop();
            if (indexStackPage == 0)
            {
                if(corToTop != null)
                    StopCoroutine(corToTop);
                corToTop = StartCoroutine(SetPageToTop(basePage));
            }

            if (indexStackPage != 0)
                toStackPage = basePage;
            else if(indexStackPage == 0 && pagesStack.Count>1)
                toStackPage = pagesStack[1].transform.GetComponent<PageBase>();


            if (toStackPage != null)
            {

                List<LayerController> layerInfos = LayerController.GreatLayerInfos(toStackPage.transform);
                toStackPage.layerInfos = layerInfos;

                DebugUtils.Log($"page to stack = {toStackPage.name} layerInfos.Count = {layerInfos.Count}");
                foreach (LayerController lay in layerInfos)
                {
                    string info = lay.ToDefaultLayer(toStackPage.pageNumb, toStackPage.pageType);
                    //DebugUtil.Log(info);
                }
            }

        }

        Coroutine corToTop;
        /// <summary>
        /// ��ʱ��OnOpen�ӿڵ��ú�,�ŵ���OnTop�ӿ�
        /// </summary>
        /// <param name="toTopPage"></param>
        /// <returns></returns>
        IEnumerator SetPageToTop(PageBase toTopPage)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            corToTop = null;
            toTopPage.OnTop();

            // ҳ���ö��¼�֪ͨ(����ĳЩ�����������ʱ�������������棬���³�����Ч�ز���)
            EventCenter.Instance.EventTrigger<EventData>("ON_PAGE_EVENT", new EventData<PageName>("PageOnTopChange", toTopPage.pageName));
        }

        public void OutputPageStack(PageBase cmpPage)
        {
            int indexStackPage = -2;
            try
            {
                indexStackPage = pagesStack.IndexOf(cmpPage.gameObject);
                pagesStack.RemoveAt(indexStackPage);
            }
            catch (Exception e)
            {
                DebugUtils.LogError($" indexStackPage = {indexStackPage} PageName = {cmpPage.pageName}");
                DebugUtils.LogException(e);
            }


            if(indexStackPage == 0 && pagesStack.Count>0)
            {
                PageBase nowPage = pagesStack[0].transform.GetComponent<PageBase>();
                foreach (LayerController lay in nowPage.layerInfos)
                {
                    lay.ToOwnerLayer();
                }
                nowPage.OnTop();

                // ҳ���ö��¼�֪ͨ(����ĳЩ�����������ʱ�������������棬���³�����Ч�ز���)
                EventCenter.Instance.EventTrigger<EventData>("ON_PAGE_EVENT", new EventData<PageName>("PageOnTopChange", nowPage.pageName));
            }

            //���ҳ��
            if (cmpPage.pageNumb == pageNumbs[cmpPage.pageType])
            {
                pageNumbs[cmpPage.pageType]--;
            }

            // reset

            if (cmpPage.isCache)
            {
                cmpPage.transform.SetParent(tfmPageStore);
                foreach (LayerController lay in cmpPage.layerInfos)
                {
                    lay.ToOwnerLayer();
                }
                cmpPage.layerInfos.Clear();
                cmpPage.pageNumb = -1;
            }
            else
                GameObject.Destroy(cmpPage.gameObject);
        }



        public bool ClosePage(PageName name, EventData data = null)
        {
            PageBase page = null;
            if (!pageCacheDict.TryGetValue(name, out page))
            {
                DebugUtils.LogWarning("����δ��: " + name);
                return false;
            }

            page.OnClose(data);
            OutputPageStack(page);
            //panelDict.Remove(name);
            return true;
        }


        public bool ClosePage(PageBase page, EventData data = null)
        {

            /*if (!pageStoreDict.TryGetValue(name, out page))
            {
                DebugUtil.LogWarning("����δ��: " + name);
                return false;
            }*/

            page.OnClose(data);
            OutputPageStack(page);

            return true;
        }

    }






    public partial class PageManager
    {
        Dictionary<PageName, PageBase> pages = new Dictionary<PageName, PageBase>();

        [Button]
        void TestDoPage(PageName pageName = PageName.PageSystemMask)
        {
            if (pages.ContainsKey(pageName))
            {
                if (!pages[pageName].IsOpen())
                {
                    pages.Remove(pageName);

                    TestDoPage(pageName); //���´�
                    return;
                }
                PageManager.Instance.ClosePage(pages[pageName]);
                pages.Remove(pageName);
            }
            else
            {
                PageBase bp = PageManager.Instance.OpenPage(pageName);
                pages.Add(pageName, bp);
            }
        }

    }

}
