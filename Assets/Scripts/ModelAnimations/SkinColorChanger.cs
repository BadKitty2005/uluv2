using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinColorChanger : MonoBehaviour
{
    public RawImage svBox;
    public RawImage hueStrip;
    public SkinnedMeshRenderer bodyRenderer;

    private Texture2D svTexture;
    private float hue = 0f;
    private float saturation = 1f;
    private float value = 1f;

    private const string HueKey = "Skin_H";
    private const string SatKey = "Skin_S";
    private const string ValKey = "Skin_V";
    private GameObject currentHead;

    public void SetHead(GameObject head)
    {
        currentHead = head;
        ApplyColor();
    }
    void Start()
    {
        GenerateHueStrip();
        GenerateSVBox();

        // Загружаем HSV
        hue = PlayerPrefs.GetFloat(HueKey, hue);
        saturation = PlayerPrefs.GetFloat(SatKey, saturation);
        value = PlayerPrefs.GetFloat(ValKey, value);

        UpdateSVBox();
        ApplyColor();
    }

    void GenerateHueStrip()
    {
        Texture2D tex = new Texture2D(1, 100);
        for (int y = 0; y < tex.height; y++)
        {
            Color c = Color.HSVToRGB(y / 100f, 1, 1);
            tex.SetPixel(0, y, c);
        }
        tex.Apply();
        hueStrip.texture = tex;
    }

    void GenerateSVBox()
    {
        svTexture = new Texture2D(100, 100);
        svBox.texture = svTexture;
    }

    void UpdateSVBox()
    {
        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                float s = x / 100f;
                float v = y / 100f;
                svTexture.SetPixel(x, y, Color.HSVToRGB(hue, s, v));
            }
        }
        svTexture.Apply();
    }

    public void OnHueDrag(BaseEventData data)
    {
        if (data is PointerEventData pointerData)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(hueStrip.rectTransform, pointerData.position, pointerData.pressEventCamera, out localPoint);

            float yNormalized = Mathf.InverseLerp(hueStrip.rectTransform.rect.yMin, hueStrip.rectTransform.rect.yMax, localPoint.y);
            hue = Mathf.Clamp01(yNormalized);

            UpdateSVBox();
            ApplyColor();
            SaveColor();
        }
    }

    public void OnSVDrag(BaseEventData data)
    {
        if (data is PointerEventData pointerData)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(svBox.rectTransform, pointerData.position, pointerData.pressEventCamera, out localPoint);

            float xNormalized = Mathf.InverseLerp(svBox.rectTransform.rect.xMin, svBox.rectTransform.rect.xMax, localPoint.x);
            float yNormalized = Mathf.InverseLerp(svBox.rectTransform.rect.yMin, svBox.rectTransform.rect.yMax, localPoint.y);

            saturation = Mathf.Clamp01(xNormalized);
            value = Mathf.Clamp01(yNormalized);

            ApplyColor();
            SaveColor();
        }
    }

    void ApplyColor()
    {
        Color c = Color.HSVToRGB(hue, saturation, value);
        if (currentHead != null)
        {
            Renderer headRenderer = currentHead.GetComponentInChildren<Renderer>();
            if (headRenderer != null)
            {
                headRenderer.material.color = c;
            }
            else
            {
                Debug.LogWarning("Renderer у головы не найден.");
            }
        }
        bodyRenderer.material.color = c;
        
    }

    void SaveColor()
    {
        PlayerPrefs.SetFloat(HueKey, hue);
        PlayerPrefs.SetFloat(SatKey, saturation);
        PlayerPrefs.SetFloat(ValKey, value);
        PlayerPrefs.Save();
    }
}
