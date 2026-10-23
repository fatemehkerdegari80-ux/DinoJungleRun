using UnityEngine;
using UnityEngine.UI;

public class HealthBarImageUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image healthBarImage;

    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;

    private int lastLives = -1;

    private void Awake()
    {
        if (healthBarImage == null)
            healthBarImage = GetComponent<Image>();
    }

    private void Start()
    {
        RefreshHealthBar();
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        if (playerHealth.CurrentLives != lastLives)
        {
            RefreshHealthBar();
        }
    }

    private void RefreshHealthBar()
    {
        if (playerHealth == null || healthBarImage == null)
            return;

        lastLives = playerHealth.CurrentLives;

        switch (lastLives)
        {
            case 3:
                healthBarImage.sprite = health3;
                break;
            case 2:
                healthBarImage.sprite = health2;
                break;
            case 1:
                healthBarImage.sprite = health1;
                break;
            default:
                healthBarImage.sprite = health0;
                break;
        }
    }
}