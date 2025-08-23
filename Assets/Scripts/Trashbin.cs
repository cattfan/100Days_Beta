using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trashbin : MonoBehaviour, IInteractable
{
    public bool isChecked { get; private set; } // Flag to check if the trashbin is interactable
    public string TrashbinName { get; private set; } // Name of the trashbin
    public GameObject FailInteractIcon; // Icon to show when interaction fails

    [Header("Random Item Settings")]
    public GameObject[] itemPrefabs; // Array các prefab item có thể spawn
    public float spawnChance = 0.8f; // Tỷ lệ % spawn item (0.8 = 80%)
    public int minItems = 1; // Số lượng item tối thiểu
    public int maxItems = 3; // Số lượng item tối đa

    [Header("Spawn Settings")]
    public float spawnRadius = 1.5f; // Bán kính spawn item xung quanh trashbin
    public Vector3 spawnOffset = Vector3.down; // Offset spawn position

    [Header("Visual Settings")]
    public Sprite CheckedBin; // Sprite to show when the trashbin is checked
    public Sprite UncheckedBin; // Sprite để hiển thị khi trashbin chưa check

    [Header("Reset Settings")]
    public float resetTime = 60f; // Thời gian reset (60 giây = 1 phút)
    public bool showResetTimer = true; // Hiển thị timer trong console

    private Sprite originalSprite; // Sprite gốc của trashbin
    private Coroutine resetCoroutine; // Reference đến coroutine reset
    [Header("Music")]
    public AudioManagement audioManagement;

    private void Awake()
    {
        audioManagement = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagement>();
    }
    void Start()
    {
        TrashbinName ??= Global_Helper.GenerateUniqueID(gameObject); // Generate a unique ID for the trashbin if not already set

        FailInteractIcon.SetActive(false); // Ẩn icon fail lúc đầu
        // Lưu sprite gốc
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
        }

        // Nếu có UncheckedBin sprite thì dùng, không thì dùng originalSprite
        if (UncheckedBin == null)
        {
            UncheckedBin = originalSprite;
        }
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        CheckTrashbin(); // Check the trashbin if interaction is allowed
    }

    public bool CanInteract()
    {
        // Logic to determine if the trashbin can be interacted with
        return !isChecked;
    }

    private void CheckTrashbin()
    {
        SetChecked(true);// Set the trashbin as checked

        

        // Bắt đầu countdown để reset
        StartResetTimer();

        // Random chance to spawn items
        if (Random.Range(0f, 1f) <= spawnChance)
        {
            audioManagement.PlaySFX(audioManagement.SuccessTrashbinInteract);
            SpawnRandomItems();
        }
        else
        {
            audioManagement.PlaySFX(audioManagement.FailTrashbinInteract);
            FailInteractIcon.SetActive(true);
            StartCoroutine(HideFailIconAfterDelay(1f)); // Hide the icon after 2 seconds
        }
    }

    private IEnumerator HideFailIconAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FailInteractIcon.SetActive(false);
    }

    private void SpawnRandomItems()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            Debug.LogWarning("No item prefabs assigned to trashbin!");
            return;
        }

        // Random số lượng item sẽ spawn
        int itemCount = Random.Range(minItems, maxItems + 1);

        for (int i = 0; i < itemCount; i++)
        {
            // Random chọn 1 item prefab
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            GameObject selectedPrefab = itemPrefabs[randomIndex];

            if (selectedPrefab != null)
            {
                // Random vị trí spawn trong bán kính
                Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = transform.position + spawnOffset + new Vector3(randomOffset.x, randomOffset.y, 0);

                // Spawn item
                GameObject droppedItem = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

                // Thêm hiệu ứng nhỏ cho item vừa spawn (optional)
                StartCoroutine(ItemSpawnEffect(droppedItem));

                Debug.Log($"Spawned {selectedPrefab.name} from trashbin!");
            }
        }

        Debug.Log($"Found {itemCount} items in trashbin!");
    }

    // Hiệu ứng nhỏ khi item spawn (optional)
    private IEnumerator ItemSpawnEffect(GameObject item)
    {
        if (item == null) yield break;

        Vector3 originalScale = item.transform.localScale;
        item.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        float animationTime = 0.3f;

        while (elapsedTime < animationTime)
        {
            if (item == null) yield break;

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationTime;
            item.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            yield return null;
        }

        if (item != null)
            item.transform.localScale = originalScale;
    }

    private void SetChecked(bool value)
    {
        isChecked = value;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (isChecked && CheckedBin != null)
            {
                spriteRenderer.sprite = CheckedBin;
                Debug.Log("Trashbin checked - sprite changed!");
            }
            else if (!isChecked && UncheckedBin != null)
            {
                spriteRenderer.sprite = UncheckedBin;
                Debug.Log("Trashbin reset - sprite restored!");
            }
        }
    }

    // Bắt đầu timer reset
    private void StartResetTimer()
    {
        // Dừng timer cũ nếu có
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        // Bắt đầu timer mới
        resetCoroutine = StartCoroutine(ResetTrashbinTimer());
    }

    // Coroutine đếm ngược và reset trashbin
    private IEnumerator ResetTrashbinTimer()
    {
        float timeRemaining = resetTime;

        while (timeRemaining > 0)
        {
            // Hiển thị timer trong console (optional)
            if (showResetTimer && Mathf.FloorToInt(timeRemaining) % 10 == 0 && timeRemaining == Mathf.Floor(timeRemaining))
            {
                Debug.Log($"Trashbin '{TrashbinName}' will reset in {Mathf.FloorToInt(timeRemaining)} seconds");
            }

            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        // Reset trashbin về trạng thái ban đầu
        ResetTrashbin();
    }

    // Reset trashbin về trạng thái có thể tương tác
    private void ResetTrashbin()
    {
        SetChecked(false);
        resetCoroutine = null;
        Debug.Log($"Trashbin '{TrashbinName}' has been reset and is ready to be searched again!");
    }

    // Phương thức public để reset manual (có thể gọi từ code khác)
    public void ForceReset()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        ResetTrashbin();
    }

    // Lấy thời gian còn lại để reset
    public float GetTimeUntilReset()
    {
        if (resetCoroutine == null || !isChecked)
            return 0f;

        // Tính thời gian còn lại (chỉ estimate)
        return resetTime; // Có thể cải thiện để tính chính xác hơn
    }

}