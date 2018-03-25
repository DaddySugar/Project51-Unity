using UnityEngine.Networking;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{

	[SerializeField] private Camera cam;

	private void Start()
	{
		if (cam == null)
		{
		Debug.LogError("");
		}
	}
}
