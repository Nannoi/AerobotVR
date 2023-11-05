using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TextSpeech;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    Text uiText;

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

    void OnFinalSpeechResult(string result)
    {
        uiText.text = result;
    }
    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }

    #endregion

    void Setup(string code)
    {
        TextToSpeech.Instance.Setting(code, 1, 1);
        SpeechToText.Instance.Setting(code);
    }
}
