using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform firepoint;
    public float reloadtime = 0.5f;
    public int damage = 10;

    bool canshoot = true;

    public void Shoot(GameObject target, GameObject firearea)
    {
        if (gameObject.layer == 8)
        {
            if (GetComponent<unit>().bigunit == true)
            {
                string firepointtolookfor = "firepoint-" + firearea.name.Split('-')[1];
                firepoint = transform.Find(firepointtolookfor);
            }
        }
        if (gameObject.layer == 9)
        {
            if (GetComponent<enemy>().bigunit == true)
            {
                string firepointtolookfor = "firepoint-" + firearea.name.Split('-')[1];
                firepoint = transform.Find(firepointtolookfor);
            }
        }
        if (gameObject.layer == 10 || gameObject.layer == 15)
        {
            string firepointtolookfor = "firepoint-" + firearea.name.Split('-')[1];
            firepoint = transform.Find(firepointtolookfor);
        }

        GameObject bulletGO = (GameObject)Instantiate(bullet, firepoint.position, firepoint.rotation);
        bullet bullete = bulletGO.GetComponent<bullet>();
        bullete.damage = damage;

        if (bullete != null && canshoot)
        {
            canshoot = false;
            if (gameObject.layer == 9 || gameObject.layer == 15)
            {
                bullete.Seek(target.transform, true);
            }
            else
            {
                bullete.Seek(target.transform, false);
            }
            StartCoroutine(shottimeout());
        }
    }

    IEnumerator shottimeout()
    {
        if (canshoot == false)
        {
            yield return new WaitForSeconds(reloadtime);
            canshoot = true;
        }
    }
}
