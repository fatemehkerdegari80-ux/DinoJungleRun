
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private Slider healthSlider;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (playerHealth == null || healthSlider == null)
        {
            Debug.LogError("HealthBar is not connected!");
            return;
        }

        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.maxLives;
        healthSlider.wholeNumbers = true;
        healthSlider.value = playerHealth.CurrentLives;
    }

    private void Update()
    {
        if (playerHealth == null || healthSlider == null)
            return;

        healthSlider.value = playerHealth.CurrentLives;
    }
}
