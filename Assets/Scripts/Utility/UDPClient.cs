using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPClient : MonoBehaviour
{
    public static UDPClient instance;

    public string ip = "";
    public int portListen = 5555;
    public int portSend = 4444;

    [Space(10)] private bool receivedFlag = false;
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
        if (receivedFlag)
        {
            if (OnDataReceivedXY != null)
            {
                OnDataReceivedXY(xy, xyLR, xyFR, xyLF);
                //Debug.Log("t1");
            }

            if (OnDataReceivedDoubles != null)
            {
                OnDataReceivedDoubles(LR, FR, LF, TX);
                //Debug.Log("t2");
            }
        }
    }

    // UDP send: one byte
    public void UdpSend_byte(byte input_byte)
    {
        try
        {
            //Prepare byte-data array
            byte[] data = new byte[1];
            data[0] = input_byte;

            // Send bytes to remote client
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.LogError("Error udp send : " + err.Message);
        }
    }

    public void ReceiveData()
    {
        IPEndPoint receiveIP = new IPEndPoint(IPAddress.Any, 3002);
        while (true)
        {
            // bytes: 4x8 = 32bytes (4 Doubles: each 8bytes long)
            byte[] _w_byteArray =
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            try
            {
                _w_byteArray = client.Receive(ref receiveIP);
                //Debug.Log("_w_byteArray.Length:" + _w_byteArray.Length);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                Debug.Log("Error:" + e.ToString());
            }

            //CONVERT BYTE_ARRAY OF DOUBLES TO FLOATS

            int i = 0;
            LR = Convert.ToSingle(BitConverter.ToDouble(_w_byteArray, 8 * i));
            //Debug.Log("LR: " + Convert.ToString(LR));

            i++;
            FR = Convert.ToSingle(BitConverter.ToDouble(_w_byteArray, 8 * i));
            //Debug.Log("FR: " + Convert.ToString(FR));

            i++;
            LF = Convert.ToSingle(BitConverter.ToDouble(_w_byteArray, 8 * i));
            //Debug.Log("LF: " + Convert.ToString(LF));

            i++;
            TX = Convert.ToSingle(BitConverter.ToDouble(_w_byteArray, 8 * i));
            //Debug.Log("TX: " + Convert.ToString(TX));

            //CONVERT AND RETURN XY USING INSTANCED CLASS
            XY_Conversion xyConversion = new XY_Conversion();
            xy = xyConversion.ConvertToXY(LR, FR, LF);
            xyLR = xyConversion.ConvertToXY_LR(LR);
            xyFR = xyConversion.ConvertToXY_FR(FR);
            xyLF = xyConversion.ConvertToXY_LF(LF);
            //Debug.Log("xy.x: " + xy.x);
            //Debug.Log("xy.y: " + xy.y);

            // DATA RECEIVING DONE
            receivedFlag = true;
        }
    }

    //Exit UDP client
    public void OnDisable()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }

        client.Close();
        Debug.Log("UDPClient: exit");
    }
}