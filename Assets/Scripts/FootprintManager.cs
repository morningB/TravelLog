using UnityEngine;
using UnityEngine.UI;

public class FootprintManager : MonoBehaviour
{
    public GameObject footprintPrefab;
    public Camera arCamera;
    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main; // "MainCamera" 태그가 붙은 카메라 자동 할당
    }
    public void PlaceFootprintInFront()
    {
        Vector3 spawnPos = arCamera.transform.position + arCamera.transform.forward * 1.5f;
        Instantiate(footprintPrefab, spawnPos, Quaternion.identity);
    }
}
