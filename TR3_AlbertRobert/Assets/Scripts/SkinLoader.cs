using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SkinLoader : MonoBehaviour
{
    public string imageUrl = "http://localhost:4000/images/image/AmongUs.jpg"; // Nueva estructura de URL
    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del personaje
    public Vector2 desiredSize = new Vector2(1f, 1f); // Tamaño deseado del sprite
    public Sprite defaultSprite; // Sprite por defecto desde el inspector

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
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite newSprite = Sprite.Create(texture, rect, pivot, 100, 1, SpriteMeshType.Tight);

                ApplyNewSprite(newSprite);
            }
            else
            {
                Debug.LogError("Error al cargar la imagen: " + request.error);
                
                // // Cargar sprite por defecto desde Resources
                // Sprite amongUsSprite = Resources.Load<Sprite>("AmongUs");
                
                if(defaultSprite != null)
                {
                    ApplyNewSprite(defaultSprite);
                }
                else
                {
                    Debug.LogError("No se encontró el sprite por defecto 'AmongUs' en Resources");
                }
            }
        }
    }

    void ApplyNewSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
        GenerateCollider();
    }

    void GenerateCollider()
    {
        // Eliminar collider anterior si existe
        CircleCollider2D existingCollider = GetComponent<CircleCollider2D>();
        if (existingCollider != null)
        {
            Destroy(existingCollider);
        }

        // Crear nuevo collider
        CircleCollider2D newCollider = gameObject.AddComponent<CircleCollider2D>();

        if (spriteRenderer.sprite != null)
        {
            // Calcular radio basado en el sprite
            Bounds spriteBounds = spriteRenderer.sprite.bounds;
            newCollider.radius = Mathf.Max(spriteBounds.size.x, spriteBounds.size.y) / 2f;
            newCollider.offset = spriteBounds.center;

            // // Ajustar escala si es necesario
            // transform.localScale = new Vector3(
            //     desiredSize.x / spriteBounds.size.x,
            //     desiredSize.y / spriteBounds.size.y,
            //     1f
            // );
        }
    }
}