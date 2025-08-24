using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slotObj = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

            // Tạm thời không dùng Slot component
            if (i < itemPrefabs.Length && itemPrefabs[i] != null)
            {
                GameObject item = Instantiate(itemPrefabs[i], slotObj.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slotObj.currentItem = item;
            }
        }
    }
}
