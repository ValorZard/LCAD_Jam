using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerSpawner : MonoBehaviour
{
    public GameObject daggerTemplate;
    public float m_TimeForDaggerSpawn = 2f;
    public float m_TimeLeftForNextSpawn = 6f;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_TimeLeftForNextSpawn -= Time.deltaTime;
        if (m_TimeLeftForNextSpawn <= 0)
        {
            GameObject dagger = Instantiate(daggerTemplate);
            dagger.transform.position = transform.position;
            dagger.GetComponent<DaggerScript>().velocity = velocity;
            m_TimeLeftForNextSpawn = m_TimeForDaggerSpawn;
        }
    }
}
