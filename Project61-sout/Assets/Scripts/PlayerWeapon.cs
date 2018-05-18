using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

	public string name = "HandGun";

	public int damage = 10;
	public float range = 100f;

	public float fireRate = 15f;

	public int maxBullets = 20;
	[HideInInspector]
	public int bullets = 20;

	public int maxBulletsTotal = 200;
	[HideInInspector] public int BulletsTotal = 200;

	//public float reloadTime = 1f;

	//public GameObject graphics;

	/*public PlayerWeapon ()
	{
		bullets = maxBullets;
	}
	*/

}