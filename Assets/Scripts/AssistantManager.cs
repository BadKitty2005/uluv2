using System.Collections;
using System.Linq;
using UnityEngine;

public class AssistantManager : MonoBehaviour
{
    public GameObject canv;
    [SerializeField] private AssistantAnimCont anim;

    private enum AssistantState
    {
        WaitingForWakeWord,
        Listening
    }

    private AssistantState currentState = AssistantState.WaitingForWakeWord;

    private string[] wakeWords = { "ассистент" };

    public SpeechRecognizer voiceRecognizer;
    public IntentRecognizer intentRecognizer;
    public CommandDispatcher dispatcher;

    void Start()
    {

        anim = GetComponent<AssistantAnimCont>();
        gameObject.SetActive(true);
        canv.SetActive(false);
        voiceRecognizer.OnTextRecognized.AddListener(OnVoiceCommand);

        StartCoroutine(WaitAndPlayIdle());
    }
    private IEnumerator WaitAndPlayIdle()
    {
        yield return null; // подождать один кадр, пока Awake и Start всех компонентов пройдут
        anim.PlayIdle();
    }
    void OnVoiceCommand(string text)
    {
        text = text.ToLower();

        switch (currentState)
        {
            case AssistantState.WaitingForWakeWord:
                if (wakeWords.Any(word => text.Contains(word)))
                {
                    anim.PlayWakeReaction();
                    canv.SetActive(false);
                    StartCoroutine(WaitAndStartListening());
                }
                break;

            case AssistantState.Listening:
                string intent = intentRecognizer.RecognizeIntent(text);

                if (string.IsNullOrEmpty(intent) || intent == "unknown")
                {
                    anim.PlayDenial();
                    StartCoroutine(ResetToIdle(anim.denialLength));
                }
                else
                {
                    anim.PlayPerforming();
                    dispatcher.ExecuteCommand(intent, text);
                    StartCoroutine(ResetToIdle(anim.actionLength));
                }
                break;
        }
    }

    private IEnumerator WaitAndStartListening()
    {
        yield return new WaitForSeconds(anim.wakeLength);
        anim.StartListening();
        currentState = AssistantState.Listening;
    }

    private IEnumerator ResetToIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.ReturnToIdle();
        currentState = AssistantState.WaitingForWakeWord;
    }

}
