using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class spacestation : MonoBehaviour
{
    public float enemyrange = 5;
    public bool visible;
    bool showing = true;

    public colliderinfo[] fireareas;

    public GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (colliderinfo c in fireareas)
        {
            foreach (GameObject thing in c.objectsincollider)
            {
                if (gameObject.layer == 10)
                {
                    if (thing != null && thing.layer == 9)
                    {
                        if (thing.GetComponent<enemy>().bigunit == true)
                        {
                            List<Image> hitpoints = thing.transform.Find("Canvas").GetComponentsInChildren<Image>().ToList();
                            Image[] temp = hitpoints.ToArray();
                            foreach (Image hitpint in temp)
                            {
                                if (hitpoints.Contains(hitpint))
                                {
                                    if (hitpint.GetComponent<hitpoint>().health <= 0 || hitpint == null)
                                    {
                                        hitpoints.Remove(hitpint);
                                    }

                                    if (c.objectsincollider.Contains(hitpint.GetComponent<hitpoint>().hitbox.gameObject) == false)
                                    {
                                        hitpoints.Remove(hitpint);
                                    }
                                }
                                else
                                {
                                    hitpoints.Remove(hitpint);
                                }
                            }
                            if (hitpoints.Count > 0)
                            {
                                Image smallestim = hitpoints[0];
                                float smallest = Vector3.Distance(transform.position, hitpoints[0].transform.position);
                                for (int i = 0; i < hitpoints.Count; i++)
                                {
                                    if (smallest > Vector3.Distance(transform.position, hitpoints[i].transform.position))
                                    {
                                        smallest = Vector3.Distance(transform.position, hitpoints[i].transform.position);
                                        smallestim = hitpoints[i];
                                    }

                                }

                                GetComponent<shooting>().Shoot(smallestim.gameObject, c.gameObject);
                            }
                            else
                            {
                                GetComponent<shooting>().Shoot(thing, c.gameObject);
                            }
                        }
                        else
                        {
                            GetComponent<shooting>().Shoot(thing, c.gameObject);
                        }
                    }
                    else if (thing != null && thing.layer == 15)
                    {
                        GetComponent<shooting>().Shoot(thing, c.gameObject);
                    }
                }
                else if (gameObject.layer == 15)
                {
                    if (thing != null && thing.layer == 8)
                    {
                        if (thing.GetComponent<unit>().bigunit == true)
                        {
                            List<Image> hitpoints = thing.transform.Find("Canvas").GetComponentsInChildren<Image>().ToList();
                            Image[] temp = hitpoints.ToArray();
                            foreach (Image hitpint in temp)
                            {
                                if (hitpoints.Contains(hitpint))
                                {
                                    if (hitpint.GetComponent<hitpoint>().health <= 0 || hitpint == null)
                                    {
                                        hitpoints.Remove(hitpint);
                                    }

                                    if (c.objectsincollider.Contains(hitpint.GetComponent<hitpoint>().hitbox.gameObject) == false)
                                    {
                                        hitpoints.Remove(hitpint);
                                    }
                                }
                                else
                                {
                                    hitpoints.Remove(hitpint);
                                }
                            }
                            if (hitpoints.Count > 0)
                            {
                                Image smallestim = hitpoints[0];
                                float smallest = Vector3.Distance(transform.position, hitpoints[0].transform.position);
                                for (int i = 0; i < hitpoints.Count; i++)
                                {
                                    if (smallest > Vector3.Distance(transform.position, hitpoints[i].transform.position))
                                    {
                                        smallest = Vector3.Distance(transform.position, hitpoints[i].transform.position);
                                        smallestim = hitpoints[i];
                                    }

                                }

                                GetComponent<shooting>().Shoot(smallestim.gameObject, c.gameObject);
                            }
                            else
                            {
                                GetComponent<shooting>().Shoot(thing, c.gameObject);
                            }
                        }
                        else
                        {
                            GetComponent<shooting>().Shoot(thing, c.gameObject);
                        }
                    }
                    else if (thing != null && thing.layer == 10)
                    {
                        GetComponent<shooting>().Shoot(thing, c.gameObject);
                    }
                }
            }
        }

        if (gameObject.layer == 15)
        {
            unit[] temp2 = Camera.main.GetComponent<player>().units.ToArray();
            foreach (unit unit in temp2)
            {
                if (Camera.main.GetComponent<player>().units.Contains(unit) && unit != null)
                {
                    if (Vector3.Distance(transform.position, unit.transform.position) < Camera.main.GetComponent<fogofwar>().m_radius + 5)
                    {
                        visible = true;
                    }
                    else
                    {
                        visible = false;
                    }
                }
                else if (Camera.main.GetComponent<player>().units.Contains(unit) && unit == null)
                {
                    Camera.main.GetComponent<player>().units.Remove(unit);
                }
            }

            if (visible == true && showing == false)
            {
                show();
            }
            else if (visible == false && showing == true)
            {
                hide();
            }
        }
        else if (gameObject.layer == 10)
        {
            enemy[] temp2 = Camera.main.GetComponent<player>().enemys.ToArray();
            foreach (enemy enemy in temp2)
            {
                if (Camera.main.GetComponent<player>().enemys.Contains(enemy) && enemy != null)
                {
                    if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) < enemyrange && Camera.main.GetComponent<player>().testenemy == true)
                    {
                        if (enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                        {
                            enemy.GetComponent<NavMeshAgent>().isStopped = true;
                        }
                    }
                }
            }
        }
    }

    void show()
    {
        GetComponent<MeshRenderer>().enabled = true;
        transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color =
            new Color(transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.b, 1f);

        transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 1f);

        transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.b, 1f);

        transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 1f);

        Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
        foreach (Image butt in butts)
        {
            butt.color = new Color(butt.color.r, butt.color.g, butt.color.b, 1f);
        }
        showing = true;
    }

    void hide()
    {
        GetComponent<MeshRenderer>().enabled = false;
        transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color =
            new Color(transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.b, 0f);

        transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 0f);

        transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.b, 0f);

        transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 0f);

        Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
        foreach (Image butt in butts)
        {
            butt.color = new Color(butt.color.r, butt.color.g, butt.color.b, 0f);
        }
        showing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            bool candamage = true;
            Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
            foreach (Image butt in butts)
            {
                if (butt.GetComponent<hitpoint>().health > 0)
                {
                    candamage = false;
                }
            }


            if (gameObject.layer == 15 && candamage == true)
            {
                if (other.gameObject.GetComponent<bullet>().enemybullet == false)
                {
                    GetComponent<health>().takeDamage(other.gameObject.GetComponent<bullet>().damage);
                    Destroy(other.gameObject);
                }
            }
            else if (gameObject.layer == 10 && candamage == true)
            {
                if (other.gameObject.GetComponent<bullet>().enemybullet == true)
                {
                    GetComponent<health>().takeDamage(other.gameObject.GetComponent<bullet>().damage);
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
