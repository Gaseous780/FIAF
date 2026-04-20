using UnityEngine;

public class FSMClass
{
    private State currentState { get; set; }

    private EvadeState evadeState;
    private ArriveState arriveState;
    private HideState hideState;
    private DisapearState disapearState;

    private int timesSaw;
    private int maxAmountToSeeGhost;


    public State _currentState => currentState;
    public int _timesSaw { get { return timesSaw; } set { timesSaw = value; } }
    public int _maxAmountToSeeGhost => maxAmountToSeeGhost;

    public void InitializeFSM()
    {
        evadeState = new EvadeState(this);
        arriveState = new ArriveState(this);
        hideState = new HideState(this);
        disapearState = new DisapearState(this);

        currentState = arriveState;

        maxAmountToSeeGhost = 3;
    }

    public void ChangeToEvade()
    {
        ChangeState(evadeState);
    }

    public void ChangeToArrive()
    {
        ChangeState(arriveState); 
    }

    public void ChangeToHide()
    {
        ChangeState(hideState);
    }

    public void ChangeToDisapear()
    {
        ChangeState(disapearState);
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateState(bool playerUsingCameras ,bool hasSeen)
    {
        currentState.Update(playerUsingCameras, hasSeen);
    }
}

public abstract class State
{
    protected FSMClass fsm;

    public State (FSMClass fsm)
    {
        this.fsm = fsm;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public abstract void Update(bool condition, bool secondCondition);
}

public class EvadeState : State
{
    public EvadeState (FSMClass fsm) : base(fsm) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit() 
    { 
    
    }

    public override void Update(bool playerUsingCameras, bool hasSeen) //La condición es si el jugador esta usando las cámaras
    {
        if (hasSeen == true)
        {
            fsm.ChangeToHide();
        }
        else if (playerUsingCameras == false)
        {
            fsm.ChangeToArrive();
        }
        else if (hasSeen == true && fsm._timesSaw >= fsm._maxAmountToSeeGhost)
        {
            fsm.ChangeToDisapear();
        }
    }
}

public class ArriveState : State
{
    public ArriveState(FSMClass fsm) : base(fsm) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {

    }

    public override void Update(bool playerUsingCameras, bool hasSeen)
    {
        if (playerUsingCameras == true)
        {
            fsm.ChangeToEvade();
        }
        else if (hasSeen == true)
        {
            fsm.ChangeToHide();
        }
        else if (hasSeen == true && fsm._timesSaw >= fsm._maxAmountToSeeGhost)
        {
            fsm.ChangeToDisapear();
        }
    }
}

public class HideState : State
{
    public HideState(FSMClass fsm) : base(fsm) { }

    public override void Enter()
    {
        fsm._timesSaw++;
    }

    public override void Exit()
    {

    }

    public override void Update(bool playerUsingCameras, bool hasSeen)
    {
        if (playerUsingCameras == true && hasSeen == false)
        {
            fsm.ChangeToEvade();
        }
        else if (playerUsingCameras == false && hasSeen == false)
        {
            fsm.ChangeToArrive();
        }
        else if (hasSeen == true && fsm._timesSaw >= fsm._maxAmountToSeeGhost)
        {
            fsm.ChangeToDisapear();
        }
    }
}

public class DisapearState : State
{
    public DisapearState(FSMClass fsm) : base(fsm) { }

    public override void Enter()
    {
        fsm._timesSaw = 0;
    }

    public override void Exit()
    {

    }

    public override void Update(bool playerUsingCameras, bool hasSeen)
    {

    }
}
