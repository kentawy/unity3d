using UnityEngine;

public sealed class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0f, 8f, -10f);
    [SerializeField, Min(0f)] private float smoothness = 8f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        float t = 1f - Mathf.Exp(-smoothness * Time.deltaTime);
        transform.position = Vector3.Lerp(
            transform.position, targetPosition, t);
        transform.LookAt(target);
    }
}