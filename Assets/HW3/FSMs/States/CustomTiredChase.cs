using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTiredChaseState : State
{
    //Set name of this state
    public CustomTiredChaseState() : base("CustomTiredChase") { }

    public override State Update(FSMAgent agent)
    {
        //Handle Following Pacman
        Vector3 pacmanLocation = PacmanInfo.Instance.transform.position;
        if (agent.CloseEnough(pacmanLocation))
        {
            ScoreHandler.Instance.KillPacman();
        }

        //If timer complete, go to Scatter State
        if (agent.TimerComplete())
        {
            return new ScatterState(new Vector3(-ObstacleHandler.Instance.Width, -ObstacleHandler.Instance.Height), new CustomHuntState());
        }

        //If Pacman ate a power pellet, go to Frightened State
        if (PelletHandler.Instance.JustEatenPowerPellet)
        {
            return new FrightenedState(this);
        }
        //If we didn't return follow Pacman
        agent.SetTarget(pacmanLocation);

        //Stay in this state
        return this;
    }

    //Upon entering state, set timer to enter Scatter State
    public override void EnterState(FSMAgent agent)
    {
        agent.SetTimer(5f);
        agent.SetSpeedModifierHalf();
    }

    public override void ExitState(FSMAgent agent) {
        agent.SetSpeedModifierNormal();
    }
}
