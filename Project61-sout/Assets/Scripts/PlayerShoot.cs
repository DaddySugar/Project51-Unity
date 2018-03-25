using UnityEngine.Networking;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{

	public PlayerWeapon Weapon;
	[SerializeField] private Camera cam;

	[SerializeField] private LayerMask mask;

	void Start()
	{
		if (cam == null)
		{
		Debug.LogError("PlayerShoot: No camera");
			this.enabled = false;
		}
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	void Shoot()
	{
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, Weapon.range, mask ))
		{
			Debug.Log("we hit" + _hit.collider.name);
		}
		
	}
}
