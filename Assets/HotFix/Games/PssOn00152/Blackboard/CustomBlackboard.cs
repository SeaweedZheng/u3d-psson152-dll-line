using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PssOn00152
{


    /// <summary>
    /// CustomBlackboard
    /// </summary>
    /// <remarks>
    /// * ֻ����Ϸ�ж�ȡ�ġ����޸ĵ����á�д��CustomBlackboard.js
    /// * ��Ҫ�ڡ���Ϸ�С��򡰲�������Ϸֱ�ӽ����̨������ȡ�ġ����޸ĵ����á�д��game_info_g152.json
    /// * ��Ҫ��̬�޸��ұ�����������ݣ�д������sqlite
    /// </remarks>

    public class CustomBlackboard : MonoWeakSelectSingleton<CustomBlackboard>, ICustomBlackboard
    {
        /// <summary> ͼ��� </summary>
        public float symbolWidth => 183.6f;

        /// <summary> ͼ��� </summary>
        public float symbolHeight => 243f;

        /// <summary> �� </summary>
        public int column => 5;

        /// <summary> �� </summary>
        public int row => 3;

        public float reelMaxOffsetY
        {
            get => symbolHeight * row;
        }

        /// <summary> ˵��ҳ </summary>
        public List<GameObject> paytable = new List<GameObject>();


        /// <summary> ͨ��ͼ����������ȡͼ����ʵ��� </summary>
        public List<int> symbolNumber => new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        /// <summary> ����ͼ����� </summary>
        public int symbolCount => symbolNumber.Count;

        /// <summary> ��Դ��Ŀ¼·�� </summary>
        //public string gameAssetsRootFolder = "Assets/GameRes/Games/PssOn00152 (1080x1920)";

        /// <summary> Ԥ�������� - ͼ���н���Ч</summary>
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
        /// ����ͼ��
        /// </summary>
        /// <param name="index"></param>
        /// <remarks>
        /// * ����ʱ�����ŵĶ���Ч������ͨ���Ʋ�һ����
        /// </remarks>
        /// <returns></returns>
        public List<int> specialHitSymbols => new List<int> { 1, 2, 3, 4, 5, 11,12 };


        /// <summary> ��Чͼ�� - Ԥ��������</summary>
        /// <remarks>
        /// * ��Чͼ�꣬����ֹͣʱ���Ქ�Ŷ�����Ч��ͼ�ꡣ
        /// </remarks>
        public Dictionary<string, string> symbolAppearEffect => new Dictionary<string, string>
        {
            {"11", "Pool Symbol Appear 11 God" },
            {"12", "Pool Symbol Appear 12 Jackpot" },
        };


        public Dictionary<string, string> symbolExpectationEffect => new Dictionary<string, string>
        {

        };



        /// <summary> Ԥ�������� - �߿���Ч</summary>
        public string borderEffect => "Border";

        /// <summary> ͼƬ - Ĭ��ͼ��</summary>
        public Sprite[] symbolIcon;

    }
}