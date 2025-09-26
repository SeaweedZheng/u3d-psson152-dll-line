using UnityEngine;

public class MaskIconRotate : MonoBehaviour
{
    [Tooltip("��ת�ٶȣ���/�룩")]
    public float rotationSpeed = 30f;

    [Tooltip("��ת�ᣨĬ��Z�ᣬ�ʺ�2D/UI��")]
    public Vector3 rotationAxis = Vector3.forward;

    void Update()
    {
        // ÿ֡��ת��Time.deltaTimeȷ��֡���޹أ�
        transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
    }
}
