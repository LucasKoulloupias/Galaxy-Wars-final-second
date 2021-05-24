using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidui : MonoBehaviour
{
    public asteroid whatasteroid;

    public void buildmine()
    {
        if (whatasteroid != null && Camera.main.GetComponent<money>().mymoney >= 50)
        {
            Camera.main.GetComponent<player>().mines += 1;
            whatasteroid.hasmine = true;
            transform.Find("Panel").Find("Button").gameObject.SetActive(false);
            Camera.main.GetComponent<money>().subtractMoney(50);
        }
        else
        {
            Debug.Log("no asteroid or money");
        }
    }
}
