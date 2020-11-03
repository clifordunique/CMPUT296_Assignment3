using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHuntState : State
{

    //Set name of this state
    public CustomHuntState() : base("CustomHunt") { }

    public override State Update(FSMAgent agent)
    {
        //Handle Following Pacman
        Vector3 pacmanLocation = PacmanInfo.Instance.transform.position;
        Vector3 pacmanFacing = PacmanInfo.Instance.Facing;
        Vector3 location, realLocation;
        FSMAgent ghost = GhostManager.Instance.GetClosestGhost(pacmanLocation);

        if (agent.CloseEnough(pacmanLocation))
        {
            ScoreHandler.Instance.KillPacman();
        }

        //If timer complete, go to Scatter State
        if (agent.TimerComplete())
        {
            return new CustomChaseState();
        }

        //If Pacman ate a power pellet, go to Frightened State
        if (PelletHandler.Instance.JustEatenPowerPellet)
        {
            return new FrightenedState(this);
        }

        if (!ObstacleHandler.Instance.AnyIntersect(pacmanLocation, agent.GetPosition()))
        {
            return new CustomExcitedChaseState();
        }

        //If we didn't return follow Pacman
        if (ghost is CustomGhost)
        {
            agent.SetTarget(pacmanLocation);
        }
        else
        {
            if (pacmanFacing.x != 0)
            {
                location = ghost.GetPosition() + Vector3.up * 2*(pacmanLocation.y - ghost.GetPosition().y);
            }
            else
            {
                location = ghost.GetPosition() + Vector3.right * 2*(pacmanLocation.x - ghost.GetPosition().x);
            }
            GraphNode g = HW3NavigationHandler.Instance.NodeHandler.ClosestNode(location);
            realLocation = g.Location;
            agent.SetTarget(realLocation);
        }
        //Stay in this state
        return this;
    }

    //Upon entering state, set timer to enter Scatter State
    public override void EnterState(FSMAgent agent)
    {
        agent.SetTimer(10f);
        agent.SetSpeedModifierNormal();
    }

    public override void ExitState(FSMAgent agent) { }
}
