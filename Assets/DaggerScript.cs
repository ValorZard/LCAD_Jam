using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if (col.TryGetComponent(out CharacterController2D playerRef))
        {
            playerRef.GoGhost();
            playerRef.currentGhostState = CharacterController2D.GhostState.Speed;
        }
    }
}