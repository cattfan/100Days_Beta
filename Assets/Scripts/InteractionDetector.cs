using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable Interactrange = null;
    public GameObject interactIcon;

    void Start()
    {
        interactIcon.SetActive(false); // Ensure the interaction icon is hidden at the start
    }

    void Update()
    {
        // Liên tục kiểm tra trạng thái của object đang trong range
        UpdateInteractionIcon();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check if there is an interactable object in range
        if (context.performed && Interactrange != null && Interactrange.CanInteract())
        {
            // Call the Interact method on the interactable object
            Interactrange.Interact();

            // Cập nhật icon ngay sau khi interact
            UpdateInteractionIcon();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object implements IInteractable
        if (other.TryGetComponent(out IInteractable interactable))
        {
            Interactrange = interactable;
            UpdateInteractionIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited the trigger is the same interactable
        if (Interactrange != null && other.GetComponent<IInteractable>() == Interactrange)
        {
            Interactrange = null;
            interactIcon.SetActive(false); // Hide the interaction icon when exiting
        }
    }

    // Phương thức để cập nhật trạng thái icon
    private void UpdateInteractionIcon()
    {
        if (Interactrange != null)
        {
            // Hiển thị icon chỉ khi object có thể tương tác
            bool canInteract = Interactrange.CanInteract();
            interactIcon.SetActive(canInteract);
        }
        else
        {
            // Không có object nào trong range
            interactIcon.SetActive(false);
        }
    }
}