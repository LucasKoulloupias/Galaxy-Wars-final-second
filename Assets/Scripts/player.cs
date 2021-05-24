using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class player : MonoBehaviour
{
    private unitselection us;
    private money mon;
    private fogofwar fog;
    private Camera cam;

    public GameObject win;

    public GameObject lose;

    public List<unit> units = new List<unit>();
    public List<spacestation> spacestations = new List<spacestation>();
    public List<spacestation> enemyspacestations = new List<spacestation>();
    public List<enemy> enemys = new List<enemy>();
    public List<asteroid> asteroids = new List<asteroid>();
    public GameObject gotomarker;
    public GameObject camerapostion;

    public int mines = 0;
    public bool shipview;
    public bool testenemy = true;
    public bool madearrays = false;

    public bool IsMyUnit(unit unit)
    {
        if (units.Contains(unit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsMyspacestation(spacestation ss)
    {
        if (spacestations.Contains(ss))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsenemyUnit(enemy enemy)
    {
        if (enemys.Contains(enemy))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void makearrays()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 8)
            {
                units.Add(obj.GetComponent<unit>());
                fog.m_players.Add(obj.transform);
            }
        }
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 9)
            {
                enemys.Add(obj.GetComponent<enemy>());
            }
        }
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 10)
            {
                spacestations.Add(obj.GetComponent<spacestation>());
                fog.m_players.Add(obj.transform);
            }
        }
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 15)
            {
                enemyspacestations.Add(obj.GetComponent<spacestation>());
            }
        }
        foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.layer == 13)
            {
                asteroids.Add(obj.GetComponent<asteroid>());
            }
        }
        madearrays = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        us = GetComponent<unitselection>();
        fog = GetComponent<fogofwar>();
        mon = GetComponent<money>();
        StartCoroutine(minemoney());

        
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouse_pos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mouse_pos);
            RaycastHit raycast_hit;
            Physics.Raycast(ray, out raycast_hit, 100, LayerMask.GetMask("ground", "enemy", "asteroid"));
            if (raycast_hit.transform.gameObject.layer == 17)
            {
                gotomarker.transform.GetChild(0).gameObject.SetActive(true);
                gotomarker.transform.position = new Vector3(raycast_hit.point.x, gotomarker.transform.position.y, raycast_hit.point.z);
                unit[] temp = us.selectedunits.ToArray();
                foreach (unit unit in temp)
                {
                    if (us.selectedunits.Contains(unit) && unit != null)
                    {
                        unit.wantsToCapture = false;
                        unit.targetenemy = null;
                        StartCoroutine(coolenemytest(unit));
                        if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                        {
                            unit.GetComponent<NavMeshAgent>().isStopped = false;
                            unit.GetComponent<NavMeshAgent>().SetDestination(gotomarker.transform.position);
                        }
                    }
                    else if (us.selectedunits.Contains(unit) && unit == null)
                    {
                        us.selectedunits.Remove(unit);
                    }
                }
            }
            else if (raycast_hit.transform.gameObject.layer == 9)
            {
                unit[] temp = us.selectedunits.ToArray();
                foreach (unit unit in temp)
                {
                    if (us.selectedunits.Contains(unit) && unit != null)
                    {
                        unit.targetenemy = null;
                        StartCoroutine(coolenemytest(unit));
                        unit.GetComponent<NavMeshAgent>().isStopped = false;
                        unit.GetComponent<NavMeshAgent>().SetDestination(raycast_hit.transform.position);
                        unit.GetComponent<unit>().targetenemy = raycast_hit.transform.gameObject;
                    }
                    else if (us.selectedunits.Contains(unit) && unit == null)
                    {
                        us.selectedunits.Remove(unit);
                    }
                }
            }
            else if (raycast_hit.transform.gameObject.layer == 13)
            {
                if (raycast_hit.transform.gameObject.GetComponent<asteroid>().captured == false)
                {
                    unit[] temp = us.selectedunits.ToArray();
                    us.selectedunits[0].wantsToCapture = true;
                    foreach (unit unit in temp)
                    {
                        if (us.selectedunits.Contains(unit) && unit != null)
                        {
                            unit.targetenemy = null;
                            StartCoroutine(coolenemytest(unit));
                            unit.GetComponent<NavMeshAgent>().isStopped = false;
                            unit.GetComponent<NavMeshAgent>().SetDestination(raycast_hit.transform.position);
                            unit.GetComponent<unit>().targetenemy = raycast_hit.transform.gameObject;
                        }
                        else if (us.selectedunits.Contains(unit) && unit == null)
                        {
                            us.selectedunits.Remove(unit);
                        }
                    }
                }
            }
        }

        if (shipview == false)
        {
            camerapostion.transform.position = transform.position;
            camerapostion.transform.rotation = transform.rotation;
        }

        unit[] temp2 = units.ToArray();
        foreach (unit unit in temp2)
        {
            if (units.Contains(unit) && unit != null)
            {
                enemy[] temp3 = enemys.ToArray();
                foreach (enemy enemy in temp3)
                {
                    if (enemys.Contains(enemy) && enemy != null)
                    {
                        if (enemy != null && unit != null && Vector3.Distance(unit.transform.position, enemy.transform.position) < enemy.enemyrange && testenemy == true)
                        {
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled && unit.bigunit == false)
                            {
                                unit.GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);
                                unit.GetComponent<NavMeshAgent>().isStopped = true;
                            }  
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.bigunit == false)
                            {
                                enemy.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position);
                            }
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                            {
                                enemy.GetComponent<NavMeshAgent>().isStopped = true;
                            }
                        }
                        if (enemy != null && unit != null && Vector3.Distance(unit.transform.position, enemy.transform.position) <= enemy.enemyrange * 1.4f && Vector3.Distance(unit.transform.position, enemy.transform.position) > enemy.enemyrange)
                        {
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.bigunit == false)
                            {
                                enemy.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position);
                            }
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && enemy.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                            {
                                enemy.GetComponent<NavMeshAgent>().isStopped = true;
                            }
                        }
                    }
                    else if (enemys.Contains(enemy) && enemy == null)
                    {
                        enemys.Remove(enemy);
                    }
                }

                spacestation[] temp4 = enemyspacestations.ToArray();
                foreach (spacestation enemy in temp4)
                {
                    if (enemyspacestations.Contains(enemy) && enemy != null)
                    {
                        if (enemy != null && unit != null && Vector3.Distance(unit.transform.position, enemy.transform.position) < enemy.enemyrange && testenemy == true)
                        {
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled && unit.bigunit == false)
                                unit.GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);
                            if (unit.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                                unit.GetComponent<NavMeshAgent>().isStopped = true;
                        }
                    }
                    else if (enemyspacestations.Contains(enemy) && enemy == null)
                    {
                        enemyspacestations.Remove(enemy);
                    }
                }
            }
            else if (units.Contains(unit) && unit == null)
            {
                units.Remove(unit);
            }
        }

        if (madearrays == true && enemys.Count == 0)
        {
            Win();
        }

        if (madearrays == true && units.Count == 0)
        {
            Lose();
        }
    }

    public void shipView(List<unit> selectedbois)
    {
        Debug.Log("poop");
        if (shipview == false && selectedbois.Count > 0)
        {
            Debug.Log("poop2");
            shipview = true;
            GetComponent<CameraController>().enabled = false;
            GetComponent<Camera>().fieldOfView = 60;
            transform.position = selectedbois[0].transform.Find("camerapoint").position;
            transform.rotation = selectedbois[0].transform.Find("camerapoint").rotation;
            transform.parent = selectedbois[0].transform.Find("camerapoint");
            selectedbois[0].transform.Find("healthbar").GetComponent<Canvas>().enabled = false;
            GetComponent<fogofwar>().enabled = false;
            GetComponent<fogofwar>().m_fogOfWarPlane.SetActive(false);
            if (us.shipui.activeInHierarchy == true)
            {
                us.shipui.SetActive(false);
            }
        }
        else if (shipview == true)
        {
            shipview = false;
            transform.parent = null;
            transform.position = camerapostion.transform.position;
            transform.rotation = camerapostion.transform.rotation;
            GetComponent<Camera>().fieldOfView = 40;
            GetComponent<CameraController>().enabled = true;
            selectedbois[0].transform.Find("healthbar").GetComponent<Canvas>().enabled = true;
            GetComponent<fogofwar>().m_fogOfWarPlane.SetActive(true);
            GetComponent<fogofwar>().enabled = true;
        }
    }


    IEnumerator coolenemytest(unit unit)
    {
        testenemy = false;
        if (unit.bigunit == true)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        testenemy = true;
    }

    private Vector3 GetPointUnderCursor()
    {
        Vector2 mouse_pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit raycast_hit;
        Physics.Raycast(ray, out raycast_hit, 100, LayerMask.GetMask("Default", "enemy"));
        return raycast_hit.point;
    }

    public void Win()
    {
        Debug.Log("WIN");
        win.SetActive(true);
    }

    public void Lose()
    {
        Debug.Log("LOSE");
        lose.SetActive(true);
    }
}
