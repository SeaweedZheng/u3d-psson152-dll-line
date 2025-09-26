using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MaskIconSemiCircleArc : Graphic
{
    [Range(0, 360)] public float arcAngle = 180f;
    public int segments = 50;
    public float thickness = 5f;
    public Color startColor = Color.white; // 起始颜色（不透明）
    public Color endColor = new Color(1, 1, 1, 0.5f); // 结束颜色（半透明）

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float radius = rectTransform.rect.width * 0.5f;
        Vector2 center = rectTransform.rect.center;
        float angleStep = arcAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -90f + i * angleStep; // 从-90度开始（12点方向）
            float rad = angle * Mathf.Deg2Rad;

            // 计算渐变比例（0到1）
            float t = (float)i / segments;
            Color currentColor = Color.Lerp(startColor, endColor, t);

            // 外弧顶点（带颜色）
            Vector2 outerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            vh.AddVert(outerPos, currentColor, Vector2.zero);

            // 内弧顶点（带颜色）
            Vector2 innerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (radius - thickness);
            vh.AddVert(innerPos, currentColor, Vector2.zero);
        }

        // 生成三角形
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
    [Range(0, 360)] public float arcAngle = 180f; // 半圆弧角度
    public int segments = 50; // 弧线分段数（越高越平滑）
    public float thickness = 5f; // 弧线粗细

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float radius = rectTransform.rect.width * 0.5f;
        Vector2 center = rectTransform.rect.center;

        // 计算每段的角度步长
        float angleStep = arcAngle / segments;

        // 生成顶点
        for (int i = 0; i <= segments; i++)
        {
            float angle = -90f + i * angleStep; // 从-90度开始（12点方向）
            float rad = angle * Mathf.Deg2Rad;

            // 外弧顶点
            Vector2 outerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            // 内弧顶点（控制粗细）
            Vector2 innerPos = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (radius - thickness);

            // 添加顶点（外弧和内弧交替）
            vh.AddVert(outerPos, color, Vector2.zero);
            vh.AddVert(innerPos, color, Vector2.zero);
        }

        // 生成三角形（连接顶点）
        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 2;
            vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2); // 外弧-内弧-下一段外弧
            vh.AddTriangle(baseIndex + 2, baseIndex + 1, baseIndex + 3); // 下一段外弧-内弧-下一段内弧
        }
    }
}

*/