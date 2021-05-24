using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ai : MonoBehaviour
{
    player player;
    money mon;
    int mines = 0;
    public List<asteroid> capturedAsteroids = new List<asteroid>();
    public GameObject randompoint;
    public GameObject patrolarea;
    GameObject justbought;

    public int minesNeededToAttack = 2;
    public int unitsNeededToAttack = 4;

    public float chanceToCaptureAsteroid = 100;
    public float captureAsteroidTimeout = 30;
    
    public float chanceToPatrol = 15;
    public float chanceToAttackEnemy = 20;
    public float patrolTimeout = 7;

    public float chanceToPurchase = 200;
    public float purchaseTimeout = 60;

    public float gameSpeedForTesting = 1;

    public GameObject[] spawnableUnits;

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.GetComponent<player>();
        mon = GetComponent<money>();
        StartCoroutine(CaptureAsteroidLoop());
        StartCoroutine(MakeMineLoop());
        StartCoroutine(PatrolLoop());
        StartCoroutine(PurchaseLoop());

        StartCoroutine(minemoney());

        justbought = spawnableUnits[0];
        Time.timeScale = gameSpeedForTesting;
    }

    IEnumerator CaptureAsteroidLoop()
    {
        while (true)
        {
            float thing = UnityEngine.Random.Range(1, chanceToCaptureAsteroid);
            yield return new WaitForSeconds(thing);
            captureAsteroid();
            yield return new WaitForSeconds(captureAsteroidTimeout);
        }
    }

    IEnumerator MakeMineLoop()
    {
        while (true)
        {
            float seonds = mines * 5;
            if (mines == 0)
            {
                seonds = 1;
            }
            float thing = UnityEngine.Random.Range(1, seonds);
            yield return new WaitForSeconds(thing);
            makeMine();
            yield return new WaitForSeconds(captureAsteroidTimeout / 2);
        }
    }

    IEnumerator PatrolLoop()
    {
        while (true)
        {

            if (mines < minesNeededToAttack && player.enemys.Count < unitsNeededToAttack)
            {
                float thing = UnityEngine.Random.Range(1, chanceToPatrol);
                yield return new WaitForSeconds(thing);
                patrol();
            }
            else
            {
                float thing = UnityEngine.Random.Range(1, chanceToAttackEnemy);
                yield return new WaitForSeconds(thing);
                moveTowardsEnemy();
            }
            yield return new WaitForSeconds(patrolTimeout);
        }
    }

    IEnumerator PurchaseLoop()
    {
        while (true)
        {
            float thing = UnityEngine.Random.Range(1, chanceToPurchase);
            yield return new WaitForSeconds(thing);
            purchase();
            yield return new WaitForSeconds(purchaseTimeout);
        }
    }


    IEnumerator minemoney()
    {
        while (true)
        {
            if (mines > 0)
            {
                mon.addMoney(10 * mines);
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return null;
            }
        }
    }

    void captureAsteroid()
    {
        Debug.Log("getting asteroid");
        List<asteroid> tempasteroids = player.asteroids;

        if (tempasteroids.Count > 0 && player.enemys.Count > 0)
        {
            asteroid smallesta = tempasteroids[0];
            float smallest = Vector3.Distance(player.enemyspacestations[0].transform.position, tempasteroids[0].transform.position);
            for (int i = 0; i < tempasteroids.Count; i++)
            {
                if (tempasteroids[i].captured == false)
                {
                    if (smallest > Vector3.Distance(player.enemyspacestations[0].transform.position, tempasteroids[i].transform.position))
                    {
                        smallest = Vector3.Distance(player.enemyspacestations[0].transform.position, tempasteroids[i].transform.position);
                        smallesta = tempasteroids[i];
                    }
                }
            }

            List<enemy> tempenemyies = player.enemys;

            enemy smalleste = tempenemyies[0];
            smallest = Vector3.Distance(transform.position, tempenemyies[0].transform.position);
            for (int i = 0; i < tempenemyies.Count; i++)
            {
                if (tempenemyies[i].bigunit == false)
                {
                    if (smallest > Vector3.Distance(transform.position, tempenemyies[i].transform.position))
                    {
                        smallest = Vector3.Distance(transform.position, tempenemyies[i].transform.position);
                        smalleste = tempenemyies[i];
                    }
                }
            }

            smalleste.wantsToCapture = true;
            smalleste.GetComponent<NavMeshAgent>().isStopped = false;
            smalleste.GetComponent<NavMeshAgent>().ResetPath();
            smalleste.GetComponent<NavMeshAgent>().SetDestination(smallesta.transform.position);
        }
    }

    void makeMine()
    {
        if (mon.mymoney >= 50 && capturedAsteroids.Count > 0)
        {
            Debug.Log("making mine");
            List<asteroid> nonmines = capturedAsteroids;
            asteroid[] temp = nonmines.ToArray();
            foreach (asteroid a in temp)
            {
                if (a != null) 
                {
                    if (a.hasmine == true)
                    {
                        nonmines.Remove(a);
                    }
                }
                else
                {
                    nonmines.Remove(a);
                }
            }

            if (nonmines.Count > 0)
            {
                asteroid smallesta = nonmines[0];
                float smallest = Vector3.Distance(player.enemyspacestations[0].transform.position, nonmines[0].transform.position);
                for (int i = 0; i < nonmines.Count; i++)
                {
                    if (smallest > Vector3.Distance(player.enemyspacestations[0].transform.position, nonmines[i].transform.position))
                    {
                        smallest = Vector3.Distance(player.enemyspacestations[0].transform.position, nonmines[i].transform.position);
                        smallesta = nonmines[i];
                    }
                }

                smallesta.hasmine = true;
                mines += 1;
            }
        }
    }

    void patrol()
    {
        if(player.enemys.Count > 0)
        {
            Debug.Log("patrol");
            Vector3 randomp = RandomPointInBounds(patrolarea.GetComponent<BoxCollider>().bounds);
            randompoint.transform.position = new Vector3(randomp.x, player.enemys[0].transform.position.y, randomp.z);

            List<enemy> tempenemyies = player.enemys;
            enemy[] tempe = tempenemyies.ToArray();
            foreach (enemy enemy in tempe)
            {
                if (tempenemyies.Contains(enemy) && enemy != null)
                {
                    if (enemy.GetComponent<enemy>().bigunit == false && enemy.GetComponent<enemy>().attacking == false && enemy.GetComponent<enemy>().wantsToCapture == false)
                    {
                        enemy.GetComponent<NavMeshAgent>().isStopped = false;
                        enemy.StartCoroutine(enemy.movetowardsanddontstop(randompoint.transform.position));
                    }
                }
            }
        }

    }

    void purchase()
    {
        Debug.Log("buying ship");
        foreach(GameObject ship in spawnableUnits)
        {
            if (ship.GetComponent<enemy>().price <= mon.mymoney && justbought != ship)
            {
                spacestation onetouse = transform.Find("SpaceStations").GetChild(0).GetComponent<spacestation>();
                for (int i = 0; i < transform.Find("SpaceStations").childCount; i++)
                {
                    if (transform.Find("SpaceStations").GetChild(i).gameObject.activeInHierarchy == true)
                    {
                        onetouse = transform.Find("SpaceStations").GetChild(i).GetComponent<spacestation>();
                        break;
                    }
                }

                justbought = ship;
                onetouse.GetComponent<spawnunit>().spawn(ship);
                mon.subtractMoney(ship.GetComponent<enemy>().price);
            }
        }
    }

    void moveTowardsEnemy()
    {
        Debug.Log("moving to player space station");
        List<enemy> tempenemyies = player.enemys;
        enemy[] tempe = tempenemyies.ToArray();
        foreach (enemy enemy in tempe)
        {
            if (tempenemyies.Contains(enemy) && enemy != null)
            {
                Debug.Log("this " + enemy.name);
                if (enemy.GetComponent<enemy>().attacking == false)
                {
                    Debug.Log("mobifg " + enemy.name);
                    enemy.wantsToCapture = false;
                    enemy.target = player.spacestations[0].gameObject;
                    enemy.GetComponent<NavMeshAgent>().isStopped = false;
                    enemy.StartCoroutine(enemy.movetowardsanddontstop(player.spacestations[0].transform.position));
                }
            }
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
