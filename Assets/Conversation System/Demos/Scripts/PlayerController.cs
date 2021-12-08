using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool canMove = true;
    [SerializeField]
    private float speed;

    private Controls controls;
    private Rigidbody2D rb;

    private Vector2 inputVector;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        controls = new Controls();
        controls.Demo.Movement.performed += ctx => inputVector = (canMove) ? ctx.ReadValue<Vector2>().normalized : Vector2.zero;
        controls.Enable();
    }

    private void Update()
    {
        rb.velocity = inputVector * speed;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}