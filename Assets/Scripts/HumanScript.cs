using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanScript : MonoBehaviour
{
    [SerializeField] ManagerScript manager;
    [SerializeField] int location;
    [SerializeField] int team;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.addPoint(team, location);
        }
    }
}
