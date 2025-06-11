using UnityEngine;
using UnityEngine.UI;

public class CalculateDistance : MonoBehaviour
{
    private double lastLat, lastLon;        // 바로 직전 위치
    private bool isStarted = false;
    public float movedDistance;

    public float targetDistance = 100;      // 목표: 100m
    float movementThreshold = 3f;           // 위치 갱신하기 위한 최소 임계값(미터터)
    float spikeThreshold = 20f;           // 위치 튀는 값 방지용용 임계값(미터터)
    public Text infoText;                   // 거리 확인용 UI
    private float updateInterval = 1.0f;
    private float timeSinceLastUpdate = 0f;     // 마지막 업데이트 후 경과 시간
    void Update()
    {
        //시작 상태가 아니라면 멈춤
        if (!isStarted)
        {
            if (infoText != null)
                infoText.text = "시작 버튼을 눌러주세요.";
            return;
        }

        // 지정된 시간(updateInterval)마다 거리 계산을 수행
        // GPS는 1초마다 업데이트 된다고 함
        // 따라서 매 프레임마다 업데이트는 불필요요
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate < updateInterval)
        {
            return; // 아직 업데이트할 시간이 아니면 함수 종료
        }
        timeSinceLastUpdate = 0f; // 타이머 리셋


        double curLat = GPSManager.Instance.latitude;
        double curLon = GPSManager.Instance.longitude;

        float delta = Haversine((float)lastLat, (float)lastLon, (float)curLat, (float)curLon);

        // 임계값 설정 이유 : 실내나 외부에서의 gps 정확하지 않을 수 있음.
        if (delta >= movementThreshold && delta < spikeThreshold) // 3m 이상, 20m 미만만 합산
            movedDistance += delta;

        // 현재 위치를 다음 계산을 위해 저장
        lastLat = curLat;
        lastLon = curLon;

        UpdateInfoUI();
    }

    // 버튼과 연결할 함수
    // 거리 측정 시작 함수
    public void SetStartPoint()
    {
        lastLat = GPSManager.Instance.latitude;
        lastLon = GPSManager.Instance.longitude;
        movedDistance = 0;       // 누적 거리 리셋
        isStarted = true;
        if (infoText != null)
            infoText.text = "시작 위치가 저장되었습니다. 이동을 시작하세요!";
    }

    private float Haversine(float lat1, float lon1, float lat2, float lon2)
    {
        float R = 6371000f; // 지구 반지름(m)
        float dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        float dLon = (lon2 - lon1) * Mathf.Deg2Rad;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1 * Mathf.Deg2Rad) * Mathf.Cos(lat2 * Mathf.Deg2Rad) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f - a));
        return R * c;
    }
    //UI 업데이트 함수
    private void UpdateInfoUI()
    {
        if (infoText != null)
            infoText.text = $"이동 거리: {movedDistance:F1} m / {targetDistance} m";

        if (movedDistance >= targetDistance)
        {
            infoText.text += targetDistance + "m 이동 성공!";
            // 한 번만 성공 처리하려면 isStarted = false;활용
        }
    }
}
