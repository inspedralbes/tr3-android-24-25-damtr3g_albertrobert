using UnityEngine;
using UnityEngine.UIElements;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    
    private Label timeLabel;
    private float currentTime;
    public bool isGameRunning = true;

    private void Start()
    {
        // Obtener referencia al Label
        timeLabel = uiDocument.rootVisualElement.Q<Label>("Time");
        
        // Inicializar tiempo
        currentTime = 0f;
        UpdateTimeDisplay();
    }

    private void Update()
    {
        if (isGameRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    private void UpdateTimeDisplay()
    {
        // Formatear tiempo en minutos:segundos
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(currentTime);
        timeLabel.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    // Método para reiniciar el cronómetro (opcional)
    public void ResetTimer()
    {
        currentTime = 0f;
        isGameRunning = true;
        UpdateTimeDisplay();
    }
}