using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    Image healthBarFill;
    
    [SerializeField]
    Image CannonBarFill;

    [SerializeField]
    Text ammoText;

    [SerializeField] private Text ammoTotal; 

    [SerializeField] private Text cashText;

    [SerializeField]
    GameObject scoreboard;

    
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
        SetAmmoAmount(weaponManager.bullets, weaponManager.BulletsTotal);
        SetCashAmount(player.money);
        SetCannnonAmount(playercannon.Getpst());

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }



    void SetHealthAmount (float _amount)
    {
        healthBarFill.fillAmount = _amount;
    }

    void SetAmmoAmount (int _amount, int _amountTot)
    {
        ammoText.text = _amount.ToString();
        ammoTotal.text = _amountTot.ToString();
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
