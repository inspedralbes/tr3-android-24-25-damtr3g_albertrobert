using UnityEngine;
using UnityEngine.UIElements;

public class MementoMori : MonoBehaviour
{
    public string prefabTag = "Ground";
    public UIDocument uiDocument;

    void Start()
    {
        if (uiDocument != null)
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(prefabTag))
        {
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0;
            ShowGameOverUI();
        }
    }

    void ShowGameOverUI()
    {
        if (uiDocument != null)
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}