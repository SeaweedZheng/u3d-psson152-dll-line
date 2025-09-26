using Game;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PageConsoleDrawLine : PageMachineButtonBase
{

    public Button btnClose;


    GameObject goLine;

    private void Awake()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    private void OnEnable()
    {
        //GameObject go = ResourceManager.Instance.LoadD<GameObject>("Games/Console001/Prefabs/Buttons/Draw Line Controller.prefab"); // 不能用
        /*
        #if UNITY_EDITOR
                goLine = Instantiate(go);
        #else
                goLine = go;
        #endif
        */

        goLine = ResourceManager.Instance.LoadAssetAtPathOnce<GameObject>("Assets/GameRes/Games/Console001/Draw Line Bundle/Prefabs/Draw Line Controller.prefab");
        // goLine = ResMgr1001.Instance.Load<GameObject>("Games/Console001/Draw Line/Prefabs Bundle/Draw Line Controller");
        goLine.name = "Draw Line Controller (Clone)";

        //DebugUtil.Log($" root == {transform.root.name}");
        //goLine.transform.SetParent(transform.root);

        /* 将新创建的GameObject添加到当前场景中*/
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(goLine, currentScene);
    }

    private void OnDisable()
    {
        GameObject.Destroy(goLine); 
    }



    void OnClose() => PageManager.Instance.ClosePage(this);
    public override void OnClickMachineButton(MachineButtonInfo info)
    {
        if (info != null)
        {
            switch (info.btnKey)
            {
                case MachineButtonKey.BtnSpin:
                    ShowUIAminButtonClick(btnClose, info);
                    break;
            }
        }
    }
}
