using UnityEngine;
using TMPro; // Обязательно подключаем библиотеку для UI текста

public class NPCInteraction : MonoBehaviour
{
    [Header("Настройки UI")]
    public TMP_Text dialogText; // Сюда перетянем наш текст с экрана
    public string npcMessage = "Привіт! Збери 3 колекційні предмети, щоб відкрити двері.";

    void Start()
    {
        // Прячем текст при старте игры, чтобы он не висел на экране просто так
        if (dialogText != null)
        {
            dialogText.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Если подошел игрок, меняем текст и включаем его
        if (other.CompareTag("Player") && dialogText != null)
        {
            dialogText.text = npcMessage;
            dialogText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Когда игрок отходит, выключаем текст
        if (other.CompareTag("Player") && dialogText != null)
        {
            dialogText.gameObject.SetActive(false);
        }
    }
}