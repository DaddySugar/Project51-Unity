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
	public GameObject impactEffectBlood;
	public float fireRate;
	private float nextTimeToFire = 0f;
	private bool hasFinishedReloading = true;
	private bool reloadInterrupted = true;
    public GameObject GunShot;
    public GameObject Reload;
	private float timeToNextReload = 0f;
	private float temp = 2f;
	private Player idk;
    public GameObject OutofAmmo;
    bool caca = true;

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
		Weapon.BulletsTotal = Weapon.maxBulletsTotal;
        idk = GetComponent<Player>();
	}

	void Update()
	{
        if (Weapon.bullets == 0 && Weapon.BulletsTotal == 0 && caca)
        {
            GameObject outofammo = Instantiate(OutofAmmo, this.transform.position, this.transform.rotation) as GameObject;
            caca = false;
        }
        if (Weapon.BulletsTotal != 0 && !caca)
        {
            caca = true;
        }
        if (!_animation.isPlaying && !reloadInterrupted)//reload successful
		{
			if (Weapon.maxBullets - Weapon.bullets >= Weapon.BulletsTotal)//not enough ammo to fill entirely clip
			{
                Weapon.bullets = Weapon.bullets + Weapon.BulletsTotal;
                Weapon.BulletsTotal = 0;
            }
            else
			{
				Weapon.BulletsTotal = Weapon.BulletsTotal - (Weapon.maxBullets - Weapon.bullets);
				Weapon.bullets = Weapon.maxBullets;
            }

            reloadInterrupted = true;
		}
		if (Weapon.bullets == 0 && !_animation.isPlaying && !hasFinishedReloading)
		{
			if (Weapon.maxBullets - Weapon.bullets >= Weapon.BulletsTotal)//not enough ammo to fill entirely clip
			{
				Weapon.bullets = Weapon.bullets + Weapon.BulletsTotal;
                Weapon.BulletsTotal = 0;
            }
            else
			{
				Weapon.BulletsTotal = Weapon.BulletsTotal - (Weapon.maxBullets - Weapon.bullets);
                Weapon.bullets = Weapon.maxBullets;
            }
            hasFinishedReloading = true;
		}
		else if (Weapon.bullets == 0 && !_animation.isPlaying && Weapon.BulletsTotal != 0 && !PauseMenu.GameIsPaused)
		{
			_animation.Play("reload");
            GameObject reload = Instantiate(Reload, this.transform.position, this.transform.rotation) as GameObject;
            hasFinishedReloading = false;

		}
		else if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && Weapon.bullets != 0 && !PauseMenu.GameIsPaused)
		{
            GameObject gunshot = Instantiate(GunShot, this.transform.position, this.transform.rotation) as GameObject;
            if (_animation.IsPlaying("reload"))
			{
				Destroy(GameObject.FindWithTag("Sound"));
				reloadInterrupted = true;
			}
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
			_animation.Play("fire");
		}
		else if (Input.GetKey(KeyCode.R) && Time.time > timeToNextReload && Weapon.BulletsTotal != 0 && !PauseMenu.GameIsPaused)//Reload, do not put the bullets in the gun yet, wait to see if the animation was interrupted
		{
			_animation.Play("reload");
			GameObject reload = Instantiate(Reload, this.transform.position, this.transform.rotation) as GameObject;
            reloadInterrupted = false;
			timeToNextReload = Time.time + temp;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameObject go = GameObject.FindWithTag("Sound");
            if (go != null && PauseMenu.GameIsPaused)
			{
				go.GetComponent<AudioSource>().Pause();
			}
			else if (go != null)
			{
				go.GetComponent<AudioSource>().Play();
			}

		}
		if ((!_animation.isPlaying || _animation.IsPlaying("Idle2")) && !gameObject.GetComponent<PlayerController>().isFalling)
		{
			if (gameObject.GetComponent<PlayerController>().isRunning)
			{
				_animation.Play("RunGunAnim");
			}
			else if (gameObject.GetComponent<PlayerController>().isSprinting)
			{
				_animation.Play("SprintGunAnim");
			}
			else
			{
				_animation.Play("Idle2");
				
			}
		}
		

	}

	public void jump()
	{
		if (!_animation.IsPlaying("reload"))
		{
			_animation.Stop();
			weaponPrefab.transform.localPosition = new Vector3(0,0,0);
		}
		
	}

	[Client]
	void Shoot()
	{
		muzzleFlash.Play();
		Weapon.bullets -= 1;
		RaycastHit _hit;
		GameObject impact = impactEffect;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, Weapon.range, mask ))
		{
			//we hit smth 
			//Debug.Log("we ghit " + _hit.collider.name);
			
			if (_hit.collider.tag == PLAYER_TAG)
			{
				//CmdPlayerShot(_hit.collider.name, Weapon.damage); it bugs when you shoot another player
				Debug.Log(transform.name + " shoot ");
			}

			else if (_hit.collider.tag == "Alien")
			{
				string uIdentity = _hit.transform.name;
				if (_hit.collider.gameObject.GetComponent<Alien_Health>().health - Weapon.damage<= 0)
				{
					gameObject.GetComponent<Player>().money += gameObject.GetComponent<Player>().moneyRewardedByKill;
					gameObject.GetComponent<Player>().CmdUpKills();
				}
				CmdTellServerWhichZombieWasShot(uIdentity, Weapon.damage);
				//Debug.Log("One");
				impact = impactEffectBlood;
			}
			
			GameObject impactGO = Instantiate(impact, _hit.point, Quaternion.LookRotation(_hit.normal));
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
	
	[Command]
	void CmdTellServerWhichZombieWasShot (string uniqueID, int dmg)
	{
		//Debug.Log("kinda working shooting aliens ");
		GameObject go = GameObject.Find(uniqueID);
		go.GetComponent<Alien_Health>().RpcDeductHealth(dmg);
	}
	
}
