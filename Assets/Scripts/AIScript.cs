using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIScript : Agent
{
    [SerializeField] int teamID;
    [SerializeField] ManagerScript managerScript;
    public override void OnActionReceived(ActionBuffers actions)
    {
        if(managerScript.reseting)
            return;
        if (managerScript.currentTurn == teamID && managerScript.isDone)
        {
            managerScript.isDone = false;
            managerScript.addPoint(teamID, actions.DiscreteActions[0]);
           // Debug.Log("Team: " + (teamID == 0 ? "Blue" : "Yello") + " Picked the pos: " + actions.DiscreteActions[0]);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(managerScript.currentTurn);
        for (int j = 0; j <= managerScript.allWinsList.Count - 1; j++)
        {
            sensor.AddObservation(managerScript.allWinsList[j].position1);
            sensor.AddObservation(managerScript.allWinsList[j].position2);
            sensor.AddObservation(managerScript.allWinsList[j].position3);
        }
        for (int j = 0; j <= managerScript.stuff.Length - 1; j++)
        {
            sensor.AddObservation(managerScript.stuff[j]);
        }
    }

    public void addReward()
    {
        AddReward(1f);
    }
    public void addPunishment()
    {
        AddReward(-1f);
    }
}
