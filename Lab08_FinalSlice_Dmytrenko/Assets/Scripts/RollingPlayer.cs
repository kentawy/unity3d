using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public sealed class RollingPlayer : MonoBehaviour
{
[SerializeField, Min(0f)] private float acceleration = 24f;
[SerializeField, Min(0f)] private float maxSpeed = 8f;

private Rigidbody body;
private Vector2 moveInput;
private Vector3 spawnPoint;
private int score;

private void Awake()
{
body = GetComponent<Rigidbody>();
}

private void Start()
{
spawnPoint = body.position;
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
float z = (keyboard.wKey.isPressed ? 1f : 0f)
- (keyboard.sKey.isPressed ? 1f : 0f);
moveInput = new Vector2(x, z).normalized;
}

private void FixedUpdate()
{
Vector3 force = new Vector3(moveInput.x, 0f, moveInput.y);
body.AddForce(force * acceleration, ForceMode.Acceleration);

Vector3 horizontal = new Vector3(
body.linearVelocity.x, 0f, body.linearVelocity.z);
if (horizontal.magnitude <= maxSpeed) return;

horizontal = horizontal.normalized * maxSpeed;
body.linearVelocity = new Vector3(
horizontal.x, body.linearVelocity.y, horizontal.z);
}

private void OnTriggerEnter(Collider other)
{
if (other.CompareTag("Pickup"))
{
score++;
Debug.Log($"Зібрано: {score}");
Destroy(other.gameObject);
}
else if (other.CompareTag("Respawn"))
{
ResetPlayer();
}
else if (other.CompareTag("SpeedBoost"))
{
    acceleration += 15f;
    maxSpeed += 5f;
    GetComponent<Renderer>().material.color = Color.cyan; 
    Debug.Log("SpeedBoost активовано!"); 
    Destroy(other.gameObject);
}
}

private void ResetPlayer()
{
body.position = spawnPoint;
body.linearVelocity = Vector3.zero;
body.angularVelocity = Vector3.zero;
}
}