using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    private const float DayHour = 24.0f;
    private const float MorningSliderPercent = 0.35f;
    public float slider = MorningSliderPercent;
    public float slider2;
    public float hour;
    public int totalDays = 0;
    public float speed = 600.0f;
    public Light sun;
    public Color nightFogColor;
    public Color duskFogColor;
    public Color morningFogColor;
    public Color middayFogColor;
    public Color nightAmbientLight;
    public Color duskAmbientLight;
    public Color morningAmbientLight;
    public Color middayAmbientLight;
    public Color nightTint;
    public Color duskTint;
    public Color morningTint;
    public Color middayTint;
    public Color sunNight;
    public Color sunDay;
    public Material skyBoxMaterial1;
    public Material skyBoxMaterial2;
    private float timeOfDay;
    private bool dayChanged = false;

    //Draw on GUI
    void OnGUI()
    {
        if (slider >= 1.0f) {
            slider = 0f;
            dayChanged = true;
        }

        slider = GUI.HorizontalSlider(new Rect(20, 20, 200, 30), slider, 0, 1.0f);

        hour = slider * DayHour;
        timeOfDay = slider2 * DayHour;

        sun.transform.localEulerAngles = new Vector3((slider * 360) - 90, 0, 0);
        slider = slider + Time.deltaTime / speed;
        sun.color = Color.Lerp(sunNight, sunDay, slider * 2);

        if (slider < 0.5f) {
            slider2 = slider;
        } else {
            slider2 = 1 - slider;
        }

        if (slider >= MorningSliderPercent && dayChanged) {
            totalDays += 1;
            dayChanged = false;
        }

        sun.intensity = (slider2 - 0.2f) * 1.7f;

        if (timeOfDay < 4) {
            //it is Night
            RenderSettings.skybox = skyBoxMaterial1;
            RenderSettings.skybox.SetFloat("_Blend", 0);
            skyBoxMaterial1.SetColor("_Tint", nightTint);
            RenderSettings.ambientLight = nightAmbientLight;
            RenderSettings.fogColor = nightFogColor;
        }

        if (timeOfDay > 4 && timeOfDay < 6) {
            RenderSettings.skybox = skyBoxMaterial1;
            RenderSettings.skybox.SetFloat("_Blend", 0);
            RenderSettings.skybox.SetFloat("_Blend", (timeOfDay / 2) - 2);
            skyBoxMaterial1.SetColor("_Tint", Color.Lerp(nightTint, duskTint, (timeOfDay / 2) - 2));
            RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, duskAmbientLight, (timeOfDay / 2) - 2);
            RenderSettings.fogColor = Color.Lerp(nightFogColor, duskFogColor, (timeOfDay / 2) - 2);
            //it is Dusk
        }

        if (timeOfDay > 6 && timeOfDay < 8) {
            RenderSettings.skybox = skyBoxMaterial2;
            RenderSettings.skybox.SetFloat("_Blend", 0);
            RenderSettings.skybox.SetFloat("_Blend", (timeOfDay / 2) - 3);
            skyBoxMaterial2.SetColor("_Tint", Color.Lerp(duskTint, morningTint, (timeOfDay / 2) - 3));
            RenderSettings.ambientLight = Color.Lerp(duskAmbientLight, morningAmbientLight, (timeOfDay / 2) - 3);
            RenderSettings.fogColor = Color.Lerp(duskFogColor, morningFogColor, (timeOfDay / 2) - 3);
            //it is Morning
        }

        if (timeOfDay > 8 && timeOfDay < 10) {
            RenderSettings.ambientLight = middayAmbientLight;
            RenderSettings.skybox = skyBoxMaterial2;
            RenderSettings.skybox.SetFloat("_Blend", 1);
            skyBoxMaterial2.SetColor("_Tint", Color.Lerp(morningTint, middayTint, (timeOfDay / 2) - 4));
            RenderSettings.ambientLight = Color.Lerp(morningAmbientLight, middayAmbientLight, (timeOfDay / 2) - 4);
            RenderSettings.fogColor = Color.Lerp(morningFogColor, middayFogColor, (timeOfDay / 2) - 4);
        }
    }
}
