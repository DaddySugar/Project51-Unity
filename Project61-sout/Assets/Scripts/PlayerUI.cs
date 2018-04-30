using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    Image healthBarFill;

    [SerializeField]
    Text ammoText;


    private Player player;
   // private PlayerController controller;
    private PlayerWeapon weaponManager;

    public void SetPlayer (Player _player)
    {
        player = _player;
       // controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<PlayerShoot>().Weapon;
    }

    //void Start ()
    //{
       // PauseMenu.IsOn = false;
    //}

    void Update ()
    {
       // SetFuelAmount (controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthpct());
        SetAmmoAmount(weaponManager.bullets);


    }



    void SetHealthAmount (float _amount)
    {
        healthBarFill.fillAmount = _amount;
    }

    void SetAmmoAmount (int _amount)
    {
        ammoText.text = _amount.ToString();
    }

}
