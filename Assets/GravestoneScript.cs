using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravestoneScript : MonoBehaviour
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
        GameObject player = GameObject.Find("Player");

        if (col.TryGetComponent(out CharacterController2D playerRef))
        {
            player.GetComponent<CharacterController2D>().TurnBackIntoHuman();
        }
        else
        {
            player.GetComponent<CharacterController2D>().RewindBackToDeadBody();
        }
    }
}
