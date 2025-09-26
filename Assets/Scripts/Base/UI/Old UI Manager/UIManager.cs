using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UIConst;

namespace Game
{
    public enum UILayer
    {
        SceneLayer = 1000,
        BackgroundLayer = 2000,
        NormalLayer = 3000,
        InfoLayer = 4000,
        TopLayer = 5000,
        TipLayer = 6000,
        BlackMaskLayer = 7000,
    }
    public class UIManager
    {
        private static UIManager _instance;
        private Transform _uiRoot;
        // 预制件缓存字典
        private Dictionary<PageName, GameObject> prefabDict;
        // 已打开界面的缓存字典
        public Dictionary<PageName, BaseView> panelDict;

        private Dictionary<int, GameObject> layerDict;

        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIManager();
                }
                return _instance;
            }
        }

        public Transform UIRoot
        {
            get
            {
                if (_uiRoot == null)
                {
                    _uiRoot = GameObject.Find("UIRoot").transform;
                    return _uiRoot;
                };
                return _uiRoot;
            }
        }

        private UIManager()
        {
            InitDicts();
        }

        private void InitDicts()
        {
            prefabDict = new Dictionary<PageName, GameObject>();
            panelDict = new Dictionary<PageName, BaseView>();
            layerDict = new Dictionary<int, GameObject>();

            GameObject g = GameObject.Find("UIRoot/SceneLayer");
            layerDict.Add((int)UILayer.SceneLayer, g);
            g = GameObject.Find("UIRoot/BackgroundLayer");
            layerDict.Add((int)UILayer.BackgroundLayer, g);
            g = GameObject.Find("UIRoot/NormalLayer");
            layerDict.Add((int)UILayer.NormalLayer, g);
            g = GameObject.Find("UIRoot/InfoLayer");
            layerDict.Add((int)UILayer.InfoLayer, g);
            g = GameObject.Find("UIRoot/TopLayer");
            layerDict.Add((int)UILayer.TopLayer, g);
            g = GameObject.Find("UIRoot/TipLayer");
            layerDict.Add((int)UILayer.TipLayer, g);
            g = GameObject.Find("UIRoot/BlackMaskLayer");
            layerDict.Add((int)UILayer.BlackMaskLayer, g);
        }

        public BaseView OpenPanel(PageName panelName)
        {
            string name = Enum.GetName(typeof(PageName), panelName);

            BaseView panel = null;
            // 检查是否已打开
            if (panelDict.TryGetValue(panelName, out panel))
            {
                if (!panel.PanelIsOpen())
                {
                    panel.OpenPanel(name);
                }
                return panel;
            }

            // 检查路径是否配置
            string path = "";
            if (!UIConst.Instance.pathDict.TryGetValue(panelName, out path))
            {
                Debug.LogError("界面名称错误，或未配置路径: " + panelName);
                return null;
            }

            GameObject panelPrefab = null;
            if (!prefabDict.TryGetValue(panelName, out panelPrefab))
            {
                string realPath = path;
                //panelPrefab = ResMgr1001.Instance.Load<GameObject>(realPath);

                panelPrefab = new GameObject(realPath);
                prefabDict.Add(panelName, panelPrefab);
            }

            // 打开界面
            GameObject panelObject = panelPrefab;
            panelObject.transform.SetParent(UIRoot);
            panel = panelObject.GetComponent<BaseView>();
            SetLayer(panel, panel.layer);
            panelDict.Add(panelName, panel);
            panel.OpenPanel(name);
            return panel;
        }

        public void SetLayer(BaseView basePanel, UILayer layer)
        {
            GameObject o;
            if (layerDict.TryGetValue((int)layer, out o))
            {
                basePanel.transform.SetParent(o.transform);
                basePanel.ResetScaleAndPos();
            }
        }

        public bool ClosePanel(PageName name)
        {
            BaseView panel = null;
            if (!panelDict.TryGetValue(name, out panel))
            {
                Debug.LogError("界面未打开: " + name);
                return false;
            }

            panel.ClosePanel();
            //panelDict.Remove(name);
            return true;
        }

    }

}