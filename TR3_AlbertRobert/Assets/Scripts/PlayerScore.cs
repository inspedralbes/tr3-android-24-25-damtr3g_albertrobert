using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScore : MonoBehaviour
{
    public UIDocument uiDocument; // Assigna el teu UIDocument des de l'Inspector
    private Label scoreLabel; // Referència al Label "ScorePoints"
    private int score = 0; // Variable per emmagatzemar la puntuació

    void Start()
    {
        // Obtenim la referència al Label "ScorePoints"
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            scoreLabel = root.Q<Label>("ScorePoints");

            if (scoreLabel == null)
            {
                Debug.LogError("No s'ha trobat el Label 'ScorePoints'.");
            }
            else
            {
                // Inicialitza el Label amb la puntuació actual
                UpdateScoreLabel();
            }
        }
        else
        {
            Debug.LogError("No s'ha assignat un UIDocument.");
        }
    }

    // Mètode per incrementar la puntuació
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreLabel(); // Actualitza el Label quan la puntuació canvia
    }

    // Mètode per actualitzar el text del Label
    private void UpdateScoreLabel()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = $"{score}";
        }
    }
}