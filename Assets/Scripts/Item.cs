using UnityEngine;

[System.Serializable]
public enum ItemRarity
{
    Common,     // Thường - Trắng
    Uncommon,   // Không thường - Xanh lá
    Rare,       // Hiếm - Xanh dương  
    Epic,       // Sử thi - Tím
    Legendary   // Huyền thoại - Cam/Vàng
}

[System.Serializable]
public class Item : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName = "New Item";
    public string description = "Item description";
    public Sprite itemIcon;
    public int itemID;

    [Header("Rarity & Drop System")]
    public ItemRarity rarity = ItemRarity.Common;
    [Range(0f, 100f)]
    public float dropRate = 50f;           // Tỉ lệ drop (%) - 0-100
    [Range(1, 10)]
    public int minDropAmount = 1;          // Số lượng tối thiểu khi drop
    [Range(1, 10)]
    public int maxDropAmount = 1;          // Số lượng tối đa khi drop

    [Header("Shop Properties")]
    public int sellPrice = 10;
    public int buyPrice = 5;
    public bool canSell = true;

    [Header("Stack Properties")]
    public bool isStackable = false;
    public int maxStackSize = 1;
    public int currentAmount = 1;

    void Start()
    {
        // Đảm bảo object có tag "Item"
        if (!gameObject.CompareTag("Item"))
        {
            gameObject.tag = "Item";
        }

        // Đảm bảo có Collider2D và set là Trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<CircleCollider2D>();
        }
        col.isTrigger = true;

        // Adjust giá trị dựa trên rarity
        AdjustPriceByRarity();
    }

    // Phương thức kiểm tra có drop hay không
    public bool ShouldDrop()
    {
        float randomValue = Random.Range(0f, 100f);
        return randomValue <= dropRate;
    }

    // Phương thức random số lượng drop
    public int GetRandomDropAmount()
    {
        return Random.Range(minDropAmount, maxDropAmount + 1);
    }

    // Phương thức tạo item với số lượng random
    public GameObject CreateRandomDrop(Vector3 position)
    {
        if (!ShouldDrop())
        {
            return null; // Không drop
        }

        GameObject droppedItem = Instantiate(gameObject, position, Quaternion.identity);
        Item itemComponent = droppedItem.GetComponent<Item>();

        if (itemComponent.isStackable)
        {
            itemComponent.currentAmount = GetRandomDropAmount();
        }

        Debug.Log($"Đã drop {itemComponent.itemName} x{itemComponent.currentAmount} tại {position}");
        return droppedItem;
    }

    // Điều chỉnh giá dựa trên độ hiếm
    void AdjustPriceByRarity()
    {
        float multiplier = GetRarityMultiplier();
        sellPrice = Mathf.RoundToInt(sellPrice * multiplier);
        buyPrice = Mathf.RoundToInt(buyPrice * multiplier);
    }

    // Lấy hệ số nhân dựa trên độ hiếm
    float GetRarityMultiplier()
    {
        switch (rarity)
        {
            case ItemRarity.Common: return 1f;
            case ItemRarity.Uncommon: return 1.5f;
            case ItemRarity.Rare: return 3f;
            case ItemRarity.Epic: return 6f;
            case ItemRarity.Legendary: return 12f;
            default: return 1f;
        }
    }

    // Lấy màu theo độ hiếm
    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case ItemRarity.Common: return Color.white;
            case ItemRarity.Uncommon: return Color.green;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return new Color(0.5f, 0f, 1f); // Purple
            case ItemRarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
            default: return Color.white;
        }
    }

    // Phương thức pickup khi player chạm vào item
    public virtual void Pickup()
    {
        // Lấy sprite từ SpriteRenderer hoặc itemIcon đã assign
        Sprite currentItemIcon = itemIcon;
        if (currentItemIcon == null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                currentItemIcon = spriteRenderer.sprite;
            }
        }

        // Hiển thị pickup notification
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(itemName, currentItemIcon);
        }

        // Gọi OnCollected để xử lý logic thu thập
        OnCollected();

        // Destroy item sau khi pickup
        Destroy(gameObject);
    }

    // Phương thức được gọi khi item được thu thập
    public virtual void OnCollected()
    {
        Debug.Log($"Đã thu thập {itemName} ({rarity}) - Giá bán: {sellPrice} gold");
    }

    // Phương thức để bán item
    public virtual int SellItem()
    {
        if (canSell)
        {
            int totalValue = GetTotalValue();
            Debug.Log($"Đã bán {itemName} x{currentAmount} với tổng giá {totalValue} gold");
            return totalValue;
        }
        else
        {
            Debug.Log($"{itemName} không thể bán được!");
            return 0;
        }
    }

    // Phương thức để lấy thông tin item
    public virtual string GetItemInfo()
    {
        string rarityText = GetRarityText();
        string info = $"Tên: {itemName}\nĐộ hiếm: {rarityText}\nMô tả: {description}\nGiá bán: {sellPrice} gold\nTỉ lệ drop: {dropRate}%";

        if (isStackable)
        {
            info += $"\nSố lượng: {currentAmount}/{maxStackSize}";
        }

        return info;
    }

    // Lấy text độ hiếm bằng tiếng Việt
    string GetRarityText()
    {
        switch (rarity)
        {
            case ItemRarity.Common: return "Thường";
            case ItemRarity.Uncommon: return "Không thường";
            case ItemRarity.Rare: return "Hiếm";
            case ItemRarity.Epic: return "Sử thi";
            case ItemRarity.Legendary: return "Huyền thoại";
            default: return "Không xác định";
        }
    }

    // Phương thức để tăng số lượng trong stack
    public bool AddToStack(int amount)
    {
        if (!isStackable) return false;

        int newAmount = currentAmount + amount;
        if (newAmount <= maxStackSize)
        {
            currentAmount = newAmount;
            return true;
        }
        return false;
    }

    // Phương thức để lấy tổng giá trị của stack
    public int GetTotalValue()
    {
        return sellPrice * currentAmount;
    }
}