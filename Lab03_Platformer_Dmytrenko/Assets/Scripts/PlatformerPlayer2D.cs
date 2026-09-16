using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlatformerPlayer2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 7f;
    [SerializeField, Min(0f)] private float jumpSpeed = 12f;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField, Min(0f)] private float groundRadius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    [Header("HUD")]
    [SerializeField] private TMP_Text scoreText;

    private Rigidbody2D body;
    private float horizontal;
    private bool jumpQueued;
    private Vector2 spawnPoint;
    private int score;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        spawnPoint = body.position;
        RefreshHud();
    }
    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        horizontal = (keyboard.dKey.isPressed ? 1f : 0f)
                   - (keyboard.aKey.isPressed ? 1f : 0f);
        jumpQueued |= keyboard.spaceKey.wasPressedThisFrame;
    }
    private void FixedUpdate()
    {
        Vector2 velocity = body.linearVelocity;
        velocity.x = horizontal * moveSpeed;

        if (jumpQueued && IsGrounded())
            velocity.y = jumpSpeed;

        body.linearVelocity = velocity;
        jumpQueued = false;
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position, groundRadius, groundLayer);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            score++;
            RefreshHud();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Respawn"))
        {
            body.position = spawnPoint;
            body.linearVelocity = Vector2.zero;
        }
        else if (other.CompareTag("Bonus"))
        {
            score += 5; 
            RefreshHud(); 
            GetComponent<SpriteRenderer>().color = Color.yellow; 
            Debug.Log("Супер-бонус зібрано!"); 
            Destroy(other.gameObject); 
        }
    }
    private void RefreshHud()
    {
        if (scoreText != null)
            scoreText.text = $"Кристали: {score}";
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}