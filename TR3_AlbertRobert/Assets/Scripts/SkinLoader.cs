using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; // Agregar esta directiva

public class SkinLoader : MonoBehaviour
{
    public string imageUrl = "http://localhost:4000/image/heart.jpg"; // URL de la imagen
    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del personaje
    public Vector2 desiredSize = new Vector2(1f, 1f); // Tamaño deseado del sprite

    void Start()
    {
        StartCoroutine(LoadSkin());
    }

    IEnumerator LoadSkin()
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Rect rect = new Rect(0, 0, 440, 482);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite newSprite = Sprite.Create(texture, rect, pivot);

                spriteRenderer.sprite = newSprite;

                // **Llamamos a la función para crear el PolygonCollider2D**
                GenerateCollider();
            }
            else
            {
                Debug.LogError("Error al cargar la imagen: " + request.error);
            }
        }
    }

    void GenerateCollider()
    {
        // **Eliminar collider anterior si existe**
        CircleCollider2D existingCollider = GetComponent<CircleCollider2D>();
        if (existingCollider != null)
        {
            Destroy(existingCollider);
        }

        // **Crear un nuevo CircleCollider2D**
        CircleCollider2D newCollider = gameObject.AddComponent<CircleCollider2D>();

        // **Ajustar el radio del CircleCollider2D al tamaño del sprite**
        if (spriteRenderer.sprite != null)
        {
            // Calcular el radio basado en el tamaño del sprite
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;
            newCollider.radius = Mathf.Max(spriteWidth, spriteHeight) / 2f;

            // Ajustar el offset si es necesario
            newCollider.offset = spriteRenderer.sprite.bounds.center;
        }
    }
}
