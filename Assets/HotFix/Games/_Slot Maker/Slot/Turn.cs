using GameMaker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMaker
{
    public enum ContentNodeType
    {
        Turn,
        Spin,
        Bonus
    };

    [Serializable]
    public class ContentNode
    {

        public ContentNodeType type;

        public string name;

        public string uid; //Guid.NewGuid().ToString();

        public long beginTime;

        public long endTime;


        /// <summary>
        /// 单轮游戏，多次玩赢的钱，累加！
        /// </summary>
        public long earnCredit;

        public ContentNode()
        {
            uid = Guid.NewGuid().ToString();
            beginTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }


        public static void AddEarnCredit(ContentNode node, long earnCredit)
        {
            if (node.type == ContentNodeType.Spin )
            {
                (node as Spin).singleCredit += earnCredit;
            }else if (node.type == ContentNodeType.Bonus)
            {
                (node as Bonus).singleCredit += earnCredit;
            }

            node.earnCredit += earnCredit;

            ContentNode parent = node.parent;            
            while (parent != null)
            {
                parent.earnCredit += earnCredit;
                parent = parent.parent;
            }

            // BlackboardUtils.FindVariable<long>(null, "./game/earnCredit").value += earnCredit;
        }

        public ContentNode parent;
    }

    [Serializable]
    public class GameInfo
    {
        /// <summary> 玩了多少轮 </summary>
        public int turnCount;

        /// <summary> 已经赢了多少分 </summary>
        public long earnCredit;

        /// <summary> 已经花费多少钱 </summary>
        public long spentCredit;

    }


    [Serializable]
    public class Turn: ContentNode
    {
        public int turnIndex;

        /// <summary> 压注金额 </summary>
        public long totalBetCredit;

        /// <summary> 玩家开始金额 </summary>
        public long beginCrediy;

        /// <summary> 玩家已花费的金额 </summary>
        public long spentCrediy;


        public Spin spin;


        public Turn() : base()
        {
            type = ContentNodeType.Turn;
        }

        public static Turn BeginTurn()
        {
            Turn turn = new Turn();
            Variable<List<Turn>> vTurnLst = BlackboardUtils.FindVariable<List<Turn>>(null, "./history");
            vTurnLst.value.Insert(0, turn);
            while (vTurnLst.value.Count>5)
            {
                vTurnLst.value.RemoveAt(vTurnLst.value.Count -1);
            }

            turn.beginCrediy = BlackboardUtils.GetValue<long>(null, "@console/myCredit");
            turn.totalBetCredit = BlackboardUtils.GetValue<long>(null, "./totalBet");


            bool isFreeSpin = false;
            turn.spentCrediy = isFreeSpin ? 0: turn.totalBetCredit;
            //AddSpentCredit(turn.totalBetCredit);

            turn.earnCredit = 0L;


            BlackboardUtils.SetValue<ContentNode>(null, "./current", turn);
            BlackboardUtils.SetValue<Turn>(null, "./turn",turn);

            return turn;
        }

        public static void EndTurn()
        {
            Variable<Turn> vTurn = BlackboardUtils.FindVariable<Turn>(null, "./turn");
            vTurn.value.endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            BlackboardUtils.SetValue<ContentNode>(null, "./current", null);
            BlackboardUtils.SetValue<Turn>(null, "./turn", null);
        }

        public static void AddSpentCredit(Turn turn , long spentCredit)
        {
            turn.totalBetCredit += spentCredit;
            long credit = BlackboardUtils.GetValue<long>(null, "./game/spentCredit");
            BlackboardUtils.SetValue<long>(null, "./game/spentCredit", credit + spentCredit);
        }
    }

    [Serializable]
    public class Bonus : ContentNode
    {
        public int bonusIndex;
        /// <summary> 自身赢分 </summary>

        public long singleCredit;

        public string response;

        public List<Spin> spinList;
        public Bonus() : base()
        {
            type = ContentNodeType.Bonus;
        }


        public static Bonus BeginBonus()
        {
            Bonus bonus = new Bonus();

            Variable<Spin> vSpin = BlackboardUtils.FindVariable<Spin>(null, "./spin");
            Variable<ContentNode> vParent = BlackboardUtils.FindVariable<ContentNode>(null, "./current");

            //vSpin.value).bonusList.Add(bonus);
            //bonus.bonusIndex = vSpin.value.bonusList.Count - 1;
            //bonus.parent = vSpin.value;

            (vParent.value as Spin).bonusList.Add(bonus);
            bonus.bonusIndex = (vParent.value as Spin).bonusList.Count - 1;
            bonus.parent = vParent.value;

            bonus.earnCredit = 0L;
            bonus.singleCredit = 0L;
            BlackboardUtils.SetValue<ContentNode>(null, "./current", bonus);
            BlackboardUtils.SetValue<Bonus>(null, "./bonus", bonus);

            return bonus;
        }
        public static void EndBonus()
        {
            Variable<Bonus> vBonus = BlackboardUtils.FindVariable<Bonus>(null, "./bonus");
            vBonus.value.endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            ContentNode parent = vBonus.value.parent;

            BlackboardUtils.SetValue<ContentNode>(null, "./current", parent);
            //BlackboardUtils.SetValue<Spin>(null, "./bonus", null);


            if (parent.type == ContentNodeType.Spin)
            {
                Spin spin = (Spin)parent;
                BlackboardUtils.SetValue<Spin>(null, "./spin", spin);

                if (spin.parent.type == ContentNodeType.Turn)
                {
                    BlackboardUtils.SetValue<Spin>(null, "./bonus", null);
                }
                else if (spin.parent.type == ContentNodeType.Bonus)
                {
                    BlackboardUtils.SetValue<Bonus>(null, "./bonus", spin.parent as Bonus);
                }
                else
                {
                    DebugUtils.LogError("[Content] Spin circle detected.");
                }
            }
            else if (parent.type == ContentNodeType.Bonus)
            {
                BlackboardUtils.SetValue<Bonus>(null, "./bonus", parent as Bonus);
            }
            else
            {
                DebugUtils.LogError("[Content] EndBonus failed. Bonus could not be placed under Turn.");
                return;
            }

        }

    }


    [Serializable]
    public class Spin : ContentNode
    {
        public int spinIndex;

        public long multiplier;

        /// <summary>
        /// 单次玩赢的钱，累加（多个中奖线、彩金、奖励、）！
        /// </summary>
        public long singleCredit;

        public string response;


        public List<Bonus> bonusList;


        public List<SymbolWin> winList;

        public Spin():base()
        {
            type = ContentNodeType.Spin;
        }

        public static Spin BeginSpin()
        {
            Spin spin = new Spin();

            Variable<ContentNode> vParent = BlackboardUtils.FindVariable<ContentNode>(null, "./current");
            ContentNodeType parentTp = vParent.value.type;

            if (parentTp == ContentNodeType.Turn)
            {
                (vParent.value as Turn).spin = spin;
                spin.spinIndex = 0;
            }
            else if (parentTp == ContentNodeType.Bonus)
            {
                (vParent.value as Bonus).spinList.Add(spin);
                spin.spinIndex = (vParent.value as Bonus).spinList.Count -1;
            }
            else
            {
                DebugUtils.LogError("[Content] BeginSpin failed. Spin always placed under Turn or Bonus.");
                return null;
            }
            spin.parent = vParent.value;

            spin.earnCredit = 0L;
            spin.singleCredit = 0L;

            BlackboardUtils.SetValue<ContentNode>(null, "./current",spin);
            BlackboardUtils.SetValue<Spin>(null, "./spin", spin);

            return spin;
        }



        public static void EndSpin()
        {
            Variable<Spin> vSpin = BlackboardUtils.FindVariable<Spin>(null, "./spin");
            vSpin.value.endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            BlackboardUtils.SetValue<ContentNode>(null, "./current", vSpin.value.parent);
            BlackboardUtils.SetValue<Spin>(null, "./spin", null);
        }

     }

}

