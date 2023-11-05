using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace YourNamespace
{
    public class Main : MonoBehaviour
    {
        private Text uiText;
        private Text previousText;

        public GameObject robotPrefab;
        public GameObject wayPointPrefab;
        public GameObject meshPrefab;
        public Dropdown dropdown;
        public int numRobots;
        public GameObject gameObjectsContainer; // Reference to the empty GameObject
       




        void Start()
        {
            numRobots = 0; // Set an initial value
            
            // Add a listener to the Dropdown component to trigger when its value changes
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        void OnDropdownValueChanged(int value)
        {
            foreach (Transform child in gameObjectsContainer.transform)
            {
                Destroy(child.gameObject);
            }
            Dropdown[] dropdowns = FindObjectsOfType<Dropdown>();

            // Loop through each found Dropdown and reset its value to the default (index 0)
            foreach (Dropdown drop in dropdowns)
            {
                if (drop != dropdown)
                {
                    drop.value = 0;
                }
            }
            // Instantiate new objects with a delay to ensure the old ones are destroyed
            StartCoroutine(InstantiateObjectsWithDelay());
        }


        IEnumerator InstantiateObjectsWithDelay()
        {
            // Wait for 2 seconds
            yield return new WaitForSeconds(1.0f);
            if (dropdown.value > 0)
            {
                // Now instantiate new objects
                numRobots = dropdown.value + 2;
                InstantiateObjects();
                Debug.Log("Generate " + numRobots + " Robots");
            }
        }

        void InstantiateObjects()
        {

            for (var i = 1; i <= numRobots; i++)
            {

                GameObject robot = Instantiate(robotPrefab,new Vector3((i * 4.0f) - ((numRobots + 1) * 2.0f), 0, 8), Quaternion.identity);

                // Set the parent of the robot to the gameObjectsContainer
                robot.transform.SetParent(gameObjectsContainer.transform);
                // Optionally, you can rename the robot for easier identification
                robot.name = "Robot" + i.ToString();

                GameObject wayPoint = Instantiate(wayPointPrefab, Vector3.zero, Quaternion.identity);
                wayPoint.transform.SetParent(gameObjectsContainer.transform);
                wayPoint.name = "WayPoint" + i.ToString();

                GameObject fabric = Instantiate(meshPrefab, Vector3.zero, Quaternion.identity);
                // Set the parent of the robot to the gameObjectsContainer
                fabric.transform.SetParent(gameObjectsContainer.transform);
                // Optionally, you can rename the robot for easier identification
                fabric.name = "Fabric" + i.ToString();  
            }
            updateRobot();
        }

        private void updateRobot()
        {
            for (var i = 1; i <= numRobots; i++)
            {
                string robotId = "Robot" + i.ToString();
                string NumId = i.ToString();
                string waypointId = "WayPoint" + i.ToString();
                string fabricId = "Fabric" + i.ToString();

                GameObject robot = GameObject.Find("Robot" + i.ToString());
                List<GameObject> neighbours = GetNeighbours(i);

                RobotBehaviour robotBehaviour = robot.AddComponent<RobotBehaviour>();
                robotBehaviour.Initialize(NumId, Vector3.zero, waypointId, numRobots, neighbours);
                robotBehaviour.Evaluate();

                GameObject fabric = GameObject.Find("Fabric" + i.ToString());
                GameObject Fneighbour = fabricNeighbour(i);

                MainFabric mainmesh = fabric.AddComponent<MainFabric>();
                DrawMesh drawmesh = fabric.AddComponent<DrawMesh>();
                mainmesh.Initialize(robotId, fabricId, numRobots, Fneighbour);
            }
        }
        private List<GameObject> GetNeighbours(int robotIndex)
        {
            List<GameObject> neighbours = new List<GameObject>();

            for (int i = 1; i <= numRobots; i++)
            {
                if (i != robotIndex)
                {
                    GameObject neighbour = GameObject.Find("Robot" + i.ToString());
                    if (neighbour != null)
                    {
                        neighbours.Add(neighbour);
                    }
                }

            }
            return neighbours;
        }

        private GameObject fabricNeighbour(int robotIndex)
        {
            GameObject Fneighbour = null;

            for ( robotIndex = 1; robotIndex <= numRobots; robotIndex++)
            {

                    if (robotIndex == numRobots)
                    {
                        Fneighbour = GameObject.Find("Robot1");
                        
                    }
                    else
                    {
                        int m = robotIndex + 1;
                        Fneighbour = GameObject.Find("Robot" + m.ToString());

                    }
                
            }
            return Fneighbour;
        }

    }
 }

