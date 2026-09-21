using UnityEngine;

[RequireComponent(typeof(Collider))]

public sealed class SessionPickup : MonoBehaviour

{

[SerializeField] private GameSession session;

private bool collected;

private Collider sensor;

private void Awake()

{

sensor = GetComponent<Collider>();

if (session == null)

Debug.LogError("Assign GameSession to this pickup instance.", this);

if (!sensor.isTrigger)

Debug.LogError("Pickup Collider must have Is Trigger enabled.", this);

}

private void OnTriggerEnter(Collider other)

{

if (collected || session == null) return;

Rigidbody body = other.attachedRigidbody;

if (body == null || !body.CompareTag("Player")) return;

if (!session.Collect()) return;

collected = true;

sensor.enabled = false;

gameObject.SetActive(false);

}

}

