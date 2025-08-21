using UnityEngine;
using UnityEngine.InputSystem; // Ensure you have the Input System package installed

public class InteractionDetector : MonoBehaviour
{
    private IInteractable Interactrange = null;
    public GameObject interactIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactIcon.SetActive(false); // Ensure the interaction icon is hidden at the start
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check if there is an interactable object in range
        if (context.performed && Interactrange != null)
        {
            // Call the Interact method on the interactable object
            Interactrange?.Interact();
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object implements IInteractable
        Interactrange = other.GetComponent<IInteractable>();
        if(other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            Interactrange = interactable;
            interactIcon.SetActive(true);
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
}
