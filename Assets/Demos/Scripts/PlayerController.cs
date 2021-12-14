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
        controls.Demo.Movement.performed += ctx => inputVector = ctx.ReadValue<Vector2>().normalized;
        controls.Enable();
    }

    private void Update()
    {
        rb.velocity = ((canMove) ? inputVector : Vector2.zero) * speed;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}