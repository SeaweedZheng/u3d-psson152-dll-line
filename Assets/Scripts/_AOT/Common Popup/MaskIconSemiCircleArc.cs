using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MaskIconSemiCircleArc : Graphic
{
    [Range(0, 360)] public float arcAngle = 180f;
    public int segments = 50;
    public float thickness = 5f;
    public Color startColor = Color.white; // ��ʼ��ɫ����͸����
    public Color endColor = new Color(1, 1, 1, 0.5f); // ������ɫ����͸����

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float radius = rectTransform.rect.width * 0.5f;
        Vector2 center = rectTransform.rect.center;
        float angleStep = arcAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -90f + i * angleStep; // ��-90�ȿ�ʼ��12�㷽��
            float rad = angle * Mathf.Deg2Rad;

            // ���㽥�������0��1��
            float t = (float)i / segments;
            Color currentColor = Color.Lerp(startColor, endColor, t);

            // �⻡���㣨����ɫ��
            Vector2 outerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            vh.AddVert(outerPos, currentColor, Vector2.zero);

            // �ڻ����㣨����ɫ��
            Vector2 innerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (radius - thickness);
            vh.AddVert(innerPos, currentColor, Vector2.zero);
        }

        // ����������
        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 2;
            vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
            vh.AddTriangle(baseIndex + 2, baseIndex + 1, baseIndex + 3);
        }
    }
}










/*
[RequireComponent(typeof(Image))]
public class SemiCircleArc : Graphic
{
    [Range(0, 360)] public float arcAngle = 180f; // ��Բ���Ƕ�
    public int segments = 50; // ���߷ֶ�����Խ��Խƽ����
    public float thickness = 5f; // ���ߴ�ϸ

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float radius = rectTransform.rect.width * 0.5f;
        Vector2 center = rectTransform.rect.center;

        // ����ÿ�εĽǶȲ���
        float angleStep = arcAngle / segments;

        // ���ɶ���
        for (int i = 0; i <= segments; i++)
        {
            float angle = -90f + i * angleStep; // ��-90�ȿ�ʼ��12�㷽��
            float rad = angle * Mathf.Deg2Rad;

            // �⻡����
            Vector2 outerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            // �ڻ����㣨���ƴ�ϸ��
            Vector2 innerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (radius - thickness);

            // ��Ӷ��㣨�⻡���ڻ����棩
            vh.AddVert(outerPos, color, Vector2.zero);
            vh.AddVert(innerPos, color, Vector2.zero);
        }

        // ���������Σ����Ӷ��㣩
        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 2;
            vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2); // �⻡-�ڻ�-��һ���⻡
            vh.AddTriangle(baseIndex + 2, baseIndex + 1, baseIndex + 3); // ��һ���⻡-�ڻ�-��һ���ڻ�
        }
    }
}

*/