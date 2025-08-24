using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("=== BEGIN DRAG ===");
        originalParent = transform.parent;
        Debug.Log($"Original parent: {originalParent.name}");

        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        Debug.Log("BEGIN DRAG COMPLETED");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        // Debug.Log($"Dragging to: {eventData.position}"); // Uncomment để test
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("=== END DRAG START ===");

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Debug.Log("Canvas group restored");

        Debug.Log($"pointerEnter: {eventData.pointerEnter?.name}");

        if (eventData.pointerEnter == null)
        {
            Debug.LogWarning("pointerEnter is NULL!");
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Debug.Log("=== END DRAG FINISHED ===");
            return;
        }

        Slot dropSlot = null;
        dropSlot = eventData.pointerEnter.GetComponent<Slot>() ?? eventData.pointerEnter.GetComponentInParent<Slot>();

        if (dropSlot == null && eventData.pointerEnter.transform.parent != null)
        {
            dropSlot = eventData.pointerEnter.transform.parent.GetComponent<Slot>();
        }

        Debug.Log($"Final drop slot found: {dropSlot?.name}");

        Slot originalSlot = originalParent.GetComponent<Slot>();
        Debug.Log($"Original slot: {originalSlot?.name}");

        if (dropSlot != null)
        {
            Debug.Log("=== VALID DROP DETECTED ===");
            GameObject draggedItem = gameObject;
            GameObject targetItem = dropSlot.currentItem;

            if (originalSlot != null)
            {
                // Always clear the original slot's item reference
                originalSlot.currentItem = null;
            }

            if (targetItem != null && targetItem != draggedItem)
            {
                Debug.Log("=== SWAPPING ITEMS ===");
                targetItem.transform.SetParent(originalParent);
                targetItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                originalSlot.currentItem = targetItem;

                draggedItem.transform.SetParent(dropSlot.transform);
                draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                dropSlot.currentItem = draggedItem;

                Debug.Log("=== SWAP COMPLETED ===");
            }
            else if (targetItem == null)
            {
                Debug.Log("=== MOVING TO EMPTY SLOT ===");
                draggedItem.transform.SetParent(dropSlot.transform);
                draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                dropSlot.currentItem = draggedItem;

                Debug.Log("=== MOVE COMPLETED ===");
            }
            else
            {
                Debug.Log("=== DROPPING ON SAME ITEM - RETURNING ===");
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // Restore the original item reference since the drag was canceled
                originalSlot.currentItem = draggedItem;
            }
        }
        else
        {
            Debug.Log("=== INVALID DROP - RETURNING TO ORIGINAL ===");
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if (originalSlot != null)
            {
                originalSlot.currentItem = gameObject;
            }
        }

        Debug.Log("=== END DRAG FINISHED ===");
    }
}