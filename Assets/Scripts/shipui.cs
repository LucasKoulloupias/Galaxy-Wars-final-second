using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipui : MonoBehaviour
{
    public void doit()
    {
        Camera.main.GetComponent<player>().shipView(Camera.main.GetComponent<unitselection>().selectedunits);
    }
}
