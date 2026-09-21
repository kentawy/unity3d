using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Обертання джерела світла навколо осі X створює зміну дня і ночі
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}