using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PssOn00152
{


    /// <summary>
    /// CustomBlackboard
    /// </summary>
    /// <remarks>
    /// * 只在游戏中读取的“不修改的配置”写入CustomBlackboard.js
    /// * 需要在“游戏中”或“不进入游戏直接进入后台”，读取的“不修改的配置”写在game_info_g152.json
    /// * 需要动态修改且保存的配置数据，写到本地sqlite
    /// </remarks>

    public class CustomBlackboard : MonoWeakSelectSingleton<CustomBlackboard>, ICustomBlackboard
    {
        /// <summary> 图标宽 </summary>
        public float symbolWidth => 183.6f;

        /// <summary> 图标高 </summary>
        public float symbolHeight => 243f;

        /// <summary> 列 </summary>
        public int column => 5;

        /// <summary> 行 </summary>
        public int row => 3;

        public float reelMaxOffsetY
        {
            get => symbolHeight * row;
        }

        /// <summary> 说明页 </summary>
        public List<GameObject> paytable = new List<GameObject>();


        /// <summary> 通过图标索引，获取图标真实编号 </summary>
        public List<int> symbolNumber => new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        /// <summary> 所有图标个数 </summary>
        public int symbolCount => symbolNumber.Count;

        /// <summary> 资源根目录路径 </summary>
        //public string gameAssetsRootFolder = "Assets/GameRes/Games/PssOn00152 (1080x1920)";

        /// <summary> 预制体名称 - 图标中奖特效</summary>
        public Dictionary<string, string> symbolHitEffect => new Dictionary<string, string>
        {
            //{0, "Symbol0 Wild" },

            {"1", "Pool Symbol Hit 1 Ruyi" },
            {"2", "Pool Symbol Hit 2 Lucky Bags" },
            {"3", "Pool Symbol Hit 3 Red Packet" },
            {"4", "Pool Symbol Hit 4 Firecrackers" },
            {"5", "Pool Symbol Hit 5 Tangerine" },
            {"6", "Pool Symbol Hit 6 A" },
            {"7", "Pool Symbol Hit 7 K" },
            {"8", "Pool Symbol Hit 8 Q" },
            {"9", "Pool Symbol Hit 9 J" },
            {"10", "Pool Symbol Hit 10 No10" },
            {"11", "Pool Symbol Hit 11 God" },
            {"12", "Pool Symbol Hit 12 Jackpot" },
        };

        /// <summary>
        /// 特殊图标
        /// </summary>
        /// <param name="index"></param>
        /// <remarks>
        /// * 中线时，播放的动画效果和普通的牌不一样。
        /// </remarks>
        /// <returns></returns>
        public List<int> specialHitSymbols => new List<int> { 1, 2, 3, 4, 5, 11,12 };


        /// <summary> 特效图标 - 预制体名称</summary>
        /// <remarks>
        /// * 特效图标，滚轮停止时，会播放动画特效的图标。
        /// </remarks>
        public Dictionary<string, string> symbolAppearEffect => new Dictionary<string, string>
        {
            {"11", "Pool Symbol Appear 11 God" },
            {"12", "Pool Symbol Appear 12 Jackpot" },
        };


        public Dictionary<string, string> symbolExpectationEffect => new Dictionary<string, string>
        {

        };



        /// <summary> 预制体名称 - 边框特效</summary>
        public string borderEffect => "Border";

        /// <summary> 图片 - 默认图标</summary>
        public Sprite[] symbolIcon;

    }
}