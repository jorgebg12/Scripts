using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button next;
    public Button prev;
    public TextMeshProUGUI explicacion;
    public Image imagen1;
    void Start()
    {
        prev.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
