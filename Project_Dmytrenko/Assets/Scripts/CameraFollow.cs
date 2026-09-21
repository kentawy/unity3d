using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    // Налаштовуємо ідеальний ракурс: x=0 (по центру), y=3 (вище голови), z=-6 (за спиною)
    public Vector3 offset = new Vector3(0, 3, -6); 
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Множимо поворот гравця на offset, щоб камера завжди була за спиною
            Vector3 desiredPosition = target.position + target.rotation * offset;
            
            // Плавно переміщуємо камеру
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            // Камера завжди дивиться на гравця
            transform.LookAt(target);
        }
    }
}