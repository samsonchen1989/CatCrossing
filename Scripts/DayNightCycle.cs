using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
	private const float DAY_HOUR = 24;
	private const float MorningSliderPercent = 0.35f;

	public float slider = MorningSliderPercent;
	public float slider2;
	public float Hour;
	public int Days = 0;

	public float speed = 600.0f;

	public Light sun;

	public Color NightFogColor;
	public Color DuskFogColor;
	public Color MorningFogColor;
	public Color MiddayFogColor;

	public Color NightAmbientLight;
	public Color DuskAmbientLight;
	public Color MorningAmbientLight;
	public Color MiddayAmbientLight;

	public Color NightTint;
	public Color DuskTint;
	public Color MorningTint;
	public Color MiddayTint;

	public Color SunNight;
	public Color SunDay;

	public Material SkyBoxMaterial1;
	public Material SkyBoxMaterial2;

	private float Tod;
	private bool dayChanged = false;

	//Draw on GUI
	void OnGUI()
	{
		if (slider >= 1.0f) {
			slider = 0f;
			dayChanged = true;
		}

		slider = GUI.HorizontalSlider(new Rect(20, 20, 200, 30), slider, 0, 1.0f);

		Hour = slider * DAY_HOUR;
		Tod = slider2 * DAY_HOUR;

		sun.transform.localEulerAngles = new Vector3((slider * 360) - 90, 0, 0);
		slider = slider + Time.deltaTime / speed;
		sun.color = Color.Lerp(SunNight, SunDay, slider * 2);

		if (slider < 0.5f) {
			slider2 = slider;
		} else {
			slider2 = 1 - slider;
		}

		if (slider >= MorningSliderPercent && dayChanged) {
			Days += 1;
			dayChanged = false;
		}

		sun.intensity = (slider2 - 0.2f) * 1.7f;

		if(Tod < 4) {
			//it is Night
			RenderSettings.skybox = SkyBoxMaterial1;
			RenderSettings.skybox.SetFloat("_Blend", 0);
			SkyBoxMaterial1.SetColor ("_Tint", NightTint);
			RenderSettings.ambientLight = NightAmbientLight;
			RenderSettings.fogColor = NightFogColor;
		}

		if (Tod > 4 && Tod < 6) {
			RenderSettings.skybox = SkyBoxMaterial1;
			RenderSettings.skybox.SetFloat("_Blend", 0);
			RenderSettings.skybox.SetFloat("_Blend", (Tod / 2) - 2);
			SkyBoxMaterial1.SetColor ("_Tint", Color.Lerp (NightTint, DuskTint, (Tod / 2) - 2));
			RenderSettings.ambientLight = Color.Lerp (NightAmbientLight, DuskAmbientLight, (Tod / 2) - 2);
			RenderSettings.fogColor = Color.Lerp (NightFogColor,DuskFogColor, (Tod / 2) - 2);
			//it is Dusk
		}

		if (Tod > 6 && Tod < 8) {
			RenderSettings.skybox = SkyBoxMaterial2;
			RenderSettings.skybox.SetFloat("_Blend", 0);
			RenderSettings.skybox.SetFloat("_Blend", (Tod / 2) - 3);
			SkyBoxMaterial2.SetColor ("_Tint", Color.Lerp(DuskTint, MorningTint, (Tod / 2) - 3));
			RenderSettings.ambientLight = Color.Lerp(DuskAmbientLight, MorningAmbientLight, (Tod / 2) - 3);
			RenderSettings.fogColor = Color.Lerp (DuskFogColor,MorningFogColor, (Tod / 2) - 3);
			//it is Morning
		}

		if(Tod > 8 && Tod < 10) {
			RenderSettings.ambientLight = MiddayAmbientLight;
			RenderSettings.skybox = SkyBoxMaterial2;
			RenderSettings.skybox.SetFloat("_Blend", 1);
			SkyBoxMaterial2.SetColor ("_Tint", Color.Lerp(MorningTint,MiddayTint, (Tod / 2) - 4));
			RenderSettings.ambientLight = Color.Lerp (MorningAmbientLight, MiddayAmbientLight, (Tod / 2) - 4);
			RenderSettings.fogColor = Color.Lerp (MorningFogColor, MiddayFogColor, (Tod / 2) - 4);
		}
	}
}
