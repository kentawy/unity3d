using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public sealed class SlicePlayer : MonoBehaviour

{

[SerializeField, Min(0f)] private float acceleration = 12f;

[SerializeField, Min(0.1f)] private float maxSpeed = 6f;

private Rigidbody body;

private Vector3 input;

private Vector3 spawn;

private void Awake()

{

body = GetComponent<Rigidbody>();

spawn = body.position;

}

private void Update()

{

Keyboard keys = Keyboard.current;

if (keys == null) { input = Vector3.zero; return; }

float x = (keys.dKey.isPressed ? 1f : 0f)

- (keys.aKey.isPressed ? 1f : 0f);

float z = (keys.wKey.isPressed ? 1f : 0f)

- (keys.sKey.isPressed ? 1f : 0f);

input = Vector3.ClampMagnitude(new Vector3(x, 0f, z), 1f);

}

private void FixedUpdate()

{

body.AddForce(input * acceleration, ForceMode.Acceleration);

Vector3 velocity = body.linearVelocity;

Vector3 planar = Vector3.ClampMagnitude(

new Vector3(velocity.x, 0f, velocity.z), maxSpeed);

body.linearVelocity = new Vector3(planar.x, velocity.y, planar.z);

if (body.position.y < -3f)

{

body.position = spawn;

body.linearVelocity = Vector3.zero;

body.angularVelocity = Vector3.zero;

}

}

}

