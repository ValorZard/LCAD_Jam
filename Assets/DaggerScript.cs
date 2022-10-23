using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerScript : MonoBehaviour
{
    public Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + (velocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if (col.TryGetComponent(out CharacterController2D playerRef))
        {
            playerRef.currentGhostState = CharacterController2D.GhostState.Speed;
            playerRef.GoGhost();
        }
        print(col.gameObject.name);
        Destroy(gameObject);
    }
}
