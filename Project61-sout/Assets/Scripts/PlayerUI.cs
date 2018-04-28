using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform healthBarFill; 

    private Player player;
    private PlayerController controller;
    
    
    public void SetPlayer (Player _player)
    {
        player = _player ;
        controller = player.GetComponent<PlayerController>();
    }


    void Update ()
    {
//        SetHealthAmount(player.GetHealthPct()); 
    }
    
    
    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
