using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI ghostTimerText;
    public CharacterController2D playerReference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // clean up the number by rounding it to 3 decimal places
        ghostTimerText.text = (Mathf.Round(playerReference.m_TimeLeftInGhostForm * 1000) / 1000).ToString();
    }
}
