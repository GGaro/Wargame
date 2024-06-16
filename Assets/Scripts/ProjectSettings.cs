using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
[DisallowMultipleComponent]
public class ProjectSettings : MonoBehaviour
{
    [Tooltip("Changes the rate at which time passes")]
    public float TimeScale;

    private void FixedUpdate()
    {
        Time.timeScale = TimeScale;
    }
}
