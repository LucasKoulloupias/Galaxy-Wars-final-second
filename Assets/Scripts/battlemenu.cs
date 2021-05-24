using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battlemenu : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetString("faction", "human");
    }

    public void human()
    {
        PlayerPrefs.SetString("faction", "human");
    }

    public void greek()
    {
        PlayerPrefs.SetString("faction", "greek");
    }
}
