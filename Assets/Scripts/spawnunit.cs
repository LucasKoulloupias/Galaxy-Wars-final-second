using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnunit : MonoBehaviour
{
    public void spawn(GameObject unit)
    {
        if (gameObject.layer == 15)
        {
            if (transform.parent.parent.GetComponent<money>().mymoney >= unit.GetComponent<enemy>().price)
            {
                spacestation ss = GetComponent<spacestation>();

                GameObject spawned = Instantiate(unit, ss.transform.Find("shipspawnpoint").transform.position, ss.transform.Find("shipspawnpoint").transform.rotation);
                Camera.main.GetComponent<player>().enemys.Add(spawned.GetComponent<enemy>());
                transform.parent.parent.GetComponent<money>().subtractMoney(unit.GetComponent<enemy>().price);
            }
        }
        else
        {
            if (Camera.main.GetComponent<money>().mymoney >= unit.GetComponent<unit>().price)
            {
                spacestation ss = GetComponent<spacestation>();

                GameObject spawned = Instantiate(unit, ss.transform.Find("shipspawnpoint").transform.position, ss.transform.Find("shipspawnpoint").transform.rotation);
                Camera.main.GetComponent<player>().units.Add(spawned.GetComponent<unit>());
                Camera.main.GetComponent<fogofwar>().m_players.Add(spawned.transform);
                Camera.main.GetComponent<money>().subtractMoney(unit.GetComponent<unit>().price);
            }
        }


    }
}
