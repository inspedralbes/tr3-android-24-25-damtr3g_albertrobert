using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Assigna el teu SpriteRenderer des de l'Inspector
    private Color originalColor; // Per desar el color original
    private float interval = 1f; // Interval de temps en segons

    void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("No s'ha assignat un SpriteRenderer.");
            return;
        }

        // Desa el color original del SpriteRenderer
        originalColor = spriteRenderer.color;

        // Inicia la corrutina per canviar el color
        StartCoroutine(ChangeColorOverTime());
    }

    System.Collections.IEnumerator ChangeColorOverTime()
    {
        while (true) // Bucle infinit
        {
            yield return new WaitForSeconds(interval); // Espera 1 segon

            // Redueix els components G i B del color
            Color currentColor = spriteRenderer.color;
            currentColor.g = Mathf.Max(0, currentColor.g - 2f / 255f); // Redueix G en 2 unitats (normalitzat)
            currentColor.b = Mathf.Max(0, currentColor.b - 2f / 255f); // Redueix B en 2 unitats (normalitzat)

            // Assigna el nou color al SpriteRenderer
            spriteRenderer.color = currentColor;

            // Si G i B arriben a 0, pots aturar el bucle o reiniciar el color
            if (currentColor.g <= 0 && currentColor.b <= 0)
            {
                Debug.Log("Els components G i B han arribat a 0.");
                break; // Atura el bucle
            }
        }
    }

    // Opcional: Reiniciar el color a l'original
    public void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }
}