﻿using UnityEngine;
using System.Collections;

public enum MaterialType
{
    Brown = 0,
    Black,
    White,
    Grey
}

public class StartSceneController : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector

    public GameObject buttonLabel;
    public GameObject selectLabel;
    public GameObject catMesh;
    public UILabel pingLabel;
    public UIInput nameInput;
    public UIButton loginButton;

    #endregion

    const int RotateYSpeed = 5;

    float yRotate;
    Transform catTransform;
    MaterialType matType;
    SkinnedMeshRenderer meshRenderer;

    // Use this for initialization
    void Start()
    {
        if (buttonLabel == null || selectLabel == null || pingLabel == null ||
            loginButton ==  null || nameInput == null) {
            Debug.LogError("Fail to find button/select/ping/name label or login button.");
            return;
        }

        if (catMesh == null) {
            Debug.LogError("Fail to find cat mesh.");
            return;
        }

        matType = MaterialType.Brown;
        meshRenderer = catMesh.GetComponent<SkinnedMeshRenderer>();

        yRotate = this.transform.rotation.eulerAngles.y;
        // Use cached transform
        catTransform = this.transform;

        // Connect to photon cloud server
        if (NetworkManager.Instance.ConnectUsingSettings("0.1")) {
            StartCoroutine(RefreshPhotonServerPing());
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Rotate around Y axis when drag with left mouse button
        if (Input.GetMouseButton(0)) {
            yRotate = (yRotate - Input.GetAxis("Mouse X") * RotateYSpeed) % 360;
            catTransform.rotation = Quaternion.Euler(0, yRotate, 0);
        }
    }

    IEnumerator RefreshPhotonServerPing()
    {
        while (true) {
            pingLabel.text = PhotonNetwork.networkingPeer.RoundTripTime.ToString() + " ms";
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void OnFailedToConnectToPhoton()
    {
        pingLabel.text = "N/A";
        StopCoroutine(RefreshPhotonServerPing());
        loginButton.isEnabled = false;
    }

    public void BrownMaterial()
    {
        if (matType == MaterialType.Brown) {
            return;
        }

        meshRenderer.material = Resources.Load("Materials/cu_cat_col_low") as Material;
        matType = MaterialType.Brown;
    }

    public void BlackMaterial()
    {
        if (matType == MaterialType.Black) {
            return;
        }

        meshRenderer.material = Resources.Load("Materials/cu_cat_col_low1") as Material;
        matType = MaterialType.Black;
    }

    public void WhiteMaterial()
    {
        if (matType == MaterialType.White) {
            return;
        }

        meshRenderer.material = Resources.Load("Materials/cu_cat_col_low2") as Material;
        matType = MaterialType.White;
    }

    public void GreyMaterial()
    {
        if (matType == MaterialType.Grey) {
            return;
        }

        meshRenderer.material = Resources.Load("Materials/cu_cat_col_low3") as Material;
        matType = MaterialType.Grey;
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(nameInput.value)) {
            nameInput.GetComponent<TweenColor>().PlayForward();
            return;
        }

        // Store name and material type first
        PlayerPrefs.SetString("name", nameInput.value);
        PlayerPrefs.SetInt("type", (int)matType);

        ChatHandler.Instance.ConnectChatServer(nameInput.value);
        GameSceneManager.Instance.Login();
    }

    public void Play()
    {
        buttonLabel.SetActive(false);
        selectLabel.SetActive(true);
    }
}
