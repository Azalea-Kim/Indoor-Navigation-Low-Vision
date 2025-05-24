using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGlowPulse : MonoBehaviour
{
    public Material glowMaterial;
    public float pulseSpeed = 2f;

    private Color originalColor;

    void Start()
    {
        originalColor = glowMaterial.color;  
    }

    void Update()
    {
        float intensity = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        float alpha = 0.2f + 0.2f * intensity;
        glowMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
