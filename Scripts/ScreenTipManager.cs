using UnityEngine;
using System.Collections;

public class ScreenTipManager : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector

    public UILabel tipLabel;

    #endregion

    private static ScreenTipManager instance;
    public static ScreenTipManager Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to get ScreenTipManager Instance.");
            }

            return instance;
        }
    }

    bool startFade;

    void Awake()
    {
        if (instance != null) {
            Debug.LogError("Only one instance of ScreenTipManager is allowed.");
            return;
        } else {
            instance = this;
        }
    }

    void Start()
    {
        if (tipLabel == null) {
            Debug.LogError("Fail to find screen tip label.");
            return;
        }

        startFade = false;
    }

    void Update()
    {
        if (startFade) {

            // 2s to fade away.
            tipLabel.alpha = Mathf.Max(0.0f, tipLabel.alpha - Time.deltaTime / 2);

            if (tipLabel.alpha == 0) {
                startFade = false;
                //Debug.Log("Time:" + (Time.timeSinceLevelLoad - time).ToString());
            }
        }
    }

    public void DisplayTipMessage(string msg)
    {
        tipLabel.text = msg;
        tipLabel.alpha = 1.0f;
        startFade = true;
    }
}
