using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // �������� ���� ������ body1
    public Vector3 offset = new Vector3(0, 5, -10); // �������� ������

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
