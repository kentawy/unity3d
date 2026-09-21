using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PrototypePlayer : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 6f;
    public GameObject door; // Перешкода, яка зникне після виконання завдання

    private Rigidbody rb;
    private bool isGrounded;
    private int itemsCollected = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // Рух WASD
        float moveX = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
        float moveZ = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * speed;
        // В Unity 6.5 використовується linearVelocity замість velocity
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // Стрибок
        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Перевірка приземлення
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Збір предметів
        if (other.CompareTag("Collectible"))
        {
            itemsCollected++;
            Destroy(other.gameObject);
            Debug.Log("Зібрано предметів: " + itemsCollected + " / 3");

            // Виконання завдання: відкриття дверей
            if (itemsCollected >= 3 && door != null)
            {
                Destroy(door);
                Debug.Log("Завдання виконано! Шлях відкрито.");
            }
        }
    }
}