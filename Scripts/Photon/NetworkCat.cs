using UnityEngine;
using System.Collections;

public class NetworkCat : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();

        // Default send data rate
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine) {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext((float)(animator.GetFloat("Speed")));
            
        } else {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            animator.SetFloat("Speed", (float)stream.ReceiveNext());
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // Change the player object's material and name depending on
        // instantiationData data[], which was sent by PhotonNetwork.Instantiate().
        object[] data = photonView.instantiationData;
        string name = (string)data[0];
        int type = (int)data[1];
        
        this.GetComponent<PlayerStatus>().SetMeshType(type);
        this.GetComponent<PlayerStatus>().name = name;

        if (!photonView.isMine) {
            Messenger<GameObject>.Invoke(MyEventType.OTHER_PLAYER_JOIN, this.gameObject);
        } else {
            // Set our own player name
            photonView.owner.name = name;
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("Player leave:" + other.ID + ", name:" + other.name);
        Messenger<int>.Invoke(MyEventType.OTHER_PLAYER_LEAVE, other.ID);
    }
}
