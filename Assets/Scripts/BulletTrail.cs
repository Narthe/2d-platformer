using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {

    private Vector3 m_start, m_end;
    private float m_duration = 0f;
    private bool m_runAnimation;
    private float m_startTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (m_runAnimation)
        {
            if (Time.time > m_startTime + m_duration || Vector3.Distance(transform.position, m_end) < 1f)
            {
                Destroy(gameObject);
            }
            transform.position = Vector3.MoveTowards(transform.position, m_end, 1f);
        }
	}

    public void DrawLine(Vector3 start, Vector3 end, float duration)
    {
        m_start = start;
        m_end = end;
        m_duration = duration;

        m_startTime = Time.time;
        m_runAnimation = true;
    }
}
