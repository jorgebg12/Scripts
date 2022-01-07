using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeBar : MonoBehaviour
{
    public static UITimeBar instance { get; private set; }
    
    public Image mask;
    public float maxTime = 5f;
    float originalSize;
    public float timeLeft;
    public bool ended;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        timeLeft = maxTime;
        ended = false;
    }

    void Update()
    {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize - originalSize * timeLeft / maxTime);
        } else {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize);
            ended = true;
        }
        
    }
}