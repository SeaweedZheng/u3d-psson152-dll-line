using UnityEngine;

public class ExampleConditionalShow : MonoBehaviour
{
    public enum Mode { A, B, C }
    public Mode currentMode;

    [ConditionalShow("currentMode", Mode.B)] // 仅当 currentMode == Mode.B 时显示
    public Vector3 positionWhenModeB;

    public int threshold = 5;

    [ConditionalShow("threshold", 10)] // 仅当 threshold == 10 时显示
    public string message;

    public string password = "secret";

    [ConditionalShow("password", "admin")] // 仅当 password == "admin" 时显示
    public bool isAdmin;
}
