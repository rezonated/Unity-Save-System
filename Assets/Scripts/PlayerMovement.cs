using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _rigidbody.velocity =
            new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * playerSpeed;
    }
}
