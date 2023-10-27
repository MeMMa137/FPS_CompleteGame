using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController instance; //essendo solo 1 il player da poter colpire, possiamo usare "static"

    public int maxHealth, currentHealth; //"maxHealth" non rappresenta per forza il valore iniziale della vita, ma a quanto può arrivare al massimo in gioco con cure ecc... 

    public float invincibleLength = 1f; //durata dell'effetto invincibilità subito dopo essere stato colpito
    private float invincCounter; //conto alla rovescia dell'effetto invincibilità subito dopo essere stato colpito

    private void Awake()
    {
        instance = this; 
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth; //imposta la grafica del massimo dello slider al massimo
        UIController.instance.healthSlider.value = currentHealth;//imposta la grafica dello slider al massimo
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth; //imposta la scritta della vita iniziale
    }

    
    void Update()
    {
        if(invincCounter > 0) //se il tempo di invincibilità non è finito, lo abbassa di 1 (60fps)
        {
            invincCounter -= Time.deltaTime;
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        if(invincCounter <= 0 && !GameManager.instance.levelEnding) //potremo prendere danno solo se il contatore di invincibilità sarà finito
        {
            AudioManager.instance.PlaySXF(7);
            currentHealth -= damageAmount; //toglie vita

            UIController.instance.ShowDamage();

            if(currentHealth <= 0) //se muore
            {
               gameObject.SetActive(false); //freeza la scena

                currentHealth = 0; //per evitare che venga mostrato a video un numero inferiore a 0

                GameManager.instance.PlayerDied();

                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySXF(6);
                AudioManager.instance.StopSFX(7);
            }

            invincCounter = invincibleLength; //resetta il conto alla rovescia x invincibilità

            UIController.instance.healthSlider.value = currentHealth;//cambia la grafica dello slider con la vita attuale
            UIController.instance.healthText.text = "VITA: " + currentHealth + "/" + maxHealth; //imposta la scritta della vita attuale

        }

        

    }

    public void HealPlayer(int healAmount) //funzione per recuperare vita
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;//cambia la grafica dello slider con la vita attuale
        UIController.instance.healthText.text = "VITA: " + currentHealth + "/" + maxHealth; //imposta la scritta della vita attuale
    }

}
