using UnityEngine;

public class IniciPlataform : MonoBehaviour
{

    void Start()
    {
        // Inicia la cuenta regresiva para eliminar el componente
        Invoke("DestroyThisComponent", 30f);
    }

    void DestroyThisComponent()
    {
        Destroy(this); // Elimina este componente del GameObject
    }
}
