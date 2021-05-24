using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogofwar : MonoBehaviour
{

	public GameObject m_fogOfWarPlane;
	public List<Transform> m_players = new List<Transform>();
	public LayerMask m_fogLayer;
	public float m_radius = 5f;
	private float m_radiusSqr { get { return m_radius * m_radius; } }

	private Mesh m_mesh;
	private Vector3[] m_vertices;
	private Color[] m_colors;

	// Use this for initialization
	void Start()
	{
		Initialize();
	}

	// Update is called once per frame
	void Update()
	{
		Transform[] temp = m_players.ToArray();
		m_colors = new Color[m_vertices.Length];
		for (int i = 0; i < m_colors.Length; i++)
		{
			m_colors[i] = new Color(0.5f, 0.5f, 0.5f, 1);
		}
		UpdateColor();
		foreach (Transform player in temp)
        {
			if (m_players.Contains(player) && player != null)
			{
				Ray r = new Ray(transform.position, player.position - transform.position);
				RaycastHit hit;
				if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
				{
					
					for (int b = 0; b < m_vertices.Length; b++)
					{
						Vector3 v = m_fogOfWarPlane.transform.TransformPoint(m_vertices[b]);
						float dist = Vector3.SqrMagnitude(v - hit.point);
						if (dist < m_radiusSqr)
						{
							float alpha = Mathf.Min(m_colors[b].a, dist / m_radiusSqr);
							m_colors[b].a = alpha;
						}
					}
					UpdateColor();
				}
			}
			else if (m_players.Contains(player) && player == null)
			{
				m_players.Remove(player);
			}
		}
	}

	void Initialize()
	{
		m_fogOfWarPlane.SetActive(true);
		m_mesh = m_fogOfWarPlane.GetComponent<MeshFilter>().mesh;
		m_vertices = m_mesh.vertices;
		m_colors = new Color[m_vertices.Length];
		for (int i = 0; i < m_colors.Length; i++)
		{
			m_colors[i] = new Color(0.5f, 0.5f, 0.5f, 1);
		}
		UpdateColor();
	}

	void UpdateColor()
	{
		m_mesh.colors = m_colors;
	}
}

