//https://blog.csdn.net/ChinarCSDN/article/details/82263126
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem : BaseManager<FSMSystem>
{
    private List<FSMState> states;
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void SetCurrentState(FSMState state)
    {
        currentState = state;
        currentStateID = state.ID;
        state.DoBeforeEntering();
    }

    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        if (states.Count == 0)
        {
            states.Add(state);
            return;
        }

        foreach (FSMState s in states)
        {
            if (s.ID == state.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + state.ID.ToString() + 
                                                  " because state has already been added");
                return;
            }
        }
        states.Add(state);
    }

    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() + 
                                                     ". It was not on the list of states");
    }

    public void PerformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        StateID id = currentState.GetOutputState(transition);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state for transition " + transition.ToString());
            return;
        }

        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();
                currentState = state;
                currentState.DoBeforeEntering();
                break;
            }
        }

        //MsgInfo msgInfo = new MsgInfo()
        //{
        //    cmd = (int)S2C_CMD.S2C_State,
        //    id = -1,
        //    jsonData = JsonConvert.SerializeObject(new CurrentStateID() { stateID = (int)currentStateID })
        //};
        //NetMgr.Instance.SendToAllClient(JsonConvert.SerializeObject(msgInfo));

        Debug.Log($"{transition} To {currentState}");
    }
}
