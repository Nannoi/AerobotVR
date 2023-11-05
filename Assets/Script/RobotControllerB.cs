using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

namespace YourNamespace
{
    public class RobotControllerB : MonoBehaviour
    {
        public float distanceTolerance = 0.1f;
        public float angleTolerance = 0.1f;
        public float linearSpeed = 5.0f;
        public float angularSpeed = 10.0f;
        private int numWayPoints;
        private string robotId;
        private string waypointId;
        private Vector3 centerLocation;
        private List<GameObject> neighbours;

        private List<Transform> destinations;
        public Transform destination;
        private Formation formation;

        public void Initialize(string robotId, Vector3 centerLocation, string waypointId, int numWayPoints, List<GameObject> neighbours)
        {
            this.robotId = robotId;
            this.centerLocation = centerLocation;
            this.waypointId = waypointId;
            this.numWayPoints = numWayPoints;
            this.neighbours = neighbours;

            destinations = new List<Transform>();

            string currentWaypointId = waypointId;
            GameObject waypointObject = GameObject.Find(currentWaypointId);

            if (waypointObject != null)
            {
                destination = waypointObject.transform;
                destinations.Add(destination);
            }
            else
            {
                Debug.LogError(currentWaypointId + " not found!");
            }
        }
        private void Start()
        {

            MoveToDestination();
        }

        private void MoveToDestination()
        {
            StartCoroutine(MoveCoroutine());
        }

        public System.Collections.IEnumerator MoveCoroutine()
        {
            while (true)
            {
                // Calculate the distance and direction to the destination
                Vector3 direction = destination.position - transform.position;
                float distance = direction.magnitude;

                // If the distance is greater than the tolerance, move towards the destination
                if (distance > distanceTolerance)
                {
                    // Move towards the destination
                    float movementStep = linearSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, destination.position, movementStep);
                }
          

                else
                {
                    // Rotate and look at Vector3.zero
                    Vector3 directionToLook = new Vector3(transform.position.x,0,-1) - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                    float angle = Quaternion.Angle(transform.rotation, targetRotation);
                    float rotationStep = angularSpeed * Time.deltaTime;

                    if (angle > angleTolerance)
                    {
                    // Rotate towards the desired front direction
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
                    }
                   
                    

                }

                yield return null;
            }


        }

    }
}
