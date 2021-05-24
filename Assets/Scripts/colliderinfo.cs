using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderinfo : MonoBehaviour
{
    public List<GameObject> objectsincollider = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        objectsincollider.Add(other.gameObject);

        if (transform.parent.gameObject.layer == 15 && other.gameObject.layer == 11)
        {
            Debug.Log("bullet it eneymss system");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsincollider.Remove(other.gameObject);
    }

    private void Update()
    {
        GameObject[] temp = objectsincollider.ToArray();

        foreach (GameObject obj in temp)
        {
            if (objectsincollider.Contains(obj) && obj == null)
            {
                objectsincollider.Remove(obj);
            }
        }
    }
}
