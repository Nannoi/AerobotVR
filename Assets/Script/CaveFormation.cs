using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNamespace
{
    public class CaveFormation : MonoBehaviour
    {
        public string robotId;
        private int FrobotId;
        Vector3 centerLocation;
        public Vector3 startPosition;
        public Vector3 endPosition;
        public int numIterations;
        public List<GameObject> neighbours;

        //private float distanceTolerance = 0.1f;
        private string json;
        //private float syncInterval = 2f;
        private float totalRobots;
        private int robotIndex;
        private int FrobotIndex;
        private float angle = 180;
        private int numberOfRays = 36;
        private float positionRatio;
        private Vector3 currentPosition;
        private bool isCompleted;



        public void Initialize(string robotId, Vector3 centerLocation, Vector3 startPosition, Vector3 endPosition, int numIterations, List<GameObject> neighbours)
        {
            this.robotId = robotId;
            this.centerLocation = centerLocation;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.numIterations = numIterations;
            this.neighbours = neighbours;
        }

        public void Start()
        {

            robotIndex = int.Parse(robotId);
            positionRatio = (endPosition.z - startPosition.z) / (numIterations - 1f);

            if (robotIndex <= numIterations / 2)
            {
                // Initialize position  
                currentPosition = new Vector3(centerLocation.x - 1.3f, 0, (centerLocation.z - (positionRatio * ((numIterations / 4) - 1f)) +
                    positionRatio * (robotIndex - 1f))+2);
            }
            else if (robotIndex > numIterations / 2)
            {
                currentPosition = new Vector3(centerLocation.x + 1.3f, 0, (centerLocation.z - (positionRatio *
               ((numIterations / 4) - 1f))) + positionRatio * ((robotIndex - (numIterations / 2)) - 1f)+2);
            }

            StartCoroutine(Execute());
            StartCoroutine(RunCaveFormation());
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        void FixedUpdate()
        {
            AvoidObstacle();
        }
        public IEnumerator Execute()
        {
            // Perform the line formation logic here
            Debug.Log($"Cave Formation executing for robot {robotId}");

            // Simulate the formation process by waiting for a few seconds
            yield return new WaitForSeconds(2.0f);

            // Set the completion flag
            isCompleted = true;

            // Send the line formation completion message
            Debug.Log("CAVE_FORMATION_COMPLETED");
        }

        private IEnumerator RunCaveFormation()
        {
            // Move to the initial position
            yield return StartCoroutine(MoveToPosition(currentPosition));

            for (int i = 1; i <= numIterations; i++)
            {
                // Share position with neighbors
                foreach (GameObject neighbour in neighbours)
                {
                    SendMessageToNeighbour("POSITION", robotId, currentPosition.x, currentPosition.y, neighbour);
                }

                // Retrieve positions of all neighbors
                Dictionary<string, Vector2> positions = new Dictionary<string, Vector2>();
                foreach (GameObject neighbour in neighbours)
                {
                    PositionData positionData = neighbour.GetComponent<CaveFormation>().ReceivePositionFromNeighbour(neighbour);
                    positions.Add(positionData.senderId, new Vector2((float)positionData.x, (float)positionData.y));
                }

                // Calculate the average position of the robot and its neighbors
                Vector2 avgPosition = currentPosition;

                foreach (Vector2 position in positions.Values)
                {
                    avgPosition += position;
                }
                // Point the object at the world origin (0,0,0)
                //transform.LookAt(Vector3.zero);


                //avgPosition /= (neighbours.Count + 1);

                // Update the position based on the average position and the desired line formation
                // Vector2 newPosition = startPosition + new Vector2(positionRatio * robotIndex,0f) + 0.5f * (avgPosition - currentPosition);

                // Move to the updated position
                // yield return StartCoroutine(MoveToPosition(newPosition));

                // Wait before the next iteration
                // yield return new WaitForSeconds(syncInterval);
            }

            // Formation completed
            // Debug.Log($"Line Formation completed for robot {robotId}");
        }


        private IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            // float movementSpeed = 6f; // Adjust this value to control the movement speed

            GetComponent<RobotControllerC>().destination.position = targetPosition;
            yield return StartCoroutine(GetComponent<RobotControllerC>().MoveCoroutine());

        }


        private void AvoidObstacle()
        {
            float rayRange = 4f; // Set the desired range to 4 meters
            float targetVelocity = 10f;
            float avoidanceDistance = 1.0f; // Set the distance at which avoidance behavior is triggered
            float ObavoidanceDistance = 1.0f;
            float rayOffset = 0.15f; // Offset to move the rays out of the object's body
            Vector3 backward = new Vector3(0, 0, -1);
            var deltaPosition = Vector3.zero;
            for (int i = 0; i < numberOfRays; ++i)
            {
                var rotation = this.transform.rotation;
                var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle * 2 - angle, this.transform.up);
                var direction = rotation * rotationMod * Vector3.forward * rayRange;

                // Apply the ray offset to the starting position
                var rayStartPosition = this.transform.position + rayOffset * direction;

                var ray = new Ray(rayStartPosition, direction);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, rayRange, LayerMask.GetMask("Robot")))
                {
                    if (hitInfo.distance <= avoidanceDistance)
                    {
                        //var otherRobot = hitInfo.collider.gameObject;

                        // Check if the other robot has reached its destination
                        //var otherFormation = otherRobot.GetComponent<CircleFormation>();
                        if (IsCompleted == false)
                        {
                            // Calculate the avoidance direction based on the difference between the robot's position and the obstacle's position
                            Vector3 avoidanceDirection = backward + ((transform.position - hitInfo.point).normalized - (transform.forward));

                            //Vector3 desiredDirection = avoidanceDirection.normalized + transform.forward;
                            deltaPosition += (10.0f / numberOfRays) * targetVelocity * avoidanceDirection;

                            // Apply the avoidance direction to the delta position
                            Debug.DrawRay(ray.origin, direction, Color.red);

                        }
                    }
                }
                else if (Physics.Raycast(ray, out hitInfo, rayRange, LayerMask.GetMask("Obstacles")))
                {
                    if (hitInfo.distance <= ObavoidanceDistance)
                    {
                        // Calculate the avoidance direction based on the difference between the robot's position and the obstacle's position
                        Vector3 avoidanceDirection = (transform.position - hitInfo.point).normalized;

                        // Apply the avoidance direction to the delta position
                        deltaPosition += (1.0f / numberOfRays) * targetVelocity * avoidanceDirection;

                        if (avoidanceDirection != Vector3.zero)
                        {
                            // Calculate the desired direction by combining the avoidance direction and the robot's current direction
                            Vector3 desiredDirection = backward + (avoidanceDirection.normalized + transform.forward);

                            // Apply the desired direction to the delta position
                            deltaPosition = desiredDirection.normalized * targetVelocity;

                        }
                        Debug.DrawRay(ray.origin, direction, Color.cyan);


                    }
                }
                else
                {
                    Debug.DrawRay(ray.origin, direction, Color.white);
                }
            }

            this.transform.position += deltaPosition * Time.deltaTime;
        }

        private void SendMessageToNeighbour(string message, string senderId, double x, double y, GameObject neighbour)
        {
            // Construct the message data
            PositionData positionData = new PositionData(senderId, x, y);

            // Convert the position data to a JSON string
            json = JsonUtility.ToJson(positionData);

            // Send the message to the neighbour using your Unity-specific communication method
            //Debug.Log($"Message Sent: {positionData.senderId}: {positionData.x}, {positionData.y}");
            // Replace the code below with your actual communication method
            //neighbour.GetComponent<LineFormation>().ReceivePositionFromNeighbour(message, senderId, x, y);
        }

        private PositionData ReceivePositionFromNeighbour(GameObject neighbour)
        {
            // Convert the JSON string to position data
            //PositionData positionData = JsonUtility.FromJson<PositionData>(json);

            // Process the received position data here

            PositionData neighbourData = new PositionData(robotId, neighbour.transform.position.x, neighbour.transform.position.z);
            //Debug.Log($"Received Position from {neighbourData.senderId}: {neighbourData.x}, {neighbourData.y}");

            // Return the position data
            return neighbourData;
        }
    }


}
