using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

namespace YourNamespace
{
    public class RobotControllerA : MonoBehaviour
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
                    // Calculate the angle to the destination
                  //  Quaternion targetRotationFront = Quaternion.LookRotation(direction);
                  //  Quaternion targetRotationBack = Quaternion.LookRotation(-direction);
                  //  float angleFront = Quaternion.Angle(transform.rotation, targetRotationFront);
                  //  float angleBack = Quaternion.Angle(transform.rotation, targetRotationBack);

                    // Determine the closer angle
                   // float minAngle = Mathf.Min(angleFront, angleBack);
                   // Quaternion targetRotation;
                    //if (minAngle > angleTolerance)
                   // {
                       // if (minAngle == angleFront)
                       // {
                       //     targetRotation = targetRotationFront;
                       // }
                       // else 
                       // {
                       //     targetRotation = targetRotationBack;
                       // }
                    
                    // Rotate towards the destination
                    //float rotationStep = angularSpeed * Time.deltaTime;
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
                    //}
                    //else
                    //{
                        // Move towards the destination
                        float movementStep = linearSpeed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, destination.position, movementStep);
                    }
                //}
              
             else
             {
                    // Rotate and look at Vector3.zero
                    Vector3 center = new Vector3(centerLocation.x,0,centerLocation.z);
                    Vector3 centerDirection = center - transform.position;
                    Quaternion zeroRotation = Quaternion.LookRotation(centerDirection);
                    float zeroAngle = Quaternion.Angle(transform.rotation, zeroRotation);
                    float zeroRotationStep = angularSpeed * Time.deltaTime;

                    if (zeroAngle > angleTolerance)
                    {
                        // Rotate towards Vector3.zero
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, zeroRotation, zeroRotationStep);
                    }
                    else
                    {
                        // Object is now looking at Vector3.zero
                        Debug.Log("Destination reached!");
                        yield break;
                    }
             }

                    yield return null;
             }

           
        }

    }
}
