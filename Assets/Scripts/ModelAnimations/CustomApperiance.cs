using System.Collections.Generic;
using UnityEngine;

public class CustomApperiance : MonoBehaviour
{
    public Transform headSocket;
    public List<GameObject> headPrefabs;
    public Renderer skinRenderer;
    public GameObject eyeSlot;
    public GameObject mouthSlot;

    GameObject currentHead;
    int headIndex;
    Color skinColor;

    void Start()
    {
        LoadAppearance();
    }

    public void SetHead(int index)
    {
        if (currentHead) Destroy(currentHead);
        currentHead = Instantiate(headPrefabs[index], headSocket);
        headIndex = index;
        SaveAppearance();
    }

    public void SetEyes(Texture tex)
    {
        eyeSlot.GetComponent<Renderer>().material.mainTexture = tex;
        SaveAppearance();
    }

    public void SetMouth(Texture tex)
    {
        mouthSlot.GetComponent<Renderer>().material.mainTexture = tex;
        SaveAppearance();
    }

    public void SetSkinColor(Color color)
    {
        skinColor = color;
        skinRenderer.material.color = color;
        SaveAppearance();
    }

    public void OnColorSliderChange(float r, float g, float b)
    {
        SetSkinColor(new Color(r, g, b, 1f));
    }

    void SaveAppearance()
    {
        PlayerPrefs.SetInt("HeadIndex", headIndex);
        PlayerPrefs.SetString("SkinColor", ColorUtility.ToHtmlStringRGBA(skinColor));
        PlayerPrefs.Save();
    }

    void LoadAppearance()
    {
        if (PlayerPrefs.HasKey("HeadIndex"))
            SetHead(PlayerPrefs.GetInt("HeadIndex"));

        if (PlayerPrefs.HasKey("SkinColor") && ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("SkinColor"), out Color savedColor))
            SetSkinColor(savedColor);
    }
}
