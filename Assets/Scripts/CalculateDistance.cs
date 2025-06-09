using UnityEngine;
using UnityEngine.UI;

public class CalculateDistance : MonoBehaviour
{
    private double lastLat, lastLon;        // 바로 직전 위치
    private bool isStarted = false;
    public float movedDistance = 0;
    public float targetDistance = 100;      // 목표: 100m
    public Text infoText;                   // 테스트용 UI

    void Update()
    {
        if (!isStarted)
        {
            if (infoText != null)
                infoText.text = "시작 버튼을 눌러주세요.";
            return;
        }

        double curLat = GPSManager.Instance.latitude;
        double curLon = GPSManager.Instance.longitude;

        float delta = Haversine((float)lastLat, (float)lastLon, (float)curLat, (float)curLon);
        float threshold = 5f;

        // 임계값 설정 이유 : 실내나 외부에서의 gps 정확하지 않을 수 있음.
        if (delta >= threshold && delta < 30f) // 5m 이상, 30m 미만만 합산
            movedDistance += delta;

        // 현재 위치를 다음 계산을 위해 저장
        lastLat = curLat;
        lastLon = curLon;

        if (infoText != null)
            infoText.text = $"이동 거리: {movedDistance:F1} m / {targetDistance} m";

        if (movedDistance >= targetDistance)
        {
            infoText.text += "\n100m 이동 성공!";
            // 한 번만 성공 처리하려면 isStarted = false;활용
        }
    }

    // 버튼과 연결할 함수
    public void SetStartPoint()
    {
        lastLat = GPSManager.Instance.latitude;
        lastLon = GPSManager.Instance.longitude;
        movedDistance = 0;       // 누적 거리 리셋
        isStarted = true;
        if (infoText != null)
            infoText.text = "시작 위치가 저장되었습니다. 이동을 시작하세요!";
    }

    private static float Haversine(float lat1, float lon1, float lat2, float lon2)
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

}
