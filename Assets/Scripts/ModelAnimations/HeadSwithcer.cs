using System;
using UnityEngine;

public class HeadSwithcer : MonoBehaviour
{
    
    public Transform headAnchor;                    // точка крепления головы (headanchor в риге)
    public GameObject[] headPrefabs;                // головы
    public Material[] eyeMaterials;                 // материалы глаз
    public Material[] mouthMaterials;               // материалы рта

    public Transform eyeQuad;                       // внешний Quad для глаз (НЕ внутри головы!)
    public Transform mouthQuad;                     // внешний Quad для рта

    private GameObject currentHead;
    public SkinColorChanger skinColorChanger;

    void Start()
    {
        int savedHead = PlayerPrefs.GetInt("SelectedHead", 0);
        int savedEye = PlayerPrefs.GetInt("SelectedEye", 0);
        int savedMouth = PlayerPrefs.GetInt("SelectedMouth", 0);

        SwitchHead(savedHead);
        SetEye(savedEye);
        SetMouth(savedMouth);
        
    }

    public void SwitchHead(int index)
    {
        if (index < 0 || index >= headPrefabs.Length)
        {
            Debug.LogWarning("Неверный индекс головы.");
            return;
        }

        PlayerPrefs.SetInt("SelectedHead", index);

        // Удалить предыдущую голову
        if (currentHead != null)
        {
            Destroy(currentHead);
        }

        // Создание головы
        currentHead = Instantiate(headPrefabs[index], headAnchor);
        currentHead.transform.localPosition = new Vector3(0f, -1.25f, 0f); // подгон по месту
        if (index == 1)  // вторая голова, поправим
        {
            currentHead.transform.localPosition = new Vector3(2.3f, -1.25f, 0f); // подгони вручную
        }

        currentHead.transform.localRotation = Quaternion.identity;
        currentHead.transform.localScale = new Vector3(0.7f, 0.7f, 0.5f); // более плоская голова

        // Отключить камеры, если есть
        Camera[] childCameras = currentHead.GetComponentsInChildren<Camera>(true);
        foreach (var cam in childCameras)
        {
            cam.enabled = false;
        }
        //SkinColorChanger picker = UnityEngine.Object.FindFirstObjectByType<SkinColorChanger>();
        //if (picker != null)
        //{
        //    picker.SetHead(currentHead);
        //}
        if (skinColorChanger != null)
        {
            skinColorChanger.SetHead(currentHead);
        }

        Debug.Log($"Голова сменена на {index}");
    }

    public void SetEye(int materialIndex)
    {
        if (eyeQuad == null)
        {
            Debug.LogWarning("eyeQuad не найден");
            return;
        }

        var renderer = eyeQuad.GetComponent<MeshRenderer>();
        if (renderer != null && materialIndex < eyeMaterials.Length)
        {
            renderer.material = eyeMaterials[materialIndex];
            Debug.Log($"Материал глаз установлен на {materialIndex}");
            PlayerPrefs.SetInt("SelectedEye", materialIndex);
        }
    }

    public void SetMouth(int materialIndex)
    {
        if (mouthQuad == null)
        {
            Debug.LogWarning("mouthQuad не найден");
            return;
        }

        var renderer = mouthQuad.GetComponent<MeshRenderer>();
        if (renderer != null && materialIndex < mouthMaterials.Length)
        {
            renderer.material = mouthMaterials[materialIndex];
            Debug.Log($"Материал рта установлен на {materialIndex}");
            PlayerPrefs.SetInt("SelectedMouth", materialIndex);
        }
    }

}
