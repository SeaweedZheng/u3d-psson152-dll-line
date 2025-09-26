//https://blog.csdn.net/ChinarCSDN/article/details/82263126
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition = 0,
    Launch,
    Prepare,
    Bet,
    Roll,
    RollLittleGame,
    JP,
    SuperTimes,
    FreeRoll,
    AllPick,
    Jackpot,
    Dragon,
    RollRole,
    Reward,
    Controller,
    Gaming,
}

public enum StateID
{
    NullStateID = 0,
    InitSBox,
    Launch,
    Prepare,
    Bet,
    Roll,
    RollLittleGame,
    JP,
    SuperTimes,
    FreeRoll,
    AllPick,
    Jackpot,
    Dragon,
    RollRole,
    Reward,
    Controller,
    Gaming,
}

public abstract class FSMState
{
    public Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }


    public void AddTransition(Transition transition, StateID id)
    { 
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        if (map.ContainsKey(transition))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + transition.ToString() + 
                                          "Impossible to assign to another state");
            return;
        }

        map.Add(transition, id);
    }

    public void DeleteTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        if (map.ContainsKey(transition))
        {
            map.Remove(transition);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + transition.ToString() + " passed to " + stateID.ToString() + 
                                                 " was not on the state's transition list");
    }

    public StateID GetOutputState(Transition transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        return StateID.NullStateID;
    }

    public virtual void DoBeforeEntering() { }

    public virtual void DoBeforeLeaving() { }

    public virtual void Reason() { }

    public virtual void Act() { }
    
}
