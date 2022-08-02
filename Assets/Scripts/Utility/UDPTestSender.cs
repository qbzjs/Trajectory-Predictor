using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPTestSender : MonoBehaviour
{
    public bool active = false;

    public int valueToSend=0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SendUDP_byte(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendUDP_byte(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendUDP_byte(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SendUDP_byte(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SendUDP_byte(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SendUDP_byte(5);
        }

        {
            if (Input.GetKeyDown(KeyCode.Alpha9)){
                SendUDP_byte(valueToSend);
                SendUDP_byte(0);
            }
        }


    }
    private void SendUDP_byte(int t)
    {
        if (active)
        {
            
            //UDPClient.instance.SendData(t.ToString());

            UDPClient.instance.SendData((byte)t);
            Debug.Log("Value to send : " + t);
        }
    }
}
