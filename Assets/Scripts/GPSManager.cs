using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
    // 위도 경도 확인용 ui (나중에 삭제 예정)
    public Text text_ui; 
    //싱글톤 인스턴스
    public static GPSManager Instance { get; private set; }

    //최신 GPS 정보 보관(위도, 경도)
    public double latitude { get; private set; }
    public double longitude { get; private set; }
    // 높이(필요할지는 모르지만 혹시 모르니)
    // public double altitude { get; private set; }
    private float desiredAccuracyInMeters = 0.5f;
    private float updateDistanceInMeters = 0.5f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 만약 씬 이동에도 유지되어야한다면
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 코루틴 활용
    void Start()
    {
        StartCoroutine(InitializeLocationService());
    }

    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // 위도/경도 실시간 표시
            UpdateLocationData();
            UpdateUIText();
        }
        else
        {
            text_ui.text = "Need to active GPS";
        }
    }
     // Unity Location Script 문서에서 가져온 코드
    private IEnumerator InitializeLocationService()
    {
        // 위치 권한 요청 루프
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            yield return null;
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Start(얼마나 정확하게 위치를 잡을지, 얼마나 자주 위치를 갱신할지)
        Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);


        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }


        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else // LocationServiceStatus가 Running이라는 상태
        {

            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            text_ui.text = Input.location.lastData.latitude + " / " + Input.location.lastData.longitude;


        }
    }
    //최신 위치 데이터 저장
    private void UpdateLocationData()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        //altitude = Input.location.lastData.altitude;
    }

    //GPS 갱신 확인용 UI (나중에 삭제 예정)
    private void UpdateUIText()
    {
        if (text_ui != null)
        {
            string lat = latitude.ToString("F6");
            string lon = longitude.ToString("F6");
            text_ui.text = $"{lat} / {lon}";
        }
    }
}
