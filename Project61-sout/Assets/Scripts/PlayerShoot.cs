using UnityEngine.Networking;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{

	private const string PLAYER_TAG = "Player";
	public PlayerWeapon Weapon;
	[SerializeField] private Camera cam;
	[SerializeField] private GameObject weaponPrefab;
	[SerializeField] private LayerMask mask;
	public ParticleSystem muzzleFlash;
	private Animation _animation;
	public GameObject impactEffect;
	public float fireRate;
	private float nextTimeToFire = 0f;
	private bool hasFinishedReloading = true;
	private bool reloadInterrupted = true;

	void Start()
	{
		if (cam == null)
		{
		Debug.LogError("PlayerShoot: No camera");
			this.enabled = false;
		}
		_animation = weaponPrefab.GetComponent<Animation>();
		fireRate = Weapon.fireRate;
		Weapon.bullets = Weapon.maxBullets;
	}

	void Update()
	{
		if (!_animation.isPlaying && !reloadInterrupted)//reload successful
		{
			Weapon.bullets = Weapon.maxBullets;
			reloadInterrupted = true;
		}
		if (Weapon.bullets == 0 && !_animation.isPlaying && !hasFinishedReloading)
		{
			Weapon.bullets = Weapon.maxBullets;
			hasFinishedReloading = true;
		}
		else if (Weapon.bullets == 0 && !_animation.isPlaying)
		{
			_animation.Play("reload");
			hasFinishedReloading = false;

		}
		else if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && Weapon.bullets != 0)
		{
			if (_animation.IsPlaying("reload"))
			{
				reloadInterrupted = true;

			}
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
			_animation.Play("fire");
		}
		else if (Input.GetKey(KeyCode.R))//Reload, do not put the bullets in the gun yet, wait to see if the animation was interrupted
		{
			_animation.Play("reload");
			reloadInterrupted = false;
		}
		

	}

	[Client]
	void Shoot()
	{
		muzzleFlash.Play();
		Weapon.bullets -= 1;
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, Weapon.range, mask ))
		{
			//we hit smth 
			//Debug.Log("we ghit " + _hit.collider.name);
			
			if (_hit.collider.tag == PLAYER_TAG)
			{
				CmdPlayerShot(_hit.collider.name, Weapon.damage);
				Debug.Log(transform.name + " shoot ");
			}
			GameObject impactGO = Instantiate(impactEffect, _hit.point, Quaternion.LookRotation(_hit.normal));
			Destroy(impactGO, 2f);
		}
		
	}
	
	[Command]
	void CmdPlayerShot (string _PlayerID, int damage)
	{
		Debug.Log(_PlayerID + " has been shot.");
		Player _player = GameManager.GetPlayer(_PlayerID);
		_player.RpcTakeDamage(damage);

	}
}
