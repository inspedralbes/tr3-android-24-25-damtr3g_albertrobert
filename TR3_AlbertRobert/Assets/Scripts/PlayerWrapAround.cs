using UnityEngine;

public class PlayerWrapAround : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float offset = 0.1f; // Pequeño margen para suavizar la transición
    [SerializeField] private float transitionDuration = 0.5f; // Duración del efecto visual
    
    private Camera mainCamera;
    private float screenWidth;
    private bool isWrapping = false;
    private Vector3 newPosition;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // ← Añade esta inicialización
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CalculateScreenBounds();
    }

    void Update()
    {
        if (!isWrapping)
        {
            CheckScreenEdges();
        }
        else
        {
            SmoothTransition();
        }
    }

    void CalculateScreenBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        screenWidth = mainCamera.orthographicSize * screenAspect;
    }

    void CheckScreenEdges()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        
        // Salida por la izquierda
        if (viewportPos.x < -offset)
        {
            StartWrap(Vector3.right * (screenWidth * 2));
        }
        // Salida por la derecha
        else if (viewportPos.x > 1 + offset)
        {
            StartWrap(Vector3.left * (screenWidth * 2));
        }
    }

    void StartWrap(Vector3 direction)
    {
        isWrapping = true;
        newPosition = transform.position + direction;
        
        // Opcional: Efecto visual durante la transición
        StartCoroutine(FadeEffect());
    }

    void SmoothTransition()
    {
        rb.gravityScale = 0; // Desactivar gravedad
        
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10f);
        
        // Finalizar transición cuando esté cerca
        if (Vector3.Distance(transform.position, newPosition) < 1f)
        {
            isWrapping = false;
            rb.gravityScale = 2; // Restaurar gravedad original
            spriteRenderer.enabled = true;
        }
    }

    System.Collections.IEnumerator FadeEffect()
    {
        // Efecto de desvanecimiento
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / transitionDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
        spriteRenderer.color = Color.white;
    }
}
