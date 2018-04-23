
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float jumpForce = 5f;

    private bool isGrounded = true;
    private Vector3 _velocity = new Vector3(0f, 0f, 0f);
    private Animator anim;
    private PlayerMotor motor;
    // Use this for initialization
	
    void Start ()
    {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
    }
	
    void OnCollisionStay()
    {
        isGrounded = true;
    }
	
    // Update is called once per frame
    void Update ()
    {
        
        
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");


        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _velocity = (_moveHorizontal + _moveVertical).normalized * sprintSpeed;

        }
        else
        {
            _velocity = (_moveHorizontal + _moveVertical).normalized * speed;
        }
        anim.SetFloat("Speed", _velocity.magnitude);
		
        motor.Move(_velocity);
		
        
        //calculate rotations

        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);
		
        // Calculate camera rotations
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;
		

        motor.RotateCamera(_cameraRotationX);

        // apply force for jump
		
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            motor.Jump(jumpForce);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            motor.Reset();
        }

    }
}