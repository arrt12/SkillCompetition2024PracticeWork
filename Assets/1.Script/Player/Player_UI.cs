using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Image HpBar;
    [SerializeField] Image FuelBar;
    private float fueltime = 0.1f;
    void Start()
    {
        
    }

    void Update()
    {
        Hp();
        Fuel();
    }
    void Hp()
    {
        HpBar.fillAmount = Player.player.hp / 100;
        if (Player.player.hp <= 0)
        {
            Player.player.hp = 0;
        }
    }
    void Fuel()
    {
        FuelBar.fillAmount = Player.player.fuel / 1000;
        fueltime -= Time.deltaTime;
        if (fueltime <= 0)
        {
            Player.player.fuel--;
            fueltime = 0.1f;
        }
    }
}
