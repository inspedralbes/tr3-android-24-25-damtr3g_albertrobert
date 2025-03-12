using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    public float fallSpeed = 5f; // Velocidad ajustable

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}
