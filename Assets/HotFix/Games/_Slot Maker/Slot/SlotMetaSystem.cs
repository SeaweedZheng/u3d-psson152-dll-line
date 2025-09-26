using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 目前没用上
/// </summary>
public class SlotMetaSystem : MonoSingleton<SlotMetaSystem>
{
    ISlotMetaSystem metaSystem;
    public static void InitializeMetaSystem(ISlotMetaSystem metaSystem)
    {
        Instance.metaSystem = metaSystem;
    }

}
public interface ISlotMetaSystem
{
    void SelectGame(int gameId);
    void EnterGame();
    void ClearContentData();
}