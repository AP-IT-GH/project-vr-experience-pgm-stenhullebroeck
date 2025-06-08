using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI percentageText;
    public HealthManager playerHealth;

    private int maxHealth;

    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("Player HealthManager not assigned!");
            return;
        }

        maxHealth = playerHealth.health;
    }

    void Update()
    {
        float current = playerHealth.health;
        float fraction = Mathf.Clamp01(current / maxHealth);

        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, fraction, Time.deltaTime * 10f);

        percentageText.text = Mathf.RoundToInt(fraction * 100f) + "%";
    }
}
