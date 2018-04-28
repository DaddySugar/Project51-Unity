
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    public float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpDelay = 15f;

    private bool isAbleToJump = true;
    private Vector3 _velocity = new Vector3(0f, 0f, 0f);
    private Animator anim;
    private PlayerMotor motor;
    private bool isFalling = false;

    private float nextTimeToJump = 0f;
    // Use this for initialization
	
    void Start ()
    {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
    }
	
    void OnCollisionStay()
    {
        if (Time.time >= nextTimeToJump)
        {
            isFalling = false;
        }
    }
	
    // Update is called once per frame
    void Update ()
    {
        
        if (Time.time >= nextTimeToJump)
        {
            isAbleToJump = true;
        }
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
		Debug.Log(_velocity.magnitude);
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
		
        if (Input.GetKeyDown(KeyCode.Space) && isAbleToJump && !isFalling)
        {
            Debug.Log("jump");
            motor.Jump(jumpForce);
            isAbleToJump = false;
            nextTimeToJump = Time.time + jumpDelay;
            isFalling = true;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            motor.Reset();
        }

    }
}