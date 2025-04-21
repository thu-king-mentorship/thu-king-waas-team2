using UnityEngine;
using UnityEngine.UI;
/* http://www.Mousawi.Dev By @AbdullaMousawi*/
public class LightUI : MonoBehaviour
{
    public Image ringHealthBar;

    float health, maxHealth = 100;
    float lerpSpeed;

    private bool isHealing = false; // Controla si se debe curar o no

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        HealthBarFiller();
        ColorChanger();

        // Se cura solo si isHealing es true
       
    }

    void HealthBarFiller()
    {
        // Se cura solo si isHealing es true
        if (isHealing)
        {
            ringHealthBar.fillAmount = Mathf.Lerp(ringHealthBar.fillAmount, (health / maxHealth), lerpSpeed * Time.deltaTime);
        }
        
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        ringHealthBar.color = healthColor;
    }

    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
            if (health > maxHealth) health = maxHealth;
        }
    }

    public void SetHealing(bool healingStatus)
    {
        isHealing = healingStatus;
    }
}
