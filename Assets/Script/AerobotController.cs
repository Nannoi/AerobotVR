using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;

[RequireComponent(typeof(Animator))]
public class AerobotController : MonoBehaviour
{
    private Text uiText;
    private Text previousText;
    private Animator animator;
    private InputField text;
    private string height="2 m";
    private string state ="standby";
    private string stringLength ="1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

    private Text heightData;
    private Text stringLData;
    private Dropdown dropdown;


    private void Start()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        GameObject aeroTransform = FindObjectByName(canvasObject.transform, "Action");
        dropdown = aeroTransform.GetComponent<Dropdown>();
        // Add a listener to the onValueChanged event of the InputField component
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        GameObject heightG = FindObjectByName(canvasObject.transform, "HeightText");
        heightData = heightG.GetComponent<Text>();

        GameObject stringG = FindObjectByName(canvasObject.transform, "Stringdata");
        stringLData = stringG.GetComponent<Text>();

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
    void OnDropdownValueChanged(int result)
    {
        animator = GetComponent<Animator>();
        AerobotAnim(result);
        heightData.text = height.ToString(); // Convert the Action to a string
        stringLData.text = stringLength.ToString();
        
    }

    // Update is called once per frame

    void AerobotAnim(int result)
    {      
        if (result==1)
        {
            height = "2 m";
            state = "standby";
            stringLength = "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", true);
        }

        else if (result==2)
        {
            height = "1.8 m";
            state = "open";
            stringLength = "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.2 m" + System.Environment.NewLine +
                "1.2 m";

            animator.SetBool("Bend01", true);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);
        }

        else if (result==3)
        {
            height = "1.5 m";
            state = "cover";
            stringLength = "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.0 m" + System.Environment.NewLine +
                "1.0 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", true);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);
        }

        else if (result==4)
        {
            height = "1.5 m";
            state = "special";
            stringLength = "1.2 m" + System.Environment.NewLine +
                "1.2 m" + System.Environment.NewLine +
                "1.2 m" + System.Environment.NewLine +
                "1.2 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", true);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);
        }

        else if (result==5)
        {
            height = "1.8 m";
            state = "closed";
            stringLength = "1.2 m" + System.Environment.NewLine +
                "1.2 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", true);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);
        }

        else if (result==6)
        {
            height = "1.5 m";
            state = "reverse";
            stringLength = "1.0 m" + System.Environment.NewLine +
                "1.0 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", true);
            animator.SetBool("StandBy", false);
        }

        else if (result==7)
        {
            StartCoroutine(BloomCoroutine());
        }
        else if (result==8)
        {
            StartCoroutine(WaveCoroutine());
        }

        IEnumerator BloomCoroutine()
        {
            height = "1.5 m";
            state = "bloom";
            stringLength = "1.0 m" + System.Environment.NewLine +
                "1.0 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", true);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.0f);

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", true);
            animator.SetBool("StandBy", false);
        }

        IEnumerator WaveCoroutine()
        {
            height = "1.5 m";
            state = "wave";
            stringLength = "1.0 m" + System.Environment.NewLine +
                "1.0 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", true);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.0f);

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", true);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.5f);

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", true);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.0f);

            animator.SetBool("Bend01", true);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.0f);

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", false);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", true);
            animator.SetBool("StandBy", false);

            yield return new WaitForSeconds(1.0f);

            animator.SetBool("Bend01", false);
            animator.SetBool("Bend02", true);
            animator.SetBool("Bend03", false);
            animator.SetBool("Bend04", false);
            animator.SetBool("Bend05", false);
            animator.SetBool("StandBy", false);
        }

        Debug.Log("Aerobot Action: " + state);

    }
}
     

