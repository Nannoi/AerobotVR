using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using System.Linq;
using System;
using System.Text;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

namespace YourNamespace
{
    public class Formation : MonoBehaviour
    {
        private Text uiText;
        private Text previousText;

        private Text shapeData;
        private Text areaData;
        private Text lengthData;
        public string robotId;
        Vector3 centerLocation;
        public int numIterations;
        public List<GameObject> neighbours;
        private string waypointId;
        private bool isCompleted;
        private float radius = 4.0f;
        private float radiusS;
        private float sideLength = 8.0f;
        private float sideTLength = 10.0f;
        private float scaleFactor = 1f;
        private Vector2 lineStart = new Vector2(-10.0f, 0f);
        private Vector2 lineEnd = new Vector2(10.0f, 0f);
        public double area;
        private Action currentShape;

        private LineFormation lineFormationProcess;
        private CircleFormation circleFormationProcess;
        private TriangleFormation triangleFormationProcess;
        private SquareFormation squareFormationProcess;
        private FlowerFormation flowerFormationProcess;
        private SeaFormation seaFormationProcess;
        private CaveFormation caveFormationProcess;
        private RobotControllerA robotControllerAProcess;
        private RobotControllerB robotControllerBProcess;
        private RobotControllerC robotControllerCProcess;
        private Dropdown dropdown;
        private Dropdown dropup;
        private GameObject popup;




        public void Initialize(string robotId, Vector3 centerLocation, string waypointId, int numIterations, List<GameObject> neighbours)
        {
            this.robotId = robotId;
            this.centerLocation = centerLocation;
            this.waypointId = waypointId;
            this.numIterations = numIterations;
            this.neighbours = neighbours;

        }

        private void Start()
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            GameObject formationTransform = FindObjectByName(canvasObject.transform, "Formation");
            GameObject areaTransform = FindObjectByName(canvasObject.transform, "Area");
            dropdown = formationTransform.GetComponent<Dropdown>();
            dropup = areaTransform.GetComponent<Dropdown>();
 
            // Add a listener to the onValueChanged event of the InputField component
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            dropup.onValueChanged.AddListener(OnDropupValueChanged);


            GameObject areaG = FindObjectByName(canvasObject.transform, "AreaText");
            areaData = areaG.GetComponent<Text>();

            GameObject lengthG = FindObjectByName(canvasObject.transform, "RadiusText");
            lengthData = lengthG.GetComponent<Text>();


        }

