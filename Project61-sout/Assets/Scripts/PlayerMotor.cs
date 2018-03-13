using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
	[SerializeField] private Camera cam;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 jump = new Vector3(0.0f, 2.0f, 0.0f);
	private Vector3 originalPosition;

	[SerializeField] private float cameraRotationLimit = 85f;
	
	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		originalPosition = rb.position;
	}

	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}

	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	public void Jump(float jumpForce)
	{
		rb.AddForce(jump * jumpForce, ForceMode.Impulse);
	}

	public void Reset()
	{
		
		rb.MovePosition(originalPosition);
	}
	
	private void FixedUpdate()
	{
		PerformMovement();
		PerformRotation();

	}

	void PerformMovement()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if (cam != null)
		{
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

		}
	}
	

}
