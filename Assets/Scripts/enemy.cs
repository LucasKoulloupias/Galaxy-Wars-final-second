using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    public float enemyrange;
    public List<colliderinfo> fireareas = new List<colliderinfo>();
    public GameObject target;
    NavMeshAgent agnet;

    bool testunit = true;
    public bool bigunit;
    public bool visible;
    public bool wantsToCapture;
    bool showing = true;
    public bool attacking;

    public int price;

    // Start is called before the first frame update
    void Start()
    {
        agnet = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(colliderinfo c in fireareas)
        {
            foreach (GameObject thing in c.objectsincollider)
            {
                if (thing != null && thing.layer == 8)
                {
                    attacking = true;
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

                        if (Vector3.Distance(transform.position, thing.transform.position) > enemyrange && testunit == true)
                        {
                            StopAllCoroutines();
                            StartCoroutine(coolenemytest());
                            if (agnet.isActiveAndEnabled == true)
                            {
                                agnet.isStopped = false;
                                agnet.SetDestination(thing.transform.position);

                            }

                        }
                    }
                    else
                    {
                        GetComponent<shooting>().Shoot(thing, c.gameObject);
                        if (Vector3.Distance(transform.position, thing.transform.position) > enemyrange && testunit == true)
                        {
                            StopAllCoroutines();
                            StartCoroutine(coolenemytest());
                            agnet.isStopped = false;
                            agnet.SetDestination(thing.transform.position);
                        }
                    }
                }
                else if(thing != null && thing.layer == 10)
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

                    if (Vector3.Distance(transform.position, thing.transform.position) > enemyrange && testunit == true)
                    {
                        StopAllCoroutines();
                        StartCoroutine(coolenemytest());
                        agnet.isStopped = false;
                        agnet.SetDestination(thing.transform.position);
                    }
                }
                else
                {
                    attacking = false;
                }
            }
        }

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

        if (agnet.hasPath == true)
        {
            var lookPos = agnet.destination - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
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

    void show()
    {
        showing = true;
        GetComponent<MeshRenderer>().enabled = true;
        transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color =
            new Color(transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.b, 1f);
        
        transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 1f);


        transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.b, 1f);

        transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 1f);
    
        if (bigunit == true)
        {
            Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
            foreach(Image butt in butts)
            {
                butt.color = new Color(butt.color.r, butt.color.g, butt.color.b, 1f);
            }
        }
    }

    void hide()
    {
        showing = false;
        GetComponent<MeshRenderer>().enabled = false;
        transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color =
            new Color(transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Background").GetComponent<Image>().color.b, 0f);

        transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("health").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 0f);


        transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Background").GetComponent<Image>().color.b, 0f);

        transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color =
                new Color(transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.r, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.g, transform.Find("healthbar").Find("regen").Find("Fill Area").Find("Fill").GetComponent<Image>().color.b, 0f);

        if (bigunit == true)
        {
            Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
            foreach (Image butt in butts)
            {
                butt.color = new Color(butt.color.r, butt.color.g, butt.color.b, 0f);
            }
        }
    }

    public void captureAsteroid(GameObject asterodi)
    {
        agnet.SetDestination(asterodi.transform.position);
        agnet.isStopped = true;
    }

    IEnumerator coolenemytest()
    {
        testunit = false;
        yield return new WaitForSeconds(0.5f);
        testunit = true;
    }

    public IEnumerator movetowardsanddontstop(Vector3 postion)
    {
        NavMeshAgent agnet = GetComponent<NavMeshAgent>();
        agnet.isStopped = false;
        agnet.ResetPath();
        while (true)
        {
            if (agnet.isActiveAndEnabled == false)
            {
                break;
            }
            agnet.SetDestination(postion);
            if (agnet.isStopped == true || (Math.Round(agnet.transform.position.x, 1) == Math.Round(postion.x, 1) && Math.Round(agnet.transform.position.z, 1) == Math.Round(postion.z, 1)) || attacking == true)
            {
                break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            bool candamage = true;
            if (bigunit == true)
            {
                Image[] butts = transform.Find("Canvas").GetComponentsInChildren<Image>();
                foreach (Image butt in butts)
                {
                    if (butt.GetComponent<hitpoint>().health > 0)
                    {
                        candamage = false;
                    }
                }
            }

            if (other.gameObject.GetComponent<bullet>().enemybullet == false && candamage == true)
            {
                GetComponent<health>().myhealth -= other.gameObject.GetComponent<bullet>().damage;
            }
        }
    }
}
