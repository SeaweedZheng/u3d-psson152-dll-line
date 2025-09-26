using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using _customBB = PssOn00152.CustomBlackboard;
//using _contentBB = PssOn00152.ContentBlackboard;


public class PayTableLineSegments : MonoBehaviour
{


    public float spacingXY = 2f;
    public float cellSizeXY = 14f;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.white;

    public int lineIdx = 0;

    public GameObject goTarget;
    // Text txtComp => goTarget.GetComponent<Text>();
    // TextMeshProUGUI tmpComp => goTarget.GetComponent<TextMeshProUGUI>();

    GridLayoutGroup glgComp;
    int column, row;


    protected bool isHaveInit = false;


    void Awake()
    {
        if (goTarget == null)
        {
            goTarget = gameObject;
        }

        glgComp = goTarget.GetComponent<GridLayoutGroup>();

        //float width = cellSizeXY;
        float width = cellSizeXY + cellSizeXY * 0.3f;

        glgComp.cellSize = new Vector2(width, cellSizeXY);
        glgComp.spacing = new Vector2(spacingXY, spacingXY);


        column = BlackboardUtils.GetValue<int>(null, "@customData/column");
        row = BlackboardUtils.GetValue<int>(null, "@customData/row");

        transform.GetComponent<RectTransform>().sizeDelta =
            new Vector2(column * width + (column - 1) * spacingXY,
            row * cellSizeXY + (row - 1) * spacingXY);

    }


    protected virtual void OnEnable()
    {
        if (isHaveInit)
            return;

        List<List<int>> payLines = BlackboardUtils.GetValue<List<List<int>>>(null, "./payLines");

        List<int> pauLine = payLines[lineIdx];

        int allCount = column * row;

        GameObject goItem = goTarget.transform.GetChild(0).gameObject;
        int count = goTarget.transform.childCount;
        for (int i = count; i < allCount; i++)
        {
            GameObject go = Instantiate(goItem);
            go.transform.SetParent(goTarget.transform, false);
        }
        foreach (Transform tfm in goTarget.transform)
        {
            tfm.gameObject.SetActive(false);
        }

        List<List<Image>> imgsColRow = new List<List<Image>>();
        for (int i = 0; i < column; i++)
        {
            imgsColRow.Add(new List<Image>());
        }

        for (int i = 0; i < allCount; i++)
        {
            Transform tfm = goTarget.transform.GetChild(i);
            tfm.gameObject.SetActive(true);
            int colIdx = i % column;
            Image img = tfm.GetComponent<Image>();
            img.color = normalColor;
            imgsColRow[colIdx].Add(img);
        }
        for (int i = 0; i < pauLine.Count; i++)
        {
            imgsColRow[i][pauLine[i]].color = highlightColor;
        }

        isHaveInit = true;
    }
}
