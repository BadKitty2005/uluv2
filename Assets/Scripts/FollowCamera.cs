using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Перетащи сюда объект body1
    public Vector3 offset = new Vector3(0, 5, -10); // Смещение камеры

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
