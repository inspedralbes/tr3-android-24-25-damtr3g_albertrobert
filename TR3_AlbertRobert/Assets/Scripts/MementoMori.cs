using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class MementoMori : MonoBehaviour
{
    public string prefabTag = "Ground";
    public UIDocument uiDocument;

    // Variable per emmagatzemar la puntuació (ajusta com obtinguis el valor real)
    private int currentScore; 
    private GameTimer gameTimer;

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
            // Busca el script PlayerScore en el jugador (asumiendo que está en el mismo GameObject)
            gameTimer = GameObject.FindGameObjectWithTag("Time").GetComponent<GameTimer>();
            gameTimer.isGameRunning = false;
            Time.timeScale = 0;
            // Obtenir la puntuació actual (ajusta segons la teva implementació)
            currentScore = FindFirstObjectByType<PlayerScore>().score;
            ShowGameOverUI();
            StartCoroutine(SendScoreToServer(currentScore));
        }
    }

    IEnumerator SendScoreToServer(int score)
    {
        // Crear l'objecte amb les dades a enviar
        ScoreData scoreData = new ScoreData();
        scoreData.score = score;

        // Convertir a JSON
        string json = JsonUtility.ToJson(scoreData);
        
        // Configurar la petició POST
        using (UnityWebRequest webRequest = new UnityWebRequest("http://localhost:4000/scores/scores", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Enviar la petició i esperar resposta
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error en enviar puntuació: " + webRequest.error);
            }
            else
            {
                Debug.Log("Puntuació enviada correctament: " + webRequest.downloadHandler.text);
            }
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

// Classe auxiliar per serialitzar les dades
[System.Serializable]
public class ScoreData
{
    public int score;
}