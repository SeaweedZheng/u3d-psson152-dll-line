using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;





public class OdinExample : MonoBehaviour
{
    public enum GSOutType { None, Music, SFX, Voice }

    // ��������չʾ
    [Title("��������", titleAlignment: TitleAlignments.Centered)]
    [InfoBox("����һ��ʹ��Odin Inspector��ʾ���ű�", InfoMessageType.Info)]
    public string playerName = "���";

    [BoxGroup("��Ϸ����")]
    [Range(1, 100)]
    [Tooltip("���ƽ�ɫ�ƶ��ٶ�")] //Odin 3.x+
    public float moveSpeed = 5f;

    [BoxGroup("��Ϸ����")]
    [ProgressBar(0, 100, ColorMember = "GetHealthColor")]
    public float health = 80f;

    // ������ʾʾ��
    //[Space(20)]  Odin 3.x+
    [Title("������ʾʾ��")]
    public GSOutType outputType;

    [ShowIf("outputType", GSOutType.Music)]
    [HideLabel]
    [Title("��������")]
    [FolderPath(RequireExistingPath = true)]
    public string musicFolderPath;

    [ShowIf("outputType", GSOutType.Music)]
    [Range(0, 1)]
    public float musicVolume = 0.8f;

    // ��̬��ʾ/������
    //[Space(20)] Odin 3.x+
    [Title("�߼�����")]
    public bool enableAdvancedOptions;

    [ShowIf("enableAdvancedOptions")]
    [TabGroup("�߼�����")]
    public Vector3 spawnPosition;

    [ShowIf("enableAdvancedOptions")]
    [TabGroup("�߼�����")]
    [EnumToggleButtons]
    public LayerMask collisionLayers;

    // �Զ��������ʾ��
    //[Space(20)] Odin 3.x+
    [Title("�Զ������")]
    [CustomValueDrawer("DrawColorPicker")]
    public Color customColor = Color.red;

    // �б���ֵ�ʾ��
    //[Space(20)] Odin 3.x+
    [Title("��������")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "itemName")]
    public List<InventoryItem> inventory = new List<InventoryItem>();

    [DictionaryDrawerSettings(KeyLabel = "Ч������", ValueLabel = "Ч��ֵ")]
    public Dictionary<string, float> effects = new Dictionary<string, float>();

    // �����༭��ʾ��
    //[Space(20)] Odin 3.x+
    [Title("�����༭��")]
    [InlineEditor(InlineEditorModes.FullEditor)]
    public Equipment equippedWeapon;

    // ��ť�Ͷ���
    //[Space(20)] Odin 3.x+
    [Title("����")]
    [Button(ButtonSizes.Large)]
    [PropertyOrder(-1)]
    private void InitializePlayer()
    {
        Debug.Log("��ʼ�����...");
    }

    [ButtonGroup]
    private void SaveSettings()
    {
        Debug.Log("��������");
    }

    [ButtonGroup]
    private void LoadSettings()
    {
        Debug.Log("��������");
    }

    // ��֤����
    //[Space(20)] Odin 3.x+
    [Title("��֤")]
    [ValidateInput("ValidateName", "���Ʋ��ܰ��������ַ�!")]
    public string characterName = "Ӣ��";

    // ���ڲ���ģʽ����ʾ
    //[Space(20)] Odin 3.x+
    [Title("����ʱ����")]
    [ShowInInspector, ReadOnly]
    [ShowIf("Application.isPlaying")]
    private float runtimeValue;

    private void Update()
    {
        if (Application.isPlaying)
        {
            runtimeValue = Mathf.Sin(Time.time) * 100;
        }
    }
    /*
    // �Զ������������
    private Color DrawColorPicker(Color value, GUIContent label)
    {
        return UnityEditor.EditorGUILayout.ColorField(label, value);
    }*/

    // ��֤����
    private bool ValidateName(string name)
    {
        return !name.Any(c => !char.IsLetterOrDigit(c) && c != ' ');
    }

    // ���ڽ���������ɫ����
    private Color GetHealthColor()
    {
        if (health > 70) return Color.green;
        if (health > 30) return Color.yellow;
        return Color.red;
    }
}

[System.Serializable]
public class InventoryItem
{
    [LabelText("��Ʒ����")]
    public string itemName;

    [Range(1, 99)]
    public int quantity;

    [PreviewField]
    public Sprite icon;
}

[System.Serializable]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    public int damage;
    public float weight;
}