using UnityEngine;
using System.Collections;

public class SendMessageCheck : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        FuncCallSpeedTest t = this.GetComponent<FuncCallSpeedTest>();
        t.del += DelegateCall;
    }

    void DelegateCall()
    {
        //Debug.Log("DelegateCall");
    }

    void SendMessageCall()
    {
        //Debug.Log("SendMessageCall");
    }

    public void DirectCall()
    {
        //Debug.Log("DirectCall");
    } 
}
