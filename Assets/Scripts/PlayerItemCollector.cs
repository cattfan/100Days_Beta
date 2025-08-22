using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    //Private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //inventoryController = FindObjectOfType<InventoryController>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            // Lấy component Item từ object
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                // Gọi phương thức OnCollected trước khi thu thập
                item.OnCollected();

                // Add the item to the inventory
                // inventoryController.AddItem(item);
                item.Pickup();
                // Destroy the item after collection
                Destroy(collision.gameObject);

                // Debug log để kiểm tra
                Debug.Log($"Đã thu thập: {item.itemName} (ID: {item.itemID})");
            }
        }
    }
}