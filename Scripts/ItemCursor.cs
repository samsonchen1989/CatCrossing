using UnityEngine;
using System.Collections;

public class ItemCursor : MonoBehaviour {

    #region Inspector Assign component

    public UILabel countLabel;
    public Camera uiCamera;
    public UIAtlas atlas;

    #endregion

    #region Singleton code

    private static ItemCursor instance;
    public static ItemCursor Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to get ItemCursor instance.");
            }

            return instance;
        }
    }

    #endregion

    UISprite itemSprite;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one instance of ItemCursor is allowed.");
            return;
        }
    }

    void onDestroy()
    {
        instance = null;
    }

	// Use this for initialization
	void Start()
    {
        itemSprite = this.GetComponent<UISprite>();
        if (itemSprite == null) {
            Debug.LogError("Fail to get item sprite.");
        }

        if (countLabel == null) {
            Debug.LogError("Fail to get item cursor's count label.");
        }

        if (itemSprite.depth < 100) {
            itemSprite.depth = 100;
        }

        // Always set count label in front of sprite
        countLabel.depth = itemSprite.depth + 1;
        
        instance.itemSprite.atlas = null;
        instance.itemSprite.spriteName = null;
        instance.countLabel.text = null;

        if (uiCamera == null) {
            uiCamera = NGUITools.FindCameraForLayer(this.gameObject.layer);
        }
	}
	
	// Update is called once per frame
	void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (uiCamera != null) {
            mousePosition.x = Mathf.Clamp01(mousePosition.x / Screen.width);
            mousePosition.y = Mathf.Clamp01(mousePosition.y / Screen.height);
            this.transform.position = uiCamera.ViewportToWorldPoint(mousePosition);

            // For pixel-perfect results
            if (uiCamera.isOrthoGraphic)
            {
                Vector3 lp = this.transform.localPosition;
                lp.x = Mathf.Round(lp.x);
                lp.y = Mathf.Round(lp.y);
                this.transform.localPosition = lp;
            }
        }
	}

    public static void UpdateSprite()
    {
        if (instance.itemSprite == null || instance.countLabel == null) {
            Debug.Log("ItemCursor's sprite or countLabel is null.");
            return;
        }

        ItemStack swapItemStack = Inventory.Instance.swapItemStack;
        if (swapItemStack != null) {
            instance.itemSprite.atlas = instance.atlas;
			instance.itemSprite.spriteName = swapItemStack.item.itemName;
			instance.countLabel.text = swapItemStack.num.ToString();
			instance.Update();
		} else {
            instance.itemSprite.atlas = null;
            instance.itemSprite.spriteName = null;
            instance.countLabel.text = null;
            instance.Update();
        }
    }

    public static void EnableSprite(bool setbool)
    {
        instance.enabled = setbool;
    }
}
