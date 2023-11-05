using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelReset : MonoBehaviour
{

    // Start is called before the first frame update
    public void OpenPanel()
    {
        //if (panel != null)
        //{
            //bool isActive = panel.activeSelf;
            //panel.SetActive(!isActive);
            Debug.Log("End Task");
        //}
        ResetScene();
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
