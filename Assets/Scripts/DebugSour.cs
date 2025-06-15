using UnityEngine;

public class DebugSour : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Запускаю Resource тест...");

        var all = Resources.LoadAll("");
        Debug.Log($"Resources.LoadAll вернул: {all.Length} объектов");

        foreach (var o in all)
        {
            if (o == null)
            {
                Debug.LogError("Обнаружен null-ассет в Resources!");
            }
            else
            {
                Debug.Log($"супер {o.name} ({o.GetType()})");
            }
        }

        Debug.Log("Проверка Resources завершена");
    }
}
