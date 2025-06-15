using System.Collections.Generic;
using UnityEngine;

public class HeadManager : MonoBehaviour
{
    public Transform headAnchor; // —юда перетащим HeadAnchor из инспектора
    public List<GameObject> headPrefabs;
    private GameObject currentHead;

    public void SwitchHead(int index)
    {
        if (currentHead != null)
            Destroy(currentHead);

        currentHead = Instantiate(headPrefabs[index], headAnchor);
        currentHead.transform.localPosition = Vector3.zero;
        currentHead.transform.localRotation = Quaternion.identity;
    }
}
