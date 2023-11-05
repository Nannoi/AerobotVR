using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TextSpeech;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using YourNamespace;


public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    private Text uiText;
    public InputField text = null;
    //private Animator animator;

    void Awake()
    {
        text.text = "Listenning";
    }
    private void Start()
    {

        Setup(LANG_CODE);


#if UNITY_ANDROID
        SpeechToText.Instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif

        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.Instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.Instance.onDoneCallback = OnspeakStop;

        CheckPermission();

    }

    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    #region Text to Speech

    public void StartSpeaking(string message)
    {
        TextToSpeech.Instance.StartSpeak(message);
    }

    public void StopSpeaking()
    {
        TextToSpeech.Instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking start...");
    }

    void OnspeakStop()
    {
        Debug.Log("Talking stopped");
    }

    #endregion

    #region Speech to Text

    public void StartListening()
    {

        SpeechToText.Instance.StartRecording();
    }

    public void StopListening()
    {
        SpeechToText.Instance.StopRecording();
    }

    public void OnFinalSpeechResult(string result)
    {
        // Start a coroutine to update the InputField text
        StartCoroutine(UpdateInputFieldText(result));
    }

    private IEnumerator UpdateInputFieldText(string newText)
    {
        // Wait for a short time to ensure the InputField is ready to be updated
        yield return new WaitForSeconds(0.1f);

        // Set the text of the InputField to the new text
        text.text = newText;

        // Start the ShowListeningText coroutine
        StartCoroutine(ShowListeningText());
    }

    IEnumerator ShowListeningText()
    {
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Set the text to "Listening"
        text.text = "Listenning";
    }

    void OnPartialSpeechResult(string result)
     {
         text.text = result;

     }

        #endregion

     void Setup(string code)
     {
         TextToSpeech.Instance.Setting(code, 1, 1);
         SpeechToText.Instance.Setting(code);
     }
       
    }


