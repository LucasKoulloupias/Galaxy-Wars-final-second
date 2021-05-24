using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidbelt : MonoBehaviour
{
    public List<GameObject> thingsinbelt = new List<GameObject>();

    public List<GameObject> thingsmedamaging = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        GameObject[] temp = thingsinbelt.ToArray();
        foreach (GameObject thing in temp)
        {
            if (thing != null)
            {
                if (thing.layer == 8)
                {
                    if (thing.GetComponent<unit>().bigunit == true && thingsmedamaging.Contains(thing) == false)
                    {
                        thingsmedamaging.Add(thing);
                        StartCoroutine(damageunit(thing.GetComponent<unit>()));
                    }
                }
                if (thing.layer == 9)
                {
                    if (thing.GetComponent<enemy>().bigunit == true && thingsmedamaging.Contains(thing) == false)
                    {
                        thingsmedamaging.Add(thing);
                        StartCoroutine(damageenemy(thing.GetComponent<enemy>()));
                    }
                }
            }
            else
            {
                if (thingsinbelt.Contains(thing))
                {
                    thingsinbelt.Remove(thing);
                }
                if (thingsmedamaging.Contains(thing))
                {
                    thingsmedamaging.Remove(thing);
                }
            }
        }
    }

    IEnumerator damageunit(unit unit)
    {
        while (true)
        {
            if (unit != null)
            {
                if(thingsmedamaging.Contains(unit.gameObject) == false)
                {
                    break;
                }
                unit.GetComponent<health>().takeDamage(4);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator damageenemy(enemy unit)
    {
        while (true)
        {
            if (unit != null)
            {
                if (thingsmedamaging.Contains(unit.gameObject) == false)
                {
                    break;
                }
                unit.GetComponent<health>().takeDamage(4);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            thingsinbelt.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            if (thingsinbelt.Contains(other.gameObject))
            {
                thingsinbelt.Remove(other.gameObject);
            }

            if (thingsmedamaging.Contains(other.gameObject))
            {
                thingsmedamaging.Remove(other.gameObject);
            }
        }
    }
}
