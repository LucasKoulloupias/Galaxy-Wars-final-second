using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class hitpoint : MonoBehaviour
{
    public GameObject firearea;
    public colliderinfo hitbox;
    public float health = 100;

    int maxheakth;

    public bool breaksShields;
    public int breaksEngineBydivision = 0;


    // Start is called before the first frame update
    void Start()
    {
        maxheakth = (int)Convert.ToInt64(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (hitbox != null)
        {
            foreach (GameObject thing in hitbox.objectsincollider)
            {
                if (thing != null && thing.layer == 11 && health > 0)
                {
                    if (transform.parent.parent.gameObject.layer == 8 || transform.parent.parent.gameObject.layer == 10)
                    {
                        if (thing.GetComponent<bullet>().enemybullet == true)
                        {
                            health -= thing.GetComponent<bullet>().damage;
                            transform.parent.parent.GetComponent<health>().takeDamage(thing.GetComponent<bullet>().damage);
                            Destroy(thing);
                        }
                    }
                    else if (transform.parent.parent.gameObject.layer == 9 || transform.parent.parent.gameObject.layer == 15)
                    {
                        if (thing.GetComponent<bullet>().enemybullet == false)
                        {
                            health -= thing.GetComponent<bullet>().damage;
                            transform.parent.parent.GetComponent<health>().takeDamage(thing.GetComponent<bullet>().damage);
                            Destroy(thing);
                        }
                    }
                }
            }
        }


        if (health <= maxheakth / 2)
        {
            GetComponent<Image>().color = Color.yellow;
        }

        if (health <= 0)
        {
            if (breaksShields == true)
            {
                transform.parent.parent.GetComponent<health>().regenhealth = 0;
                transform.parent.parent.GetComponent<health>().shieldDown = true;
            }
            if (breaksEngineBydivision > 0)
            {
                transform.parent.parent.GetComponent<NavMeshAgent>().speed -= transform.parent.parent.GetComponent<NavMeshAgent>().speed / breaksEngineBydivision;
            }

            GetComponent<Image>().color = Color.red;
            health = 0;
            enabled = false;
            if (hitbox != null)
            {
                hitbox.gameObject.SetActive(false);
            }
            if (firearea != null)
            {
                firearea.gameObject.SetActive(false);
                if (transform.parent.parent.gameObject.layer == 8)
                {
                    transform.parent.parent.GetComponent<unit>().fireareas.Remove(firearea.GetComponent<colliderinfo>());
                }
                else if (transform.parent.parent.gameObject.layer == 9)
                {
                    transform.parent.parent.GetComponent<enemy>().fireareas.Remove(firearea.GetComponent<colliderinfo>());
                }
            }
        }
    }
}
