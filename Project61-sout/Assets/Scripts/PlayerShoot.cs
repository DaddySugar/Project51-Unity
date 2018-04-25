﻿using UnityEngine.Networking;
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

	void Start()
	{
		if (cam == null)
		{
		Debug.LogError("PlayerShoot: No camera");
			this.enabled = false;
		}
		_animation = weaponPrefab.GetComponent<Animation>();
		
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (_animation.IsPlaying("reload"))
			{
				Debug.Log("The reload animation was playing");// the animation of reload has been interrupted, put here a boolean to say that it has been interrupted
			}
			Shoot();
			_animation.Play("fire");
		}
		else if (Input.GetKey(KeyCode.T))//Reload, do not put the bullets in the gun yet, wait to see if the animation was interrupted
		{
			_animation.Play("reload");
			
		}
		

	}

	[Client]
	void Shoot()
	{
		muzzleFlash.Play();
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
