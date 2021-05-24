using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public float myhealth;
    public float regenhealth;
    public float maxHealth;
    public bool shieldDown;

    public GameObject healthbarUI;
    public Slider slidey;
    public Slider regenslidey;

    private void Start()
    {
        myhealth = maxHealth;
        regenhealth = maxHealth;
        slidey.value = CalculateHealth();
        if (gameObject.layer == 8 || gameObject.layer == 9 || gameObject.layer == 10 || gameObject.layer == 15)
        {
            regenslidey.value = CalculateRegen();
            StartCoroutine(regen());
        }
        else
        {
            regenhealth = 0;
        }
    }

    private void Update()
    {
        if (gameObject.layer == 8 || gameObject.layer == 9 || gameObject.layer == 10 || gameObject.layer == 15)
        {
            regenslidey.value = CalculateRegen();
        }
        slidey.value = CalculateHealth();

        if (myhealth < maxHealth)
        {
            healthbarUI.SetActive(true);
        }

        if (myhealth <= 0)
        {
            if (gameObject.layer == 15)
            {
                Debug.Log("my health is " + myhealth + " so we win love from " + name);
                Camera.main.GetComponent<player>().Win();
            }
            else if (gameObject.layer == 10)
            {
                Camera.main.GetComponent<player>().Lose();
            }
            Destroy(gameObject);
        }

        if (myhealth > maxHealth)
        {
            myhealth = maxHealth;
        }
        if (regenhealth > maxHealth)
        {
            regenhealth = maxHealth;
        }
    }

    float CalculateHealth()
    {
        return myhealth / maxHealth;
    }

    float CalculateRegen()
    {
        return regenhealth / maxHealth;
    }

    public void takeDamage(float damage)
    {
        if (regenhealth > 0)
        {
            regenhealth -= damage;
        }
        else
        {
            myhealth -= damage;
        }

        if (regenhealth <= 0)
        {
            regenhealth = 0;
        }
    }

    IEnumerator regen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (regenhealth < maxHealth)
            {
                regenhealth += 2;
            }
            if (shieldDown == true)
            {
                break;
            }
        }
    }
}
