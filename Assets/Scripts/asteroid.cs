using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class asteroid : MonoBehaviour
{
    public Slider capturebar;
    GameObject onecaptuting;
    colliderinfo range;

    public bool captured;
    public bool capturing;
    public bool hasmine;

    // Start is called before the first frame update
    void Start()
    {
        capturebar.transform.parent.gameObject.SetActive(false);
        range = GetComponentInChildren<colliderinfo>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject thing in range.objectsincollider)
        {
            if (thing.layer == 8 && captured == false && capturing == false)
            {
                if (thing.GetComponent<unit>().wantsToCapture == true)
                {
                    onecaptuting = thing;
                    thing.GetComponent<unit>().captureAsteroid(gameObject);
                    capturebar.transform.parent.gameObject.SetActive(true);
                    capturing = true;
                    StartCoroutine(capture(thing.GetComponent<unit>()));
                }
            }
            if (thing.layer == 9 && captured == false && capturing == false)
            {
                if (thing.GetComponent<enemy>().wantsToCapture == true)
                {
                    onecaptuting = thing;
                    thing.GetComponent<enemy>().captureAsteroid(gameObject);
                    capturebar.transform.parent.gameObject.SetActive(true);
                    capturing = true;
                    StartCoroutine(captureEnemy(thing.GetComponent<enemy>()));
                }
            }
        }

        if (onecaptuting != null && range.objectsincollider.Contains(onecaptuting) == false)
        {
            if (captured == false && capturing == true)
            {
                capturing = false;
                StopAllCoroutines();
                onecaptuting = null;
            }
        }

        if (hasmine == true && GetComponent<MeshRenderer>().material.color != Color.yellow)
        {
            Renderer _renderer;
            MaterialPropertyBlock _propBlock;

            _propBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();

            // Get the current value of the material properties in the renderer.
            _renderer.GetPropertyBlock(_propBlock);
            // Assign our new value.
            _propBlock.SetColor("_Color", Color.yellow);
            // Apply the edited values to the renderer.
            _renderer.SetPropertyBlock(_propBlock);
        }
    }

    public IEnumerator capture(unit onecaptuing)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            capturebar.value += 1f;
            if (capturebar.value == 100)
            {
                break;
            }
            if (onecaptuing == null)
            {
                break;
            }
        }
        beencaptured();
    }

    public IEnumerator captureEnemy(enemy onecaptuing)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            capturebar.value += 1f;
            if (capturebar.value == 100)
            {
                break;
            }
            if (onecaptuing == null)
            {
                break;
            }
        }
        beencapturedByEnemy();
    }

    public void beencaptured()
    {
        onecaptuting.GetComponent<unit>().wantsToCapture = false;
        onecaptuting = null;
        captured = true;
        capturing = false;
        capturebar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.8392157f, 0.754051f, 0);
        Camera.main.GetComponent<fogofwar>().m_players.Add(gameObject.transform);
    }

    public void beencapturedByEnemy()
    {
        onecaptuting.GetComponent<enemy>().wantsToCapture = false;
        onecaptuting = null;
        captured = true;
        capturing = false;
        capturebar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.8392157f, 0.754051f, 0);
        gameObject.layer = 16;
        GameObject.Find("EnemyUnits").GetComponent<ai>().capturedAsteroids.Add(this);
    }
}
