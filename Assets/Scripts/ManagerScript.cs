using UnityEngine;
using Unity.MLAgents;
using Unity;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.Barracuda;
using System.Collections;
using Unity.Mathematics;
using System;
using Random = UnityEngine.Random;

public class ManagerScript : MonoBehaviour
{

    [System.Serializable]
    public class Point
    {
        public Transform position;
        public bool isUsed;
    }

    [System.Serializable]
    public class allWins
    {
        public int position1;
        public int position2;
        public int position3;

        public bool has1;
        public bool has2;
        public bool has3;
    }
    public List<Point> pointsList = new();

    public List<allWins> allWinsList = new();

    public int currentTurn;

    [SerializeField] GameObject yellowTeam;
    [SerializeField] GameObject blueTeam;

    public GameObject[] stuff;

    [SerializeField] int[] pointsYellow;
    [SerializeField] int[] pointsBlue;

    [SerializeField] AIScript yellowAgent;
    [SerializeField] AIScript blueAgent;

    int yellowCurr = 0;
    int blueCurr = 0;

    public bool isDone = true;
    public bool reseting = false;

    private object lockObject = new object();

    private void Awake()
    {
        pointsYellow = new int[9];
        pointsBlue = new int[9];
        stuff = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            pointsYellow[i] = -1;
            pointsBlue[i] = -1;
        }
    }

    public void addPoint(int teamID, int point)
    {
        lock (lockObject)
        {
            StartCoroutine(AddPointCoroutine(teamID, point));
        }
    }

    public IEnumerator AddPointCoroutine(int teamID, int point)
    {
        if (reseting)
            yield break;

        if (pointsList[point].isUsed)
        {
            //Debug.Log("Returned1");
            isDone = true;
            yield break;
        }

        //Debug.Log("Passed");
        pointsList[point].isUsed = true;
        if (teamID == 0)
        {
            stuff[yellowCurr + blueCurr] = Instantiate(blueTeam, pointsList[point].position);
            currentTurn = 1;
            pointsBlue[blueCurr] = point;
            blueCurr++;
        }
        else
        {
            stuff[yellowCurr + blueCurr] = Instantiate(yellowTeam, pointsList[point].position);
            currentTurn = 0;
            pointsYellow[yellowCurr] = point;
            yellowCurr++;
        }
        //Debug.Log("Point: " + point + " By: " + (teamID == 0 ? "Blue" : "Yellow"));
        checkWin(teamID);
        if (yellowCurr + blueCurr == 9)
        {
            Restart();
            yield break;
        }
        isDone = true;
        yield break;
    }

    public void checkWin(int teamID)
    {
        bool won = false;

        if (teamID == 0)
        {
            for (int i = 0; i <= pointsBlue.Length - 1; i++)
            {
                if (pointsBlue[i] == -1)
                    break;
                for (int j = 0; j <= allWinsList.Count() - 1; j++)
                {
                    if (pointsBlue[i] == allWinsList[j].position1)
                        allWinsList[j].has1 = true;
                    else if (pointsBlue[i] == allWinsList[j].position2)
                        allWinsList[j].has2 = true;
                    else if (pointsBlue[i] == allWinsList[j].position3)
                        allWinsList[j].has3 = true;
                }
            }
        }
        else
        {
            for (int i = 0; i <= pointsYellow.Length - 1; i++)
            {
                if (pointsYellow[i] == -1)
                    break;
                for (int j = 0; j <= allWinsList.Count() - 1; j++)
                {
                    if (pointsYellow[i] == allWinsList[j].position1)
                        allWinsList[j].has1 = true;
                    else if (pointsYellow[i] == allWinsList[j].position2)
                        allWinsList[j].has2 = true;
                    else if (pointsYellow[i] == allWinsList[j].position3)
                        allWinsList[j].has3 = true;
                }
            }
        }

        for (int j = 0; j <= allWinsList.Count() - 1; j++)
        {
            if (allWinsList[j].has1 && allWinsList[j].has2 && allWinsList[j].has3)
                won = true;
            allWinsList[j].has1 = false;
            allWinsList[j].has2 = false;
            allWinsList[j].has3 = false;

        }

        if (won)
        {
            Debug.Log("Team " + (teamID == 0 ? "Blue" : "Yellow") + " won");
            if (teamID == 0)
            {
                blueAgent.addReward();
                yellowAgent.addPunishment();
            }
            else
            {
                yellowAgent.addReward();
                blueAgent.addPunishment();
            }
            Restart();
        }
    }


    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }
    public IEnumerator RestartCoroutine()
    {
        reseting = true;
        isDone = false;
        currentTurn = 2;
        foreach (GameObject g in stuff)
        {
            Destroy(g);
        }
        yellowCurr = 0;
        blueCurr = 0;
        pointsYellow = new int[9];
        pointsBlue = new int[9];
        stuff = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            pointsYellow[i] = -1;
            pointsBlue[i] = -1;
        }
        foreach (Point p in pointsList)
        {
            p.isUsed = false;
        }
        isDone = true;
        currentTurn = Random.Range(0, 2);
        reseting = false;
        yellowAgent.EndEpisode();
        blueAgent.EndEpisode();
        yield break;
    }

}
