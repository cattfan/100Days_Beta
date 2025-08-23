using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Audio Settings")]
    public AudioManagement audioManagement;    // Reference đến AudioManagement
    public float footstepInterval = 0.5f;      // Khoảng cách giữa các tiếng bước (giây)

    // Components
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 movement;

    // Sprite flip
    private SpriteRenderer spriteRenderer;

    // Animator params
    private Animator animator;
    private static readonly int HashMoveX = Animator.StringToHash("MoveX");
    private static readonly int HashMoveY = Animator.StringToHash("MoveY");
    private static readonly int HashIsMove = Animator.StringToHash("IsMoving");

    // Audio control
    private float footstepTimer = 0f;
    private bool isMoving = false;

    void Awake()
    {
        // Lấy components
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        // Tìm AudioManagement nếu chưa assign
        if (audioManagement == null)
        {
            audioManagement = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagement>();
            if (audioManagement == null)
            {
                Debug.LogError("PlayerMovement: Không tìm thấy AudioManagement! Hãy gắn tag 'Audio' cho GameObject AudioManagement.");
            }
        }

        // Warnings
        if (spriteRenderer == null)
            Debug.LogWarning("PlayerMovement: Không tìm thấy SpriteRenderer.");

        if (animator == null)
            Debug.LogWarning("PlayerMovement: Không tìm thấy Animator. Hãy gắn Animator Controller & tạo params MoveX/MoveY/IsMoving.");
    }

    void OnEnable() => moveAction.Enable();
    void OnDisable() => moveAction.Disable();

    void Update()
    {
        // Input
        movement = moveAction.ReadValue<Vector2>();
        if (movement.sqrMagnitude > 1f)
            movement = movement.normalized;

        // Kiểm tra trạng thái di chuyển
        isMoving = movement.sqrMagnitude > 0.0001f;

        // Cập nhật Animator
        UpdateAnimator();

        // Xử lý âm thanh bước chân
        HandleFootstepAudio();
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool(HashIsMove, isMoving);
            animator.SetFloat(HashMoveX, movement.x);
            animator.SetFloat(HashMoveY, movement.y);
        }
    }

    void HandleFootstepAudio()
    {
        if (isMoving)
        {
            // Giảm timer
            footstepTimer -= Time.deltaTime;

            // Khi timer hết, phát âm thanh bước chân
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval; // Reset timer
            }
        }
        else
        {
            // Dừng di chuyển, reset timer
            footstepTimer = 0f;
        }
    }

    void PlayFootstepSound()
    {
        if (audioManagement != null && audioManagement.Walking != null)
        {
            audioManagement.PlaySFX(audioManagement.Walking);
        }
        else
        {
            Debug.LogWarning("PlayerMovement: AudioManagement hoặc Walking sound chưa được thiết lập!");
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Method có thể gọi từ Animation Event (tùy chọn)
    public void OnFootstep()
    {
        PlayFootstepSound();
    }
}