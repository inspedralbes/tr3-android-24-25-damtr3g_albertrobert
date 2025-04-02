using UnityEngine;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private ClientWebSocket websocket;
    private CancellationTokenSource cts;
    private readonly Uri serverUri = new Uri("ws://localhost:4005/ws");
    private readonly int receiveBufferSize = 1024;

    // Agrega esta clase arriba de PlayerConfig
    [System.Serializable]
    private class WebSocketMessage
    {
        public string type;
        public PlayerConfig data;
    }

    [System.Serializable]
    private class PlayerConfig
    {
        public float speed;
        public float jumpForce;
    }

    async void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cts = new CancellationTokenSource();
        
        await InitializeWebSocket();
        _ = ReceiveMessagesAsync();
    }

    async Task InitializeWebSocket()
    {
        try
        {
            websocket = new ClientWebSocket();
            await websocket.ConnectAsync(serverUri, cts.Token);
            Debug.Log("Conexión WebSocket establecida");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error de conexión: {ex.Message}");
        }
    }

    async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[receiveBufferSize];
        
        try
        {
            while (websocket.State == WebSocketState.Open && !cts.IsCancellationRequested)
            {
                var result = await websocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), 
                    cts.Token
                );

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    ProcessReceivedMessage(buffer, result.Count);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error recibiendo mensajes: {ex.Message}");
        }
    }

    // Modifica el método ProcessReceivedMessage
    void ProcessReceivedMessage(byte[] buffer, int count)
    {
        try
        {
            string json = Encoding.UTF8.GetString(buffer, 0, count);
            WebSocketMessage message = JsonUtility.FromJson<WebSocketMessage>(json);
            
            if (message != null && message.type == "playerConfig")
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() => 
                {
                    ApplyNewConfiguration(message.data);
                });
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error procesando mensaje: {ex.Message}");
        }
    }

    void ApplyNewConfiguration(PlayerConfig config)
    {
        Debug.Log(config.speed);
        Debug.Log(config.jumpForce);
        speed = Mathf.Clamp(config.speed, 3f, 100f);
        jumpForce = Mathf.Clamp(config.jumpForce, 5f, 1000f);
        Debug.Log($"Configuración actualizada: Velocidad={speed} | Salto={jumpForce}");
    }

    void Update()
    {
        // Movimiento horizontal
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        
        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) isGrounded = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) isGrounded = false;
    }

    async void OnDestroy()
    {
        cts?.Cancel();
        
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            try
            {
                await websocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Cerrando conexión",
                    CancellationToken.None
                );
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error cerrando conexión: {ex.Message}");
            }
        }
    }
}

// Clase auxiliar para ejecutar en el hilo principal
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private readonly Queue<Action> actionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject("MainThreadDispatcher");
                instance = container.AddComponent<UnityMainThreadDispatcher>();
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue()?.Invoke();
            }
        }
    }
}