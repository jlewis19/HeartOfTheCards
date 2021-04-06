using UnityEngine;
using UnityEngine.UI;

public class DashCooldown : MonoBehaviour
{
    public Image pDashImage;
    public Image pCooldownImage;
    public Image pLockImage;

    static Image dashImage;
    static Image cooldownImage;
    static Image lockImage;

    // Start is called before the first frame update
    void Start()
    {
        dashImage = pDashImage;
        cooldownImage = pCooldownImage;
        lockImage = pLockImage;

        lockImage.enabled = false;
        cooldownImage.fillAmount = 0;
    }

    public static void UpdateDashUI(float time, float cooldown)
    {
        if (time >= cooldown)
        {
            lockImage.enabled = false;
            cooldownImage.fillAmount = 0;
            var dashImageColor = dashImage.color;
            dashImageColor.a = 1f;
            dashImage.color = dashImageColor;
        }
        else
        {
            lockImage.enabled = true;
            cooldownImage.fillAmount = time / cooldown;
            var dashImageColor = dashImage.color;
            dashImageColor.a = 0.4f;
            dashImage.color = dashImageColor;
        }
    }
}