        // The method that will be executed when the InputField's value changes
        void OnDropdownValueChanged(int result)
        {
            FormationSpeech(result);
            if (currentShape != null)
            {
                areaData.text = area.ToString()+" sqm";
                lengthData.text = radiusS.ToString()+" m";
            }

        }
        void OnDropupValueChanged(int result)
        {
            SizeSpeech(result);
            if (currentShape != null)
            {
                areaData.text = area.ToString() + " sqm";
                lengthData.text = radiusS.ToString() + " m";
            }
            
        }
        public GameObject FindObjectByName(Transform parent, string objectName)
        {
            if (parent.name == objectName)
            {
                return parent.gameObject;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                GameObject foundObject = FindObjectByName(child, objectName);

                if (foundObject != null)
                {
                    return foundObject;
                }
            }

            return null;
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public IEnumerator Execute()
        {
            // Perform the formation logic here
            Debug.Log($"Formation executing for robot {robotId}");

            // Simulate the formation process by waiting for a few seconds
            yield return new WaitForSeconds(2.0f);

            // Set the completion flag
            isCompleted = true;

            // Send the formation completion message
            SendDone("FORMATION_COMPLETED");
        }

        void FormationSpeech(int result)
        {

            if (result == 1)
            {
                Line();
            }
            else if (result == 2)
            {
                Circle();
            }
            else if (result == 3)
            {
                Triangle();
            }
            else if (result == 4)
            {
                Square();
            }

            else if (result == 5)
            {
                Flower();
            }
            else if (result == 6)
            {
                Sea();
            }
            else if (result == 7)
            {
                Cave();
            }
            Debug.Log("Start: " + currentShape.Method.Name);
            CalculateArea();
            Debug.Log("Area: " + area);
        }
        void SizeSpeech(int result)
        {
           
                if (result == 1)
                {
                    ScaleUp();

                }
                else if (result == 2)
                {
                    ScaleDown();
                }

                else if (result == 3)
                {
                    TwentySqm();
                }

                else if (result == 4)
                {
                    TwentyfiveSqm();
                }


                else if (result == 5)
                {
                    ThirtySqm();
                }

                else if (result == 6)
                {
                    ThirtyfiveSqm();
                }


                else if (result == 7)
                {
                    FortySqm();
                }

                else if (result == 8)
                {
                    FortyfiveSqm();
                }

                else if (result == 9)
                {
                    FiftySqm();
                }
                else if (result == 10)
                {
                    FiftyfiveSqm();
                }

                else if (result == 11)
                {
                    SixtySqm();
                }

                else if (result == 12)
                {
                    SixtyfiveSqm();
                }

                else if (result == 13)
                {
                    SeventySqm();
                }
                else if (result == 14)
                {
                    SeventyfiveSqm();
                }

                else if (result == 15)
                {
                    EightySqm();
                }

                else if (result == 16)
                {
                    EightyfiveSqm();
                }

                else if (result == 17)
                {
                    NintySqm();
                }
                else if (result == 18)
                {
                    NintyfiveSqm();
                }
                else if (result == 19)
                {
                    HundredSqm();
                }
                CalculateArea();
                Debug.Log("Area: " + area);
            

        }

        private void Line()
        {
            currentShape = Line;
            Vector2 lineStartF = new Vector3(-numIterations * 1.5f,0,0) * scaleFactor;
            Vector2 lineEndF = new Vector3(numIterations * 1.5f, 0, 0) * scaleFactor;
            int FrobotId = 1;
            radiusS = lineStartF.x+lineEndF.x;
                
                string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (robot != null)
                {

                lineFormationProcess = robot.GetComponent<LineFormation>();
                circleFormationProcess = robot.GetComponent<CircleFormation>();
                squareFormationProcess = robot.GetComponent<SquareFormation>();
                triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                seaFormationProcess = robot.GetComponent<SeaFormation>();
                caveFormationProcess = robot.GetComponent<CaveFormation>();
                robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                    triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                    || caveFormationProcess != null)
                {
                    Destroy(lineFormationProcess);
                    Destroy(circleFormationProcess);
                    Destroy(squareFormationProcess);
                    Destroy(triangleFormationProcess);
                    Destroy(flowerFormationProcess);
                    Destroy(seaFormationProcess);
                    Destroy(caveFormationProcess);
                    Destroy(robotControllerAProcess);
                    //Destroy(robotControllerBProcess);
                    Destroy(robotControllerCProcess);
                }

                // Add the CircleFormation component to the robot GameObject
                lineFormationProcess = robot.AddComponent<LineFormation>();
                lineFormationProcess.Initialize(robotId,FrobotId,centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                if (robotControllerBProcess != null)
                {
                    RobotControllerB robotControllerB = robot.GetComponent<RobotControllerB>();
                    robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }
                else
                {
                    RobotControllerB robotControllerB = robot.AddComponent<RobotControllerB>();
                    robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }

            }
                else
                {
                    Debug.Log("Robot object is null!");
                }

        }


        private void Circle()
        {
            currentShape = Circle;
            float radiusF = radius * scaleFactor;
            radiusS= radiusF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations >= 5)
            {
                if (robot != null)
                {

                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    circleFormationProcess = robot.AddComponent<CircleFormation>();
                    circleFormationProcess.Initialize(robotId, centerLocation, radiusF, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }

                }
                else
                {
                    Debug.Log("Robot object is null!");
                }
            }
            else
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null)
                {
                    popup.SetActive(true);
                }
                Debug.Log("Number of Robots is not enough!");
            }

        }

        private void Square()
        {
            currentShape = Square;
            //Square formation
            float halfLength = (sideLength / 2) * scaleFactor;
            radiusS = sideLength*scaleFactor;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (numIterations % 4 != 1 && numIterations % 4 != 2 && numIterations % 4 != 3)
            {
                if (robot != null)
                {

                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    // Add the CircleFormation component to the robot GameObject
                    squareFormationProcess = robot.AddComponent<SquareFormation>();
                    squareFormationProcess.Initialize(robotId, centerLocation, halfLength, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }

                }
                else
                {
                    Debug.Log("Robot object is null!");
                }
            }
            else
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null)
                {
                    popup.SetActive(true);
                }
                Debug.Log("Number of Robots is not enough!");
            }

        }

        private void Triangle()
        {
            currentShape = Triangle;
            float sideTLengthF = sideTLength * scaleFactor;
            //Triangle formation
            radiusS= sideTLengthF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations % 3 != 1 && numIterations % 3 != 2)
            {
                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null || 
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    // Add the TriangleFormation component to the robot GameObject
                    triangleFormationProcess = robot.AddComponent<TriangleFormation>();
                    triangleFormationProcess.Initialize(robotId, 1, centerLocation, sideTLengthF, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }
                else
                {
                    Debug.Log("Robot object is null!");
                }
            }
            else 
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null) 
                {
                    popup.SetActive(true);
                }
                Debug.Log("Number of Robots is not enough!");
            }

        }

        private void DrawShape()
        {

            if (currentShape == Circle)
            {
                Circle();
            }
            else if (currentShape == Triangle)
            {
                Triangle();
            }
            else if (currentShape == Square)
            {
                Square();
            }
            else if (currentShape == Line)
            {
                Line();
            }
            else if (currentShape == Sea)
            {
                Sea();
            }
            else if (currentShape == Flower)
            {
                Flower();
            }
            else if (currentShape == Cave)
            {
                Cave();
            }
        }

        private void CalculateArea()
        {

            if (currentShape == Circle)
            {
                CalculateCircleArea();
            }
            else if (currentShape == Triangle)
            {
                CalculateTriangleArea();
            }
            else if (currentShape == Square)
            {
                CalculateSquareArea();
            }
            else if (currentShape == Line)
            {
                CalculateLineLength();
            }
            else if (currentShape == Sea)
            {
                CalculateSeaLength();
            }
            else if (currentShape == Flower)
            {
                CalculateFlowerArea();
            }
            else if (currentShape == Cave)
            {
                CalculateCaveArea();
            }

        }

        private double CalculateCircleArea()
        {
            area= Math.Round(Mathf.PI * (radius * radius * scaleFactor * scaleFactor),2);
            return area;
        }

        private double CalculateTriangleArea()
        {
            area = Math.Round(0.5f * sideTLength * sideTLength * scaleFactor * scaleFactor,2);
            return area;
        }

        private double CalculateSquareArea()
        {
            area = Math.Round(sideLength * sideLength * scaleFactor * scaleFactor,2);
            return area;
        }

        private double CalculateLineLength()
        {
            area = Math.Round(((-lineStart.x * scaleFactor) + (lineEnd.x * scaleFactor)),2);
            return area;
        }
        private double CalculateSeaLength()
        {
            area = Math.Round(((-lineStart.x * scaleFactor) + (lineEnd.x * scaleFactor)) * (numIterations - 3) / numIterations, 2);
            return area;
        }

        private double CalculateFlowerArea()
        {
            area = Math.Round(Mathf.PI * (radius * radius * scaleFactor * scaleFactor), 2);
            return area;
        }

        private double CalculateCaveArea()
        {
            area = Math.Round((-lineStart.x * scaleFactor * 2.6), 2);
            return area;
        }

        private void ScaleUp()
        {
            scaleFactor *= 2f;
            DrawShape();
            Debug.Log("Scale Up" + scaleFactor);
        }

        private void ScaleDown()
        {
            scaleFactor /= 2f;
            DrawShape();
            Debug.Log("Scale Down" + scaleFactor);
        }

        private void TwentySqm()
        {
            radius = Mathf.Sqrt(20f / Mathf.PI);
            sideLength = Mathf.Sqrt(20f);
            sideTLength = Mathf.Sqrt(2 * 20f);
            lineStart = new Vector3(-20f / 2, 0f);
            lineEnd = new Vector3(20f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        
        private void TwentyfiveSqm()
        {
            radius = Mathf.Sqrt(25f / Mathf.PI);
            sideLength = Mathf.Sqrt(25f);
            sideTLength = Mathf.Sqrt(2 * 25f);
            lineStart = new Vector3(-25f / 2, 0f);
            lineEnd = new Vector3(25f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
       
        private void ThirtySqm()
        {
            radius = Mathf.Sqrt(30f / Mathf.PI);
            sideLength = Mathf.Sqrt(30f);
            sideTLength = Mathf.Sqrt(2 * 30f);
            lineStart = new Vector3(-30f / 2, 0f);
            lineEnd = new Vector3(30f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        
        
        private void ThirtyfiveSqm()
        {
            radius = Mathf.Sqrt(35f / Mathf.PI);
            sideLength = Mathf.Sqrt(35f);
            sideTLength = Mathf.Sqrt(2 * 35f);
            lineStart = new Vector3(-35f / 2, 0f);
            lineEnd = new Vector3(35f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        
        private void FortySqm()
        {
            radius = Mathf.Sqrt(40f / Mathf.PI);
            sideLength = Mathf.Sqrt(40f);
            sideTLength = Mathf.Sqrt(2 * 40f);
            lineStart = new Vector3(-40f / 2, 0f);
            lineEnd = new Vector3(40f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
            private void FortyfiveSqm()
        {
            radius = Mathf.Sqrt(45f / Mathf.PI);
            sideLength = Mathf.Sqrt(45f);
            sideTLength = Mathf.Sqrt(2 * 45f);
            lineStart = new Vector3(-45f / 2, 0f);
            lineEnd = new Vector3(45f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        
        private void FiftySqm()
        {
            radius = Mathf.Sqrt(50f / Mathf.PI);
            sideLength = Mathf.Sqrt(50f);
            sideTLength = Mathf.Sqrt(2 * 50f);
            lineStart = new Vector3(-50f / 2, 0f);
            lineEnd = new Vector3(50f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void FiftyfiveSqm()
        {
            radius = Mathf.Sqrt(55f / Mathf.PI);
            sideLength = Mathf.Sqrt(55f);
            sideTLength = Mathf.Sqrt(2 * 55f);
            lineStart = new Vector3(-55f / 2, 0f);
            lineEnd = new Vector3(55f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SixtySqm()
        {
            radius = Mathf.Sqrt(60f / Mathf.PI);
            sideLength = Mathf.Sqrt(60f);
            sideTLength = Mathf.Sqrt(2 * 60f);
            lineStart = new Vector3(-60f / 2, 0f);
            lineEnd = new Vector3(60f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SixtyfiveSqm()
        {
            radius = Mathf.Sqrt(65f / Mathf.PI);
            sideLength = Mathf.Sqrt(65f);
            sideTLength = Mathf.Sqrt(2 * 65f);
            lineStart = new Vector3(-65f / 2, 0f);
            lineEnd = new Vector3(65f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SeventySqm()
        {
            radius = Mathf.Sqrt(70f / Mathf.PI);
            sideLength = Mathf.Sqrt(70f);
            sideTLength = Mathf.Sqrt(2 * 70f);
            lineStart = new Vector3(-70f / 2, 0f);
            lineEnd = new Vector3(70f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SeventyfiveSqm()
        {
            radius = Mathf.Sqrt(75f / Mathf.PI);
            sideLength = Mathf.Sqrt(75f);
            sideTLength = Mathf.Sqrt(2 * 75f);
            lineStart = new Vector3(-75f / 2, 0f);
            lineEnd = new Vector3(75f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void EightySqm()
        {
            radius = Mathf.Sqrt(80f / Mathf.PI);
            sideLength = Mathf.Sqrt(80f);
            sideTLength = Mathf.Sqrt(2 * 80f);
            lineStart = new Vector3(-80f / 2, 0f);
            lineEnd = new Vector3(80f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void EightyfiveSqm()
        {
            radius = Mathf.Sqrt(85f / Mathf.PI);
            sideLength = Mathf.Sqrt(85f);
            sideTLength = Mathf.Sqrt(2 * 85f);
            lineStart = new Vector3(-85f / 2, 0f);
            lineEnd = new Vector3(85f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void NintySqm()
        {
            radius = Mathf.Sqrt(90f / Mathf.PI);
            sideLength = Mathf.Sqrt(90f);
            sideTLength = Mathf.Sqrt(2 * 90f);
            lineStart = new Vector3(-90f / 2, 0f);
            lineEnd = new Vector3(90f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void NintyfiveSqm()
        {
            radius = Mathf.Sqrt(95f / Mathf.PI);
            sideLength = Mathf.Sqrt(95f);
            sideTLength = Mathf.Sqrt(2 * 95f);
            lineStart = new Vector3(-95f / 2, 0f);
            lineEnd = new Vector3(95f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void HundredSqm()
        {
            radius = Mathf.Sqrt(100f / Mathf.PI);
            sideLength = Mathf.Sqrt(100f);
            sideTLength = Mathf.Sqrt(2 * 100f);
            lineStart = new Vector3(-100f / 2, 0f);
            lineEnd = new Vector3(100f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }


        private void Sea()
        {
            currentShape = Sea;
            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations >= 6)
            {
                Vector3 StartF = new Vector3(-numIterations*1.5f, 0f, 0f);
                Vector3 EndF = new Vector3(numIterations*1.5f, 0f, 0f);

                Vector3 newcenterLocation = new Vector3(centerLocation.x, centerLocation.z, centerLocation.y + 3f);

                int FrobotId = 1;

                Vector2 lineStartF = StartF * scaleFactor;
                Vector2 lineEndF = EndF * scaleFactor;


                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        Destroy(robotControllerAProcess);
                        //Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }
                    seaFormationProcess = robot.AddComponent<SeaFormation>();


                    //circleFormationProcess.enabled = true;
                    seaFormationProcess.Initialize(robotId, FrobotId, centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                    if (robotControllerBProcess != null)
                    {
                        RobotControllerB robotControllerB = robot.GetComponent<RobotControllerB>();
                        robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerB robotControllerB = robot.AddComponent<RobotControllerB>();
                        robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }

                }
                else
                {
                    Debug.Log("Robot object is null!");
                }
            }

            else
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null)
                {
                    popup.SetActive(true);
                }
                Debug.Log("Robot numbers are not enough!");
            }


        }

        private void Flower()
        {
            currentShape = Flower;
            float radiusF = radius * scaleFactor;
            radiusS = radiusF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations >= 6)
            {

                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    flowerFormationProcess = robot.AddComponent<FlowerFormation>();
                    //circleFormationProcess.enabled = true;
                    flowerFormationProcess.Initialize(robotId, centerLocation, radiusF, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }

                else
                {
                   
                    Debug.Log("Robot object is null!");
                }
            }
            else
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null)
                {
                    popup.SetActive(true);
                }
                Debug.Log("Robot numbers are not enough!");
            }
        }

        private void Cave()
        {
            currentShape = Cave;
            float radiusF = radius * scaleFactor;
            radiusS = radiusF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (numIterations >= 6 && numIterations % 2 !=1)
            {
                Vector3 StartF = new Vector3(0f, 0f, -numIterations * 1.5f);
                Vector3 EndF = new Vector3(0f, 0f, numIterations * 1.5f);

                Vector3 lineStartF = StartF * scaleFactor;
                Vector3 lineEndF = EndF * scaleFactor;

                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        //Destroy(robotControllerCProcess);
                    }

                    caveFormationProcess = robot.AddComponent<CaveFormation>();
                    caveFormationProcess.Initialize(robotId, centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                    if (robotControllerCProcess != null)
                    {
                        RobotControllerC robotControllerC = robot.GetComponent<RobotControllerC>();
                        robotControllerC.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerC robotControllerC = robot.AddComponent<RobotControllerC>();
                        robotControllerC.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }

                else
                {
                    Debug.Log("Robot object is null!");
                }
            }
            else
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                popup = FindObjectByName(canvasObject.transform, "Error Panel");
                if (popup != null)
                {
                    popup.SetActive(true);
                }
                Debug.Log("Robot numbers are not enough!");
            }
        }




        private void SendDone(string message)
        {
            // Send the message using your Unity-specific communication method
            // Replace the code below with your actual communication method
            Debug.Log($"Message Sent: {message}");
        }

    }

    [System.Serializable]
    public class PositionData
    {
        public string senderId;
        public double x;
        public double y;

        public PositionData(string senderId, double x, double y)
        {
            this.senderId = senderId;
            this.x = x;
            this.y = y;
        }
    }
}

