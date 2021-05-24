using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour

{
    Camera cam;

    public int health;

    public int shield;

    public LayerMask groundLayer;

    public NavMeshAgent playerAgent;

    #region MonoBehaviour API

    private void Awake()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAgent.SetDestination(GetPointUnderCursor());
        }
    }

    #endregion

    private Vector3 GetPointUnderCursor()
    {
        Vector2 mouse_pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        RaycastHit raycast_hit;
        Physics.Raycast(ray, out raycast_hit);
        return raycast_hit.point;
    }

}
