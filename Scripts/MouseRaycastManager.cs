using UnityEngine;
using System.Collections;

public enum HitObjectType
{
    NPC,
    InteractiveItem,
    Default
}

public delegate void HitTypeChangedHandler();

public class MouseRaycastManager : MonoBehaviour
{
    private static MouseRaycastManager instance;

    public static MouseRaycastManager Instance {
        get {
            if (instance == null) {
                Debug.LogError("Fail to get RaycastHitManager instance.");
            }

            return instance;
        }
    }

    [HideInInspector]
    public HitObjectType hitObjectType;
    [HideInInspector]
    public GameObject hitObject;
    [HideInInspector]
    public float hitDistance;

    private HitObjectType formerType;

    public event HitTypeChangedHandler HitTypeChanged = delegate {};

    void Awake()
    {
        if (instance != null) {
            Debug.LogError("Only one instance of RaycastHitManager is allowed.");
        } else {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.hitObjectType = HitObjectType.Default;
        this.formerType = HitObjectType.Default;
        this.hitObject = null;
        this.hitDistance = -1;
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckHitTypeChange();

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
        if (hit) {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject && hitObject.transform.parent) {
                GameObject parentObject = hitObject.transform.parent.gameObject;
                if (parentObject) {
                    this.hitObject = hitObject;
                    this.hitDistance = hitInfo.distance;

                    if (parentObject.tag == "NPC") {
                        this.hitObjectType = HitObjectType.NPC;
                        return;
                    } else if (parentObject.tag == "Interactive Item") {
                        this.hitObjectType = HitObjectType.InteractiveItem;
                        return;
                    }
                }
            }
        }

        this.hitObjectType = HitObjectType.Default;
        this.hitObject = null;
        this.hitDistance = -1;
    }

    private void CheckHitTypeChange()
    {
        if (this.hitObjectType != this.formerType) {
            // Hit object type changed
            HitTypeChanged();
            this.formerType = this.hitObjectType;
        }
    }
}
