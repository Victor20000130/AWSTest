using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;

public class UDPTest : MonoBehaviour
{
    private UdpClient udpClient;
    public TextMeshProUGUI text;
    public TMP_InputField inputfield;
    public string serverIP;
    public int serverPort;
    public int clientPort;

    private bool isRunning;

    private void Awake()
    {
        inputfield.onEndEdit.AddListener(Send);
    }

    private async void Start()
    {
        try
        {
            udpClient = new UdpClient(clientPort);
            isRunning = true;
            while (isRunning)
            {
                UdpReceiveResult result = await udpClient.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);
                IPEndPoint serverEndPoint = result.RemoteEndPoint;
                text.text += $"\n{serverEndPoint.Address}_{serverEndPoint.Port} message\n"
                            + $"{message} \n";
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void Send(string message)
    {
        byte[] messageByte = Encoding.UTF8.GetBytes(message);
        inputfield.interactable = false;
        inputfield.text = "";
        await udpClient.SendAsync(messageByte, messageByte.Length, serverIP, serverPort);
        inputfield.interactable = true;
    }

    private void OnDisable()
    {
        udpClient.Close();
        isRunning = false;
    }
}
