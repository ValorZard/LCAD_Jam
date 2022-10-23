using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyScript : MonoBehaviour
{
    public bool hasLeftBody = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if (col.TryGetComponent(out CharacterController2D playerRef))
        {
            if (!hasLeftBody)
            {
                hasLeftBody = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if (col.TryGetComponent(out CharacterController2D playerRef))
        {
            if (hasLeftBody)
            {
                playerRef.m_TimeLeftInGhostForm = 0;
                hasLeftBody = false;
            }
        }
    }
    

}
