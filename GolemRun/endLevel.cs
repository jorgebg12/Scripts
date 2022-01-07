using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevel : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup Background;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;

     void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == player)
        {
        
            m_IsPlayerCaught = true;
        }
    }
    void Update ()
    {
        
        if (m_IsPlayerCaught)
        {
            EndLevel (Background, true);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)
    {
        
        m_Timer += Time.deltaTime;

        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene ("Menu");
            
            
        }
    }
}
