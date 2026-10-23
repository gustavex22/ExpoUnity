using UnityEngine;

/// <summary>
/// Cámara que sigue suavemente al jugador desde una vista superior / tercera persona.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo")]
    [Tooltip("Transform del jugador a seguir.")]
    public Transform target;

    [Header("Configuración de cámara")]
    [Tooltip("Desplazamiento relativo respecto al jugador (ejemplo: 0, 8, -5).")]
    public Vector3 offset = new Vector3(0f, 8f, -5f);

    [Tooltip("Suavidad del seguimiento (mayor valor = cámara más rápida).")]
    public float smoothSpeed = 5f;

    [Tooltip("Si está activado, la cámara mira siempre al jugador.")]
    public bool lookAtTarget = true;

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada basada en el jugador más el offset
        Vector3 desiredPosition = target.position + offset;

        // Interpolación suave para movimiento fluido (no sacude al jugador)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
