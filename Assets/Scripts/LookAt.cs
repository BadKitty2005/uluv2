using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target; // Перетащи сюда нужный объект в инспекторе

    void Update()
    {
        transform.LookAt(target); // Камера будет смотреть на цель
    }
}
