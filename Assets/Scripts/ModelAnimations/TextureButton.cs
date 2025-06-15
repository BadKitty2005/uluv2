using UnityEngine;

public class TextureButton : MonoBehaviour
{
    public CustomApperiance appearance;
    public Texture targetTexture;
    public bool isEye;

    public void Apply()
    {
        if (isEye)
            appearance.SetEyes(targetTexture);
        else
            appearance.SetMouth(targetTexture);
    }
}
