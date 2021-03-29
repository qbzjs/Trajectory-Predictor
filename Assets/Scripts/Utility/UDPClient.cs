using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

    public string ip = "127.0.0.1";
    public int portListen = 3002;
    public int portSend = 3010;

    private string received = "";
    
    private bool receivedFlag = false;
    
    [Space(12)]
    public Vector2 xy;
    public Vector2 xyLR;
    public Vector2 xyFR;
    public Vector2 xyLF;
    public float LR;
    public float FR;
    public float LF;
    public float TX;

    private UdpClient client;
    private Thread receiveThread;
    private IPEndPoint remoteEndPoint;

    //EVENTS/DELAGATES TO SEND DATA ACROSS THE GAME
    public delegate void dataReceivedString(string dataString);
    public static event dataReceivedString OnDataReceivedString;

    public delegate void dataReceivedDoubles(float LR, float FR, float LF, float TX);
    public static event dataReceivedDoubles OnDataReceivedDoubles;

    public delegate void dataReceivedXY(Vector2 xy, Vector2 xy_LR, Vector2 xy_FR, Vector2 xy_LF);
    public static event dataReceivedXY OnDataReceivedXY;

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

    void Update()
    {
        //Check if a data has been recived
        // if (receivedFlag)
        // {
        //     if (OnDataReceivedXY != null){
        //         OnDataReceivedXY(xy, xyLR, xyFR, xyLF);
        //     }
        //
        //     if (OnDataReceivedDoubles != null){
        //         OnDataReceivedDoubles(LR, FR, LF, TX);
        //     }
        // }
        
        //Check if a message has been received
        if (received != ""){
            Debug.Log("UDPClient: message received \'" + received + "\'");
            //Clear message
            received = "";
        }
    }

    // UDP send: one byte
    public void SendData(byte inputByte)
    {
        try
        {
            //Prepare byte-data array
            byte[] data = new byte[1];
            data[0] = inputByte;

//            Debug.Log("UDP Send : " + data);

            // Send bytes to remote client
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.LogError("Error udp send : " + err.Message);
        }
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
    public void ReceiveData()
    {
        while (true){
            //----------------------
            try {
                // Bytes received
                IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
                byte[] data = client.Receive (ref anyIP);

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
}