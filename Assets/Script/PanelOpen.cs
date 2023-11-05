using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelOpen : MonoBehaviour
{
    public GameObject panel;

    // Start is called before the first frame update
    public void OpenPanel()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            
        }
     
    }

}
