using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;





public class OdinExample : MonoBehaviour
{
    public enum GSOutType { None, Music, SFX, Voice }

    // 基本属性展示
    [Title("基本设置", titleAlignment: TitleAlignments.Centered)]
    [InfoBox("这是一个使用Odin Inspector的示例脚本", InfoMessageType.Info)]
    public string playerName = "玩家";

    [BoxGroup("游戏设置")]
    [Range(1, 100)]
    [Tooltip("控制角色移动速度")] //Odin 3.x+
    public float moveSpeed = 5f;

    [BoxGroup("游戏设置")]
    [ProgressBar(0, 100, ColorMember = "GetHealthColor")]
    public float health = 80f;

    // 条件显示示例
    //[Space(20)]  Odin 3.x+
    [Title("条件显示示例")]
    public GSOutType outputType;

    [ShowIf("outputType", GSOutType.Music)]
    [HideLabel]
    [Title("音乐配置")]
    [FolderPath(RequireExistingPath = true)]
    public string musicFolderPath;

    [ShowIf("outputType", GSOutType.Music)]
    [Range(0, 1)]
    public float musicVolume = 0.8f;

    // 动态显示/隐藏组
    //[Space(20)] Odin 3.x+
    [Title("高级功能")]
    public bool enableAdvancedOptions;

    [ShowIf("enableAdvancedOptions")]
    [TabGroup("高级设置")]
    public Vector3 spawnPosition;

    [ShowIf("enableAdvancedOptions")]
    [TabGroup("高级设置")]
    [EnumToggleButtons]
    public LayerMask collisionLayers;

    // 自定义绘制器示例
    //[Space(20)] Odin 3.x+
    [Title("自定义绘制")]
    [CustomValueDrawer("DrawColorPicker")]
    public Color customColor = Color.red;

    // 列表和字典示例
    //[Space(20)] Odin 3.x+
    [Title("集合类型")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "itemName")]
    public List<InventoryItem> inventory = new List<InventoryItem>();

    [DictionaryDrawerSettings(KeyLabel = "效果名称", ValueLabel = "效果值")]
    public Dictionary<string, float> effects = new Dictionary<string, float>();

    // 内联编辑器示例
    //[Space(20)] Odin 3.x+
    [Title("内联编辑器")]
    [InlineEditor(InlineEditorModes.FullEditor)]
    public Equipment equippedWeapon;

    // 按钮和动作
    //[Space(20)] Odin 3.x+
    [Title("操作")]
    [Button(ButtonSizes.Large)]
    [PropertyOrder(-1)]
    private void InitializePlayer()
    {
        Debug.Log("初始化玩家...");
    }

    [ButtonGroup]
    private void SaveSettings()
    {
        Debug.Log("保存设置");
    }

    [ButtonGroup]
    private void LoadSettings()
    {
        Debug.Log("加载设置");
    }

    // 验证属性
    //[Space(20)] Odin 3.x+
    [Title("验证")]
    [ValidateInput("ValidateName", "名称不能包含特殊字符!")]
    public string characterName = "英雄";

    // 仅在播放模式下显示
    //[Space(20)] Odin 3.x+
    [Title("运行时数据")]
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
    // 自定义绘制器方法
    private Color DrawColorPicker(Color value, GUIContent label)
    {
        return UnityEditor.EditorGUILayout.ColorField(label, value);
    }*/

    // 验证方法
    private bool ValidateName(string name)
    {
        return !name.Any(c => !char.IsLetterOrDigit(c) && c != ' ');
    }

    // 用于进度条的颜色方法
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
    [LabelText("物品名称")]
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