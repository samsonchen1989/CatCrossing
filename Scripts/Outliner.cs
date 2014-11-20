using UnityEngine;
using System.Collections;

public class Outliner : MonoBehaviour
{
    public Color outlineColor = new Color(1f, 1f, 0f, 1f);
    MeshRenderer meshRenderer;
    Shader outlineShader;
    
    // Use this for initialization
    void Start()
    {
        // Set the transparent material to this object
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null) {
            Debug.LogError("Fail to find outline object's meshRenderer.");
            return;
        }

        outlineShader = Shader.Find("Outline/Outline");

        if (outlineShader == null) {
            Debug.LogError("Fail to find outline shader.");
            return;
        }

        MouseRaycastManager.Instance.HitTypeChanged += OnMouseHitObjectChanged;
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        // Unregister event OnMouseHitObjectChanged before destroy itself
        if (MouseRaycastManager.Instance != null) {
            MouseRaycastManager.Instance.HitTypeChanged -= OnMouseHitObjectChanged;
        }
    }

    public void EnableOutline(bool enable)
    {
        if (meshRenderer == null || meshRenderer.materials == null) {
            return;
        }

        Material[] materials = meshRenderer.materials;
        if (enable) {
            materials[0].SetColor("_OutlineColor", outlineColor);
            materials[0].SetFloat("_Outline", 0.005f);
        } else {
            materials[0].SetFloat("_Outline", 0.0f);
        }

    }

    private void OnMouseHitObjectChanged()
    {
        if (MouseRaycastManager.Instance.hitObjectType == HitObjectType.InteractiveItem) {
            if (MouseRaycastManager.Instance.hitObject != null &&
                MouseRaycastManager.Instance.hitObject == gameObject) {
                // Mouse raycast hit this gameobject
                EnableOutline(true);
            }
        } else {
            EnableOutline(false);
        }
    }
}
