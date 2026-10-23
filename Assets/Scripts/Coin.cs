using UnityEngine;

/// <summary>
/// Comportamiento para los objetos recolectables del laberinto.
/// Rota continuamente sobre su eje Y, tiene color oro y emite un efecto
/// de partículas simple al ser recogida por el jugador.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Header("Configuración de rotación")]
    [Tooltip("Velocidad de rotación en grados por segundo alrededor del eje Y.")]
    public float rotationSpeed = 90f;

    [Header("Efecto de recolección")]
    [Tooltip("Reproducir sonido al recolectar si hay un AudioSource en el objeto.")]
    public bool playSoundOnCollect = true;

    void Awake()
    {
        // Aplicar color oro por código
        MeshRenderer coinRenderer = GetComponent<MeshRenderer>();
        if (coinRenderer != null)
        {
            ColorTheme.ApplyColor(coinRenderer, ColorTheme.CoinColor);
        }
    }

    void Update()
    {
        // Rotación continua sobre el eje Y propio del objeto
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar colisión solo con el jugador (tag "Player")
        if (other.CompareTag("Player"))
        {
            // Efecto de partícula simple: esfera que crece y desaparece
            SpawnCollectEffect();

            // Sonido opcional
            if (playSoundOnCollect)
            {
                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null && audio.clip != null)
                {
                    audio.Play();
                    // Esperar un instante antes de destruir para que se oiga el sonido
                    Destroy(gameObject, audio.clip.length);
                    return;
                }
            }

            // Notificar al GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectCoin();
            }

            // Destruirse automáticamente al ser recolectada
            Destroy(gameObject);
        }
    }

    private void SpawnCollectEffect()
    {
        // Crear una esfera pequeña como partícula de recolección
        GameObject effect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        effect.transform.position = transform.position + Vector3.up * 0.5f;
        effect.transform.localScale = Vector3.one * 0.3f;

        // Color igual a la moneda
        MeshRenderer effectRenderer = effect.GetComponent<MeshRenderer>();
        if (effectRenderer != null)
        {
            ColorTheme.ApplyColor(effectRenderer, ColorTheme.CoinColor);
        }

        // Eliminar collider del efecto para que no interfiera con la física
        Collider effectCollider = effect.GetComponent<Collider>();
        if (effectCollider != null)
        {
            Destroy(effectCollider);
        }

        // Animación simple: crecer y desaparecer
        Destroy(effect, 0.4f);
    }
}
