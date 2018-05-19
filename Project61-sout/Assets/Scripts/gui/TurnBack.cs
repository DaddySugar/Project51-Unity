using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TurnBack : MonoBehaviour {

	[Space(10)]
	[Header("Toggle for the gui on off")]
	public bool GuiOn;


	[Space(10)]
	[Header("The text to Display on Trigger")]
	[Tooltip("To edit the look of the text Go to Assets > Create > GUIskin. Add the new Guiskin to the Custom Skin proptery. If you select the GUIskin in your project tab you can now adjust the Label section to change this text")]
	[SerializeField] public string Text;


	public GameObject Buyobject;
	[SerializeField]
	Text buyText;
	private const string pressText = "Press E to buy ";


	private void Start()
	{
		Buyobject.SetActive(false);
		
	}

	// Update is called once per frame
	void OnTriggerEnter (Collider player)
	{
		if (player.gameObject.tag == "Player")
		{
			Buyobject.SetActive(true);
			buyText.text = pressText + Text;
			StartCoroutine("WaitForSec");
		}
	}
	IEnumerator WaitForSec()
	{
		yield return new WaitForSeconds(3);
		Buyobject.SetActive(false);
	}

}
