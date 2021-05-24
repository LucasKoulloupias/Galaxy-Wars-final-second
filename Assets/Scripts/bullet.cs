using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Transform target;
    public bool enemybullet;

    public int damage = 10;
    public float speed = 70f;

    public void Seek(Transform _target, bool eb)
    {
        target = _target;
        enemybullet = eb;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distancethisframe = speed * Time.deltaTime;

        if (dir.magnitude <= distancethisframe)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distancethisframe, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}
