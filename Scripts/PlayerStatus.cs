using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public enum HealthState {
		Healthy = 0,
		Uncomfortable,
		Sick
	};

	public float life {get; set;}
	public float hungry {get; set;}
	public float health {get; set;}
	
	public bool isDead {get; set;}
	
	private float dayNightSpeed = 0;
	private DayNightCycle timeCycle;
	
	private const float MAX_NUM = 100.0f;
	private const float MAX_HUNGRY_DAY = 3.0f;
	// Use this for initialization
	void Start () {
		life = MAX_NUM;
		hungry = MAX_NUM;
		health = MAX_NUM;

		timeCycle = GameObject.Find("TOD").GetComponent<DayNightCycle>();
		if (timeCycle == null) {
			Debug.LogError("Fail to find TOD's DayNightCycle component");
		}

		dayNightSpeed = MAX_NUM * Time.deltaTime / (timeCycle.speed * (MAX_HUNGRY_DAY - 1));
	}
	
	// Update is called once per frame
	void Update () {
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
	public float getLifePercent()
	{
		return (life / MAX_NUM);
	}
	
	public float getHungryPercent()
	{
		return (hungry / MAX_NUM);
	}
	
	public float getHealthPercent()
	{
		return (health / MAX_NUM);
	}
	
	public HealthState getHealthState()
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
