using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class unit : MonoBehaviour
{
    public GameObject selectionvisual;
    public GameObject targetenemy;
    public List<colliderinfo> fireareas = new List<colliderinfo>();
    public float Distance_;
    public int price = 10;

    public bool selected;
    public bool lookattarget;
    public bool bigunit;
    public bool wantsToCapture;
    NavMeshAgent agnet;

    // Start is called before the first frame update
    void Start()
    {
        agnet = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (colliderinfo c in fireareas)
        {
            foreach (GameObject thing in c.objectsincollider)
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
            }
        }

        if (agnet.hasPath == true)
        {
            var lookPos = agnet.destination - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
        }
    }

    public void ToggleSelectionVisual(bool selected)
    {
        selectionvisual.transform.GetChild(0).gameObject.SetActive(selected);
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


            if (other.gameObject.GetComponent<bullet>().enemybullet == true && candamage == true)
            {
                GetComponent<health>().takeDamage(other.gameObject.GetComponent<bullet>().damage);
                Destroy(other.gameObject);
            }
        }
    }

    public void captureAsteroid(GameObject asterodi)
    {
        agnet.SetDestination(asterodi.transform.position);
        agnet.isStopped = true;
    }
}
