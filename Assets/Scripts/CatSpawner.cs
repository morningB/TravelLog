// CatSpawner.cs
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject catPrefab;
    public Camera arCamera;
    public float followDistance = 1.5f;

    private GameObject spawnedCat;

    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;
    }

    // 버튼 OnClick에 이 함수를 연결하세요.
    public void OnSpawnCatButton()
    {
        if (spawnedCat != null) return;  // 한 번만 생성

        // 고양이 인스턴스 생성
        spawnedCat = Instantiate(catPrefab);
        
        // 카메라 추종 스크립트 붙이고 파라미터 세팅
        var follower = spawnedCat.AddComponent<CatFollower>();
        follower.arCamera = arCamera;
        follower.distance = followDistance;
    }
}
