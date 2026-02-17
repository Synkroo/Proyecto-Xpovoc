using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes Movimiento")]
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;
    public bool canMove = true;

    private GameObject elely;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        elely = transform.Find("Elely")?.gameObject;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!canMove) return;
        ReadInput();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        Move();
    }

    private void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector3(x, 0f, z).normalized;
    }

    private void Move()
    {
        if (rb != null)
            rb.MovePosition(transform.position + inputDirection * speed * Time.fixedDeltaTime);
    }

    public void StopMovement()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetCombatMode(bool inCombat)
    {
        if (elely != null)
        {
            elely.SetActive(!inCombat);
        }
        canMove = !inCombat;

        if (inCombat && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.CancelDialogue();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCombatMode(false);

        if (PlayerSpawnHolder.nextSpawn != null)
        {
            transform.position = PlayerSpawnHolder.nextSpawn.position;
            transform.rotation = PlayerSpawnHolder.nextSpawn.rotation;
            PlayerSpawnHolder.nextSpawn = null;
        }
    }
}
