using UnityEngine.Networking;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{

	private const string PLAYER_TAG = "Player";
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

	[Client]
	void Shoot()
	{
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, Weapon.range, mask ))
		{
			if (_hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot(_hit.collider.name, Weapon.damage);
			}
		}
		
	}
	
	[Command]
	void CmdPlayerShot (string _PlayerID, int damage)
	{
		Debug.Log(_PlayerID + " has been shot.");
		Player _player = GameManager.GetPlayer(_PlayerID);
		_player.TakeDamage(damage);

	}
}
