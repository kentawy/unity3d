using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PrototypePlayer : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 6f;
    public float mouseSensitivity = 10f;
    public GameObject door; 

    [Header("Effects & Audio")]
    public GameObject collectVfxPrefab; // Префаб Particle System
    public AudioClip jumpSound;
    public AudioClip collectSound;

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isGrounded;
    private int itemsCollected = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (kb == null) return;

        if (mouse != null)
        {
            float mouseX = mouse.delta.x.ReadValue();
            transform.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);
        }

        float moveX = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
        float moveZ = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);

        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized * speed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            audioSource.PlayOneShot(jumpSound); // Відтворення звуку стрибка
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            itemsCollected++;
            
            // Відтворення звуку та VFX
            audioSource.PlayOneShot(collectSound);
            if (collectVfxPrefab != null)
            {
                Instantiate(collectVfxPrefab, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject);
            Debug.Log("Зібрано: " + itemsCollected);

            if (itemsCollected >= 3 && door != null) Destroy(door);
        }
    }
}