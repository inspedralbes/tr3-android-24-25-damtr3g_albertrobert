using UnityEngine;

public class MementoMori : MonoBehaviour
{
    public string prefabTag = "Ground";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(prefabTag))
        {
            Destroy(collision.gameObject);
        }
    }
}
