using UnityEngine;

public class MaskIconRotate : MonoBehaviour
{
    [Tooltip("旋转速度（度/秒）")]
    public float rotationSpeed = 30f;

    [Tooltip("旋转轴（默认Z轴，适合2D/UI）")]
    public Vector3 rotationAxis = Vector3.forward;

    void Update()
    {
        // 每帧旋转（Time.deltaTime确保帧率无关）
        transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
    }
}
