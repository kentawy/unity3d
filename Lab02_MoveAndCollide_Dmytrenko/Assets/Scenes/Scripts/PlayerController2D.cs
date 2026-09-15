using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class PlayerController2D : MonoBehaviour
{
[SerializeField, Min(0f)] private float moveSpeed = 6f;

private Rigidbody2D body;
private Vector2 moveInput;
private Vector2 spawnPoint;
private int score;

private void Awake()
{
body = GetComponent<Rigidbody2D>();
}

private void Start()
{
spawnPoint = body.position;
Debug.Log("PlayerController2D готовий");
}

private void Update()
{
Keyboard keyboard = Keyboard.current;
if (keyboard == null)
{
moveInput = Vector2.zero;
return;
}

float x = (keyboard.dKey.isPressed ? 1f : 0f)
- (keyboard.aKey.isPressed ? 1f : 0f);
float y = (keyboard.wKey.isPressed ? 1f : 0f)
- (keyboard.sKey.isPressed ? 1f : 0f);
moveInput = new Vector2(x, y).normalized;
}

private void FixedUpdate()
{
body.linearVelocity = moveInput * moveSpeed;
}

private void OnCollisionEnter2D(Collision2D collision)
{
Debug.Log($"Зіткнення з {collision.gameObject.name}");
if (!collision.collider.CompareTag("Hazard")) return;

body.position = spawnPoint;
body.linearVelocity = Vector2.zero;
}

private void OnTriggerEnter2D(Collider2D other)
{
if (other.CompareTag("Pickup")) 
        {
            score++;
            Debug.Log($"Зібрано: {score}");
            Destroy(other.gameObject);
            return; 
        }

        if (other.CompareTag("Water")) 
        {
            moveSpeed = 2f;  
            Debug.Log("Ми потрапили в калюжу!");
        }

}



}