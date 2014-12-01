using UnityEngine;
using System.Collections;

public class VitalBarBasic : MonoBehaviour
{

    // Assign NGUI UI Slider in Inspector.
    public UISlider lifeSlider;
    public UISlider hungrySlider;

    private PlayerStatus playerStat;

    void Awake()
    {
        if (lifeSlider == null || hungrySlider == null) {
            Debug.LogError("Fail to get component UISlider");
            return;
        }

        playerStat = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        if (playerStat == null) {
            Debug.LogError("Fail to get PlayerSTatus.");
            return;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        lifeSlider.value = playerStat.GetLifePercent();
        hungrySlider.value = playerStat.GetHungryPercent();
    }
}
