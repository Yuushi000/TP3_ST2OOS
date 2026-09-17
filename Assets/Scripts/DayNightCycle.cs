using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle")]
    [Tooltip("Duree d'un cycle complet de 24h virtuelles, en minutes reelles")]
    public float dayLengthInMinutes = 2f;

    [Tooltip("Multiplicateur pour accelerer/ralentir le temps manuellement")]
    public float timeScaleMultiplier = 1f;

    [Header("Intensite lumiere")]
    public float maxIntensity = 1.2f;
    public float minIntensity = 0.05f;

    [Header("Debug (lecture seule)")]
    [Range(0f, 24f)]
    public float currentTimeOfDay = 6f; // commence a 6h du matin

    private Light sunLight;

    void Start()
    {
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        // Combien d'heures virtuelles s'ecoulent par seconde reelle
        float hoursPerSecond = 24f / (dayLengthInMinutes * 60f);

        currentTimeOfDay += hoursPerSecond * timeScaleMultiplier * Time.deltaTime;
        currentTimeOfDay %= 24f; // boucle sur 24h

        UpdateSun();
    }

    void UpdateSun()
    {
        // 0h = minuit (soleil au plus bas), 12h = midi (soleil au plus haut)
        // On fait tourner la lumiere sur 360 degres pour un cycle complet
        float sunAngle = (currentTimeOfDay / 24f) * 360f - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Intensite basee sur la hauteur du soleil (sin de l'angle)
        float sunHeight = Mathf.Sin((currentTimeOfDay / 24f) * Mathf.PI * 2f - Mathf.PI / 2f);
        float normalizedHeight = Mathf.Clamp01((sunHeight + 1f) / 2f); // 0 a 1

        sunLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, normalizedHeight);
    }
}