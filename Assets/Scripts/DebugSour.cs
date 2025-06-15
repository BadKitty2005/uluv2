using UnityEngine;

public class DebugSour : MonoBehaviour
{
    void Start()
    {
        Debug.Log("�������� Resource ����...");

        var all = Resources.LoadAll("");
        Debug.Log($"Resources.LoadAll ������: {all.Length} ��������");

        foreach (var o in all)
        {
            if (o == null)
            {
                Debug.LogError("��������� null-����� � Resources!");
            }
            else
            {
                Debug.Log($"����� {o.name} ({o.GetType()})");
            }
        }

        Debug.Log("�������� Resources ���������");
    }
}
