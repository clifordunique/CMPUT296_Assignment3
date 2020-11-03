using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomExcitedChaseState : State
{
    //Set name of this state
    public CustomExcitedChaseState() : base("CustomExcitedChase") { }

    public override State Update(FSMAgent agent)
    {
        //Handle Following Pacman
        Vector3 pacmanLocation = PacmanInfo.Instance.transform.position;
        if (agent.CloseEnough(pacmanLocation))
        {
            ScoreHandler.Instance.KillPacman();
        }

        //If timer complete, go to Scatter State
        //Unfortunately this ghost is too good to lose sight of you most of the time, but if you want you can add this to make it harder
        //if (agent.TimerComplete() && ObstacleHandler.Instance.AnyIntersect(pacmanLocation,agent.GetPosition()))
        if (agent.TimerComplete())
        {
            return new CustomTiredChaseState();
        }

        //If Pacman ate a power pellet, go to Frightened State
        if (PelletHandler.Instance.JustEatenPowerPellet)
        {
            return new FrightenedState(new CustomChaseState());
        }

        //If we didn't return follow Pacman
        agent.SetTarget(pacmanLocation);

        //Stay in this state
        return this;
    }

    //Upon entering state, set timer to enter Scatter State
    public override void EnterState(FSMAgent agent)
    {
        agent.SetTimer(2f);
        agent.SetSpeedModifierDouble();
    }

    public override void ExitState(FSMAgent agent)
    {
        agent.SetSpeedModifierNormal();
    }
}
