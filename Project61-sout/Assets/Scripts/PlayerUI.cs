using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    Image healthBarFill;
    
    [SerializeField]
    Image CannonBarFill;

    [SerializeField]
    Text ammoText;

    [SerializeField] private Text cashText;

    
    private Player player;

    private Cannon playercannon;
    // private PlayerController controller;
    private PlayerWeapon weaponManager;

    public void SetPlayer (Player _player)
    {
        player = _player;
       // controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<PlayerShoot>().Weapon;
        playercannon = FindObjectOfType<Cannon>();
    }

    void Update ()
    {
       // SetFuelAmount (controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthpct());
        SetAmmoAmount(weaponManager.bullets);
        SetCashAmount(player.money);
        SetCannnonAmount(playercannon.Getpst());

    }



    void SetHealthAmount (float _amount)
    {
        healthBarFill.fillAmount = _amount;
    }

    void SetAmmoAmount (int _amount)
    {
        ammoText.text = _amount.ToString();
    }
    void SetCashAmount (int _amount)
    {
        cashText.text = _amount.ToString();
    }
    
    void SetCannnonAmount(float _amount)
    {
        CannonBarFill.fillAmount = _amount;
    }
}
