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
    }

    public void DisplayTipMessage(string msg)
    {
        tipLabel.text = msg;
        tipLabel.GetComponent<TweenAlpha>().PlayForward();
    }
}
