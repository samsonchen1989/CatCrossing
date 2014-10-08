using UnityEngine;
using System.Collections;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private float barDisplay = 0.0f;
    [SerializeField]
    private Texture2D emptyHealthTex;
    [SerializeField]
    private Texture2D fullHealthTex;
    [SerializeField]
    private Texture2D emptyHungryTex;
    [SerializeField]
    private Texture2D fullHungryTex;
    [SerializeField]
    private GUIStyle emptyStyle;
    [SerializeField]
    private GUIStyle fullStyle;

    private PlayerStatus playerStatus;

    private Vector2 healthBarPos = new Vector2(0.04f, 0.07f);
    private Vector2 size = new Vector2(0.12f, 0.04f);
    private float posX;
    private float posY;
    private float sizeX;
    private float sizeY;

    // Use this for initialization
    void Start()
    {
        playerStatus = this.GetComponent<PlayerStatus>();
        if (playerStatus == null) {
            Debug.LogError("Fail to get PlayerStatus Component.");
        }

        posX = healthBarPos.x * Screen.width;
        posY = healthBarPos.y * Screen.height;

        sizeX = size.x * Screen.width;
        sizeY = size.y * Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        barDisplay = Time.time * 0.05f;
    }

    void OnGUI()
    {
        //draw the background
        GUI.BeginGroup(new Rect(posX, posY, sizeX, sizeY));
        GUI.Box(new Rect(0, 0, sizeX, sizeY), emptyHealthTex, emptyStyle);
        GUI.BeginGroup(new Rect(0, 0, sizeX * playerStatus.GetLifePercent(), sizeY));
        GUI.Box(new Rect(0, 0, sizeX, sizeY), fullHealthTex, fullStyle);
        GUI.EndGroup();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(posX, posY * 1.6f, sizeX, sizeY));
        GUI.Box(new Rect(0, 0, sizeX, sizeY), emptyHungryTex, emptyStyle);
        GUI.BeginGroup(new Rect(0, 0, sizeX * playerStatus.GetHungryPercent(), sizeY));
        GUI.Box(new Rect(0, 0, sizeX, sizeY), fullHungryTex, fullStyle);
        GUI.EndGroup();
        GUI.EndGroup();	
    }
}
