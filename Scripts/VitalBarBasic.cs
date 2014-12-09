using UnityEngine;
using System.Collections;

public class VitalBarBasic : MonoBehaviour
{

    // Assign NGUI UI Slider in Inspector.
    public UISlider lifeSlider;
    public UISlider hungrySlider;

    private PlayerStatus playerStat;

    void Start()
    {
        if (lifeSlider == null || hungrySlider == null) {
            Debug.LogError("Fail to get component UISlider");
            return;
        }
    }

    void OnEnable()
    {
        Messenger<GameObject>.AddListener(MyEventType.PLAYER_BORN, OnPlayerBorn);
    }

    void OnDisable()
    {
        Messenger<GameObject>.RemoveListener(MyEventType.PLAYER_BORN, OnPlayerBorn);
    }

    void OnPlayerBorn(GameObject player)
    {
        playerStat = player.GetComponent<PlayerStatus>();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (playerStat == null) {
            return;
        }
        
        lifeSlider.value = playerStat.GetLifePercent();
        hungrySlider.value = playerStat.GetHungryPercent();
    }
}
