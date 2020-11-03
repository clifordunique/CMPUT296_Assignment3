using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeChaseState : State
{
    //Set name of this state
    public ClydeChaseState() : base("ClydeChase") { }

    public override State Update(FSMAgent agent)
    {
        //Handle Following Pacman
        Vector3 targetLocation;
        Vector3 pacmanLocation = PacmanInfo.Instance.transform.position;
        if (agent.CloseEnough(pacmanLocation))
        {
            ScoreHandler.Instance.KillPacman();
        }

        //If timer complete, go to Scatter State
        if (agent.TimerComplete())
        {
            return new ScatterState(new Vector3(-ObstacleHandler.Instance.Width, -ObstacleHandler.Instance.Height), this);
        }

        //If Pacman ate a power pellet, go to Frightened State
        if (PelletHandler.Instance.JustEatenPowerPellet)
        {
            return new FrightenedState(this);
        }
        if (Vector3.Distance(pacmanLocation, agent.GetPosition()) < 1.6f)
        {
            targetLocation = new Vector3(-ObstacleHandler.Instance.Width, -ObstacleHandler.Instance.Height);
        }
        //If we didn't return follow Pacman
        else
        {
            targetLocation = pacmanLocation;
        }
        
        GraphNode g = HW3NavigationHandler.Instance.NodeHandler.ClosestNode(targetLocation);
        Vector3 realtargetPosition = g.Location;
        agent.SetTarget(realtargetPosition);
        //Stay in this state
        return this;
    }

    //Upon entering state, set timer to enter Scatter State
    public override void EnterState(FSMAgent agent)
    {
        agent.SetTimer(20f);
    }

    public override void ExitState(FSMAgent agent) { }
}
