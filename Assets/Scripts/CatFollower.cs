using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public Camera arCamera;
    public float distance = 1.5f; // 항상 카메라 앞 1.5m

    void Update()
    {
        // 카메라의 위치 + 바라보는 방향 * 거리
        Vector3 frontPos = arCamera.transform.position + arCamera.transform.forward * distance;

        // 오브젝트 위치 갱신
        transform.position = frontPos;

        // (선택) 항상 플레이어를 바라보게 하려면
        Vector3 lookDir = arCamera.transform.position - transform.position;
        lookDir.y = 0; // 바닥만 따라가게 (필요하다면)
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
