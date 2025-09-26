using UnityEngine;

public class ExampleConditionalShow : MonoBehaviour
{
    public enum Mode { A, B, C }
    public Mode currentMode;

    [ConditionalShow("currentMode", Mode.B)] // ���� currentMode == Mode.B ʱ��ʾ
    public Vector3 positionWhenModeB;

    public int threshold = 5;

    [ConditionalShow("threshold", 10)] // ���� threshold == 10 ʱ��ʾ
    public string message;

    public string password = "secret";

    [ConditionalShow("password", "admin")] // ���� password == "admin" ʱ��ʾ
    public bool isAdmin;
}
