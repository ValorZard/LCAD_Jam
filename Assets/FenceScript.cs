using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceScript : MonoBehaviour
{
    private CharacterController2D playerRef;
    private GameObject colliderGameObj;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = Object.FindObjectOfType<CharacterController2D>();
        colliderGameObj = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerRef.m_isInGhostForm)
        {
            colliderGameObj.SetActive(false);
        }
        else
        {
            colliderGameObj.SetActive(true);
        }
    }
}
