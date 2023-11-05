using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    float rotationSpeed = 5f;
    float zoomSpeed = 2f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isDoubleClick = false;
    private float doubleClickTime = 0.3f; // Adjust as needed

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Right-click to control camera rotation
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Camera is rotating");
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.up, mouseX);
            transform.Rotate(Vector3.left, mouseY);

            // Keep camera parallel to the ground
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

        // Scroll wheel to zoom in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            transform.Translate(0, 0, scroll * -zoomSpeed, Space.Self);
            if (scroll < 0)
            { Debug.Log("Camera is zooming in"); }
            else if (scroll > 0)
            { Debug.Log("Camera is zooming out"); }
        }

        // Double left-click to reset camera
        if (Input.GetMouseButtonDown(0))
        {
            
            if (!isDoubleClick)
            {
                isDoubleClick = true;
                Invoke("ResetDoubleClick", doubleClickTime);
  
            }
            else
            {
                ResetCameraToInitial();
                isDoubleClick = false;
                Debug.Log("Camera is reset");
            }
        }
    }

    private void ResetDoubleClick()
    {
        isDoubleClick = false;
    }

    private void ResetCameraToInitial()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}







