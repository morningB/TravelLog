using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GetItemManager : MonoBehaviour
{
    private RaycastHit hitInfo;
    public int getItemCount;
    public Text itemText;

    void Update()
    {
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

                    itemText.text = "획득 아이템 수 : " + getItemCount + "/ 5";
                }
            }
        }
    }
}
