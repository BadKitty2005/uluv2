using UnityEngine;
using UnityEditor;

public class SceneValidator : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Проверить сцену на битые компоненты")]
    void CheckScene()
    {
        var allGameObjects = UnityEngine.SceneManagement.SceneManager
            .GetActiveScene()
            .GetRootGameObjects();

        foreach (var root in allGameObjects)
        {
            CheckGameObjectRecursive(root);
        }

        Debug.Log("Проверка завершена.");
    }

    void CheckGameObjectRecursive(GameObject go)
    {
        var components = go.GetComponents<Component>();
        foreach (var c in components)
        {
            if (c == null)
                Debug.LogError($"Missing Component на GameObject: {go.name}", go);
        }

        foreach (Transform child in go.transform)
        {
            CheckGameObjectRecursive(child.gameObject);
        }
    }
#endif
}
