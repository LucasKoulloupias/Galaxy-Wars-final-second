using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setupFaction : MonoBehaviour
{
    public GameObject menu;



    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("faction") == "human" || PlayerPrefs.HasKey("faction") == false)
        {
            PlayerPrefs.SetString("faction", "human");
            showhumans();
        }
        else if (PlayerPrefs.GetString("faction") == "greek")
        {
            showgreek();
        }
    }

    void showhumans()
    {
        foreach(Transform child in transform.Find("SpaceStations"))
        {
            if (child.name.Contains("human"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        foreach (Transform child in transform)
        {
            if (child.name.Contains("fighter-human"))
            {
                child.gameObject.SetActive(true);
            }
        }

        Camera.main.GetComponent<player>().makearrays();
    }

    void showgreek()
    {
        foreach (Transform child in transform.Find("SpaceStations"))
        {
            if (child.name.Contains("greek"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        foreach (Transform child in transform)
        {
            if (child.name.Contains("fighter-greek"))
            {
                child.gameObject.SetActive(true);
            }
        }

        Camera.main.GetComponent<player>().makearrays();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeInHierarchy)
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(true);
            }
        }
    }
}
