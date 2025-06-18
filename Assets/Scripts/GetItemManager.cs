using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GetItemManager : MonoBehaviour
{
    private RaycastHit hitInfo;
    public int getItemCount;
    public int maxItemCount = 5;
    public Text itemText; 
    public Text completeText; // 완료시 텍스트 띄우기(다른거로 대체할 예정)

    void Start()
    {
        completeText.enabled = false;
    }
    void Update()
    {
        if (getItemCount == maxItemCount)
        {
            completeText.enabled = true;
        }
        if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray screenRay = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(screenRay.origin, screenRay.direction * 1000f, out hitInfo))
                {
                    if (hitInfo.collider.CompareTag("Item"))
                    {
                        hitInfo.collider.gameObject.SetActive(false);
                        getItemCount++;

                        itemText.text = "획득 아이템 수 : " + getItemCount + "/ " + maxItemCount;
                    }
                }
            }
    }
}
