using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YourNamespace;

public class RobotBehaviour : MonoBehaviour
{
    private Formation formation;
    private int numRobots;
    private string robotId;
    private string waypointId;
    private Vector3 centerLocation;
    private float centerY;
    private List<GameObject> neighbours;
    public void Initialize(string robotId, Vector3 placedObjectLocation, string waypointId, int numRobots, List<GameObject> neighbours)
    {
        this.robotId = robotId;
        this.centerLocation = placedObjectLocation;
        this.waypointId = waypointId;
        this.numRobots = numRobots;
        this.neighbours = neighbours;

        formation = gameObject.AddComponent<Formation>();
        formation.Initialize(robotId, centerLocation, waypointId, numRobots, neighbours);
    }

    public void Evaluate()
    {
        StartCoroutine(EvaluateFormation());
    }

    private IEnumerator EvaluateFormation()
    {
        formation.Execute();

        // Wait for the formation process to complete
        yield return StartCoroutine(WaitForFormationCompletion());

        // Restart the evaluation
       // Evaluate();
    }

    private IEnumerator WaitForFormationCompletion()
    {
        while (!formation.IsCompleted)
        {
            yield return null;
        }
    }

    public bool IsCompleted => formation != null && formation.IsCompleted;
}
