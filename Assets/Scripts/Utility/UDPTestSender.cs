using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// UDP client can send integers between 0-250 without additional encoding/packing
/// </summary>
/// 
public class UDPTestSender : MonoBehaviour
{
    public bool active = false;

    public TriggerType triggerType = TriggerType.Pulse; 
    
    public int valueToSend=0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SendUDP_byte(0, triggerType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendUDP_byte(1, triggerType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendUDP_byte(2, triggerType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SendUDP_byte(3, triggerType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SendUDP_byte(4, triggerType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SendUDP_byte(5, triggerType);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SendUDP_byte(valueToSend, triggerType);
        }

    }
    private void SendUDP_byte(int t, TriggerType tType)
    {
        if (active)
        {
            
            //UDPClient.instance.SendData(t.ToString());

            UDPClient.instance.SendData((byte)t);
            Debug.Log("Value to send : " + t + " :: Trigger Type:" + triggerType.ToString());
            if (triggerType == TriggerType.Pulse){
                t = 0;
                UDPClient.instance.SendData((byte)t);
            }
            
        }
    }
    private void SendUDP_byte(float t, TriggerType tType)
    {
        if (active)
        {
            
            //UDPClient.instance.SendData(t.ToString());
    
            UDPClient.instance.SendData((byte)t);
            Debug.Log("Value to send : " + t + " :: Trigger Type:" + triggerType.ToString());
            if (triggerType == TriggerType.Pulse){
                t = 0;
                UDPClient.instance.SendData((byte)t);
            }
        }
    }
}
