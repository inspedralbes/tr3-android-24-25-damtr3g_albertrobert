using UnityEngine;

public class PuntosPorColision : MonoBehaviour
{
    // [SerializeField] private string tagObjetivo = "Objetivo"; // Tag del objeto que da puntos
    private bool yaSumado = false; // Controla si ya se sumó el punto

    // Referencia al script de puntuación del jugador
    private PlayerScore playerScore;

    void Start()
    {
        // Busca el script PlayerScore en el jugador (asumiendo que está en el mismo GameObject)
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
        
        if (playerScore == null)
        {
            Debug.LogError("PlayerScore no encontrado en el jugador.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger!");
        if (other.CompareTag("Player") && !yaSumado)
        {
            Debug.Log("TriggerIf!");
            SumarPunto();
        }
    }

    void SumarPunto()
    {
        yaSumado = true; // Marca como sumado
        playerScore.AddPoints(1); // Suma 1 punto
        
        // Opcional: Desactiva el collider para evitar futuras colisiones
        GetComponent<Collider2D>().enabled = false;
    }
}