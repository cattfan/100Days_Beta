using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 movement;

    // Sprite flip
    private SpriteRenderer spriteRenderer;

    // ✅ Animator params
    private Animator animator;
    private static readonly int HashMoveX = Animator.StringToHash("MoveX");
    private static readonly int HashMoveY = Animator.StringToHash("MoveY");
    private static readonly int HashIsMove = Animator.StringToHash("IsMoving");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("PlayerMovement: Không tìm thấy SpriteRenderer.");

        // ✅ Lấy Animator (trên chính Player hoặc child)
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("PlayerMovement: Không tìm thấy Animator. Hãy gắn Animator Controller & tạo params MoveX/MoveY/IsMoving.");
    }

    void OnEnable() => moveAction.Enable();
    void OnDisable() => moveAction.Disable();

    void Update()
    {
        // Input
        movement = moveAction.ReadValue<Vector2>();
        if (movement.sqrMagnitude > 1f) movement = movement.normalized;

        

        // ✅ Đẩy input vào Animator
        if (animator != null)
        {
            bool isMoving = movement.sqrMagnitude > 0.0001f;
            animator.SetBool(HashIsMove, isMoving);
            animator.SetFloat(HashMoveX, movement.x);
            animator.SetFloat(HashMoveY, movement.y);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
