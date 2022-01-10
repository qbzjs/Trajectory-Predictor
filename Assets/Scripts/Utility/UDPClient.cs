using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

/// <summary>
/// Ports need reinitialised when changed in settings!!!
/// Restart unity game
/// TODO -- MAKE DYNAMIC INITIALISER
/// 
/// ip - '127.0.0.1' is for sending data within local machine
/// ip - '192.168.1.10' example for sending data to machine on local network
/// </summary>
/// 
public class UDPClient : MonoBehaviour
{
    public static UDPClient instance;

    // public bool sendEnabled = true;
    // public bool receiveEnabled = true;
    
    public string ip = "127.0.0.1";
    public int portListen = 3002;
    public int portSend = 3010;

    private string received = "";
    
    [Space(8)]
    public bool receivedFlag = false;
    
    [Space(8)]
    public float x;
    public float y;
    public float z;

    private UdpClient client;
    private Thread receiveThread;
    private IPEndPoint remoteEndPoint;

    //EVENTS/DELAGATES TO SEND DATA ACROSS THE GAME
    public delegate void BCI_Data(float x, float y, float z);
    public static event BCI_Data OnBCI_Data;


    #region Initiaisation

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        //Check if the ip address entered is valid. If not, sendMessage will broadcast to all ip addresses 
        IPAddress ip;
        if (IPAddress.TryParse(this.ip, out ip))
        {
            remoteEndPoint = new IPEndPoint(ip, portSend);
        }
        else
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, portSend);
        }

        //Initialize client and thread for receiving

        client = new UdpClient(portListen);

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    #endregion

    void Update()
    {
        //Debug.Log(received.ToString());

        //Check if a message has been received and broadcast data if true
        if (received != ""){
            //Debug.Log("UDPClient: message received \'" + received + "\'");
            receivedFlag = true; 
            // received = ""; //makes it flicker on/off - need to buffer frames or add timeout
        }
        // else {
        //     receivedFlag = false;
        // }
        
        if (receivedFlag){
            if (OnBCI_Data != null){
                OnBCI_Data(x,y,z);
            }
        }
    }

    #region Data Sender
    // UDP send: one byte
    public void SendData(byte inputByte)
    {
        //Debug.Log("valueToSend");
        // if (sendEnabled){
            try{
                //Prepare byte-data array
                byte[] data = new byte[1];
                data[0] = inputByte;

                // Send bytes to remote client
                client.Send(data, data.Length, remoteEndPoint);
            }
            catch (Exception err){
                Debug.LogError("Error udp send : " + err.Message);
            }
        // }
    }
    
    //Call this method to send a message from this app to ipSend using portSend
    public void SendData (string valueToSend)
    {
        try {
            if (valueToSend != "") {

                //Get bytes from string
                byte[] data = Encoding.UTF8.GetBytes (valueToSend);

                // Send bytes to remote client
                client.Send (data, data.Length, remoteEndPoint);
                Debug.Log ("UDPClient: send \'" + valueToSend + "\'");
                //Clear message
                valueToSend = "";
            }
        } 
        catch (Exception err) {
            Debug.LogError ("Error udp send : " + err.Message);
        }
    }
    #endregion

    #region Data Receiver
    public void ReceiveData()
    {
        while (true){
            
            //data conversion to x y z
            IPEndPoint receiveIP = new IPEndPoint (IPAddress.Any, portListen);
            
            try{
                byte[] byteArray = new byte[]{}; //length is 24
                //Debug.Log(byteArray.Length); //length check ro console
                
                byteArray = client.Receive(ref receiveIP); //propagate the byte array from the ip endpoint
                
                //convert the array to x,y,z
                int i = 0;
                x = Convert.ToSingle(BitConverter.ToDouble(byteArray, 8 * i));

                i++;
                y = Convert.ToSingle(BitConverter.ToDouble(byteArray, 8 * i));
                
                i++;
                z = Convert.ToSingle(BitConverter.ToDouble(byteArray, 8 * i));
            }
            catch (Exception e){
                Debug.Log ("Error in UDP Receiver:" + e.ToString ());
            }
            
            //Unconverted data (used to check data is received)
            try {
                byte[] data = client.Receive (ref receiveIP);
                // Bytes into text
                string text = "";
                text = Encoding.UTF8.GetString (data);
                received = text;
            } 
            catch (Exception err) {
                Debug.Log ("Error:" + err.ToString ());
            }
            
        }
    }
    #endregion


    //NEW FUNCTION - CONVERT DATA TEST!!!
    private float ConvertByteData(byte[] b){
        float d = BitConverter.ToInt16(b, 0);
        return d;
    }

    //Exit UDP client
    public void OnDisable()
    {
        if (receiveThread != null){
            receiveThread.Abort();
            receiveThread = null;
        }
        client.Close();
        Debug.Log("UDPClient: exit");
    }
    
    public void OnApplicationQuit()
    {
        if (receiveThread != null){
            receiveThread.Abort();
            receiveThread = null;
        }
        client.Close();
        Debug.Log("UDPClient: exit");
    }
}