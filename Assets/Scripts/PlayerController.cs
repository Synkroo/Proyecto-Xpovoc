using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region VARIABLES
    [Header("Ajustes Movimiento")]
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;
    #endregion


    #region UNITY_METHODS
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion


    #region CUSTOM_METHODS
    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A - D
        float z = Input.GetAxisRaw("Vertical");   // W - S

        // Vector final normalizado para evitar diagonales más rápidas
        inputDirection = new Vector3(x, 0f, z).normalized;
    }
    private void Move()
    {
        rb.MovePosition(transform.position + inputDirection * speed * Time.fixedDeltaTime);
    }
    #endregion
}