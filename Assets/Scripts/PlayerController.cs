using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controlador del jugador usando el nuevo Input System de Unity.
/// Mueve una esfera con Rigidbody respetando colisiones con paredes.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    [Tooltip("Velocidad de movimiento del jugador en unidades por segundo.")]
    public float moveSpeed = 5f;

    [Tooltip("Fuerza de salto.")]
    public float jumpForce = 5f;

    [Tooltip("Permitir que el jugador salte.")]
    public bool canJump = false;

    private Rigidbody rb;
    private Vector2 moveInput;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;

        // Aplicar color del jugador por código
        MeshRenderer playerRenderer = GetComponent<MeshRenderer>();
        if (playerRenderer != null)
        {
            ColorTheme.ApplyColor(playerRenderer, ColorTheme.PlayerColor);
        }

        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Move.Enable();
        inputActions.Player.Jump.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.Jump.Disable();
    }

    void Update()
    {
        // Leer entrada del Input System (WASD, flechas, gamepad, etc.)
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (canJump && inputActions.Player.Jump.triggered && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Mover el Rigidbody respetando la física de las paredes
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}
