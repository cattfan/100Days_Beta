using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trashbin : MonoBehaviour, IInteractable
{
    public bool isChecked {get; private set; } // Flag to check if the trashbin is interactable

    public string TrashbinName { get; private set; } // Name of the trashbin

    public GameObject itemPrefab; // Prefab of the item that can be spawned from the trashbin

    public Sprite CheckedBin; // Sprite to show when the trashbin is checked
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrashbinName ??= Global_Helper.GenerateUniqueID(gameObject); // Generate a unique ID for the trashbin if not already set
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
        SetChecked(true); // Set the trashbin as checked
        // Optionally, instantiate the item prefab if it exists
        if (itemPrefab != null)
        {
            GameObject DroppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }
    }
    private void SetChecked(bool value)
    {
        isChecked = value;

        if (isChecked && CheckedBin != null)
        {
            GetComponent<SpriteRenderer>().sprite = CheckedBin;
            Debug.Log("Trashbin sprite changed!");
        }
    }
}
