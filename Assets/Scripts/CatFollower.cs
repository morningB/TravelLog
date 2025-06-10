using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public Camera arCamera;
    public float distance = 1.5f; // 항상 카메라 앞 1.5m

    void Update()
    {
        // 카메라 앞에 고정
        Vector3 frontPos = arCamera.transform.position + arCamera.transform.forward * distance;
        transform.position = frontPos;

        // 카메라 바라보게
        Vector3 lookDir = arCamera.transform.position - transform.position;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
