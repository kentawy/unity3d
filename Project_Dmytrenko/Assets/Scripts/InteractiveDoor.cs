using UnityEngine;

public class InteractiveDoor : MonoBehaviour
{
    public float openAngle = 90f;
    public float smooth = 2f;
    
    private bool isOpen = false;
    private Quaternion defaultRot;
    private Quaternion openRot;

    void Start()
    {
        defaultRot = transform.rotation;
        openRot = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * smooth);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultRot, Time.deltaTime * smooth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isOpen = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isOpen = false;
    }
}