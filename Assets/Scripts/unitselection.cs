using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class unitselection : MonoBehaviour
{
    public RectTransform selectionbox;
    public LayerMask unitlayermask;
    public LayerMask sslayermask;
    public LayerMask asteroidlayermask;
    public GameObject[] spacestationuis;
    public GameObject shipui;
    public GameObject asteroidui;

    public List<unit> selectedunits = new List<unit>();
    private Vector2 startpos;

    private Camera cam;
    private player player;
    bool selectedAsteroid = false;

    private void Awake()
    {
        cam = Camera.main;
        player = GetComponent<player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            ToggleSelectionVisual(false);
            selectedunits = new List<unit>();

            TrySelect(Input.mousePosition);
            startpos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    void TrySelect(Vector2 screenpos)
    {
        Ray ray = cam.ScreenPointToRay(screenpos);
        RaycastHit hit;
        selectedAsteroid = false;

        if (Physics.Raycast(ray, out hit, 100, unitlayermask))
        {
            unit unit = hit.collider.GetComponent<unit>();

            if (player.IsMyUnit(unit) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (unit.selected == false)
                {
                    unit.selected = true;
                    selectedunits.Add(unit);
                    unit.ToggleSelectionVisual(true);
                }
                else
                {
                    unit.selected = false;
                    selectedunits.Remove(unit);
                    unit.ToggleSelectionVisual(false);
                }
            }
        }

        if (Physics.Raycast(ray, out hit, 100, sslayermask))
        {
            spacestation ss = hit.collider.GetComponent<spacestation>();

            if (player.IsMyspacestation(ss) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (ss.ui.activeInHierarchy)
                {
                    shipui.SetActive(false);
                }
                else
                {
                    ss.ui.SetActive(true);
                }

                if (asteroidui.activeInHierarchy)
                {
                    asteroidui.SetActive(false);
                }
            }
        }

        if (Physics.Raycast(ray, out hit, 100, asteroidlayermask))
        {
            asteroid asteroid = hit.collider.GetComponent<asteroid>();

            if (asteroid.captured == true && EventSystem.current.IsPointerOverGameObject() == false)
            {
                selectedAsteroid = true;
                asteroidui.GetComponent<asteroidui>().whatasteroid = asteroid;
                if (asteroidui.activeInHierarchy == false)
                {
                    asteroidui.SetActive(true);
                    asteroidui.transform.Find("Panel").GetChild(0).gameObject.SetActive(true);
                }
                
                if (shipui.activeInHierarchy)
                {
                    shipui.SetActive(false);
                }
                foreach (GameObject ui in spacestationuis)
                {
                    if (ui.activeInHierarchy)
                    {
                        ui.SetActive(false);
                    }
                }
            }
            else if (asteroidui.activeInHierarchy == true)
            {
                asteroidui.SetActive(false);
            }
        }
    }

    void UpdateSelectionBox(Vector2 curmousepos)
    {
        if (!selectionbox.gameObject.activeInHierarchy)
        {
            selectionbox.gameObject.SetActive(true);
        }

        float width = curmousepos.x - startpos.x;
        float height = curmousepos.y - startpos.y;

        selectionbox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionbox.anchoredPosition = startpos + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        selectionbox.gameObject.SetActive(false);

        Vector2 min = selectionbox.anchoredPosition - (selectionbox.sizeDelta / 2);
        Vector2 max = selectionbox.anchoredPosition + (selectionbox.sizeDelta / 2);

        unit[] temp = player.units.ToArray();

        foreach (unit unit in temp)
        {
            if (player.units.Contains(unit) && unit != null)
            {
                Vector3 screenpos = cam.WorldToScreenPoint(unit.transform.position);

                if (screenpos.x > min.x && screenpos.x < max.x && screenpos.y > min.y && screenpos.y < max.y)
                {
                    selectedunits.Add(unit);
                    unit.ToggleSelectionVisual(true);
                }
            }
            else if (player.units.Contains(unit) && unit == null)
            {
                player.units.Remove(unit);
            }
        }

        if (selectedunits.Count > 0)
        {
            if (shipui.activeInHierarchy == false)
            {
                shipui.SetActive(true);
                foreach(GameObject ui in spacestationuis)
                {
                    if (ui.activeInHierarchy)
                    {
                        ui.SetActive(false);
                    }
                }

                if (asteroidui.activeInHierarchy && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    asteroidui.SetActive(false);
                }
            }
            else if (shipui.activeInHierarchy == true  && EventSystem.current.IsPointerOverGameObject() == false)
            {
                shipui.SetActive(false);
            }
        }
        else
        {
            if (shipui.activeInHierarchy == true && EventSystem.current.IsPointerOverGameObject() == false)
            {
                shipui.SetActive(false);
            }
            if (asteroidui.activeInHierarchy == true && selectedAsteroid == false)
            {
                asteroidui.SetActive(false);
            }
        }
    }

    void ToggleSelectionVisual(bool selected)
    {
        unit[] temp = selectedunits.ToArray();

        foreach (unit unit in temp)
        {
            if (selectedunits.Contains(unit) && unit != null)
            {
                unit.ToggleSelectionVisual(selected);
            }
            else if (selectedunits.Contains(unit) && unit == null)
            {
                selectedunits.Remove(unit);
            }
        }
    }
}
