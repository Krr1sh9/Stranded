using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public Light moon; 
    public float dayDurationSeconds = 120f;
    
    [Range(0, 1)]
    public float timeOfDay;

    public Gradient sunColor; 

    public Gradient ambientLightColor;
    
    void Update()
    {
        if (sun == null || moon == null) 
            return;

        // Increment and wrap time
        timeOfDay += Time.deltaTime / dayDurationSeconds;
        timeOfDay %= 1f;

        // Rotate the sun
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0);

        // Rotate the moon
        float moonAngle = sunAngle + 180f;
        moon.transform.rotation = Quaternion.Euler(moonAngle, -30f, 0);
        
        // Set the sun's color directly
        sun.color = sunColor.Evaluate(timeOfDay);
        
        // Set the ambient light color
        RenderSettings.ambientLight = ambientLightColor.Evaluate(timeOfDay);
    }
}