using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{

    public enum HealthState
    {
        Healthy,
        Uncomfortable,
        Sick
    };

    public float life { get; set; }

    public float hungry { get; set; }

    public float health { get; set; }
    
    public bool isDead { get; set; }
    
    private float dayNightSpeed = 0;
    private DayNightCycle timeCycle;
    private const float MaxNum = 100.0f;
    private const float MaxHungryDay = 3.0f;
    // Use this for initialization
    void Start()
    {
        life = MaxNum;
        hungry = MaxNum;
        health = MaxNum;

        timeCycle = GameObject.Find("TOD").GetComponent<DayNightCycle>();
        if (timeCycle == null) {
            Debug.LogError("Fail to find TOD's DayNightCycle component");
        }

        dayNightSpeed = MaxNum * Time.deltaTime / (timeCycle.speed * (MaxHungryDay - 1));
    }
    
    // Update is called once per frame
    void Update()
    {
        hungry = hungry - dayNightSpeed;
        
        if (hungry <= 0) {
            hungry = 0;
            life = life - dayNightSpeed;
        }
        
        if (life <= 0) {
            life = 0;
            isDead = true;
        } else {
            isDead = false;
        }
    }

    #region public method

    public float GetLifePercent()
    {
        return (life / MaxNum);
    }
    
    public float GetHungryPercent()
    {
        return (hungry / MaxNum);
    }
    
    public float GetHealthPercent()
    {
        return (health / MaxNum);
    }
    
    public HealthState GetHealthState()
    {
        HealthState stat;
        if (health < 20.0f) {
            stat = HealthState.Sick;
        } else if (health < 50.0f) {
            stat = HealthState.Uncomfortable;
        } else {
            stat = HealthState.Healthy;
        }
        
        return stat;
    }

    #endregion

}
