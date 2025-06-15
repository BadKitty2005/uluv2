using UnityEngine;

public class AssistantAnimCont : MonoBehaviour
{
    private Animator anima;

    public float actionLength { get; private set; } = 2f;
    public float denialLength { get; private set; } = 2f;
    public float wakeLength { get; private set; } = 2f;

    private void Awake()
    {
        anima = GetComponent<Animator>();
        if (anima == null)
        {
            Debug.LogError("Animator �� ������! �������, ��� Animator ���� �� �������.");
        }
        else
        {
            Debug.Log($"Animator ������: {anima.gameObject.name}");
        }
    }

    private void Start()
    {
        ResetAllBools();

        if (anima.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator Controller �� ��������!");
            return;
        }

        AnimationClip[] clips = anima.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name.Contains("Action"))
                actionLength = clip.length;
            else if (clip.name.Contains("Denial"))
                denialLength = clip.length;
            else if (clip.name.Contains("Wake") || clip.name.Contains("Reaction"))
                wakeLength = clip.length;
        }
    }

    public void PlayIdle()
    {
        ResetAllBools();
    }

    public void PlayWakeReaction()
    {
        ResetAllBools();
        anima.SetBool("IsWaking", true);
    }

    public void StartListening()
    {
        ResetAllBools();
        anima.SetBool("IsListening", true);
    }

    public void PlayPerforming()
    {
        Debug.Log("������ Play����������");
        ResetAllBools();
        anima.SetBool("IsPerforming", true);
    }

    public void PlayDenial()
    {
        Debug.Log("������ PlayDenial");
        ResetAllBools();
        anima.SetBool("IsDenial", true);
    }

    public void ReturnToIdle()
    {
        ResetAllBools();
    }

    private void ResetAllBools()
    {
        if (anima == null)
        {
            Debug.LogError("Animator �� ��� null � ResetAllBools!");
            return;
        }

        anima.SetBool("IsWaking", false);
        anima.SetBool("IsListening", false);
        anima.SetBool("IsPerforming", false);
        anima.SetBool("IsDenial", false);
    }
}
