using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClientInterface
{
    private TcpClient socketConnection;

    public void Start()
    {
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            socketConnection = new TcpClient("192.168.0.30", 7777);
            Debug.Log("서버 연결 완료"); 
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    /// Send message to server using socket connection.     
    public void SendMessage(Byte[] buffer)
    {
        if (socketConnection == null)
        {
            Debug.Log("소켓 연결 없음");
            return;
        }
        try
        {
            Debug.Log("스트림 가져오기");
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            if (stream.CanWrite)
            {
                Debug.Log("스트림 작성 시도");
                // Write byte array to socketConnection stream.                 
                stream.Write(buffer, 0, buffer.Length);// Receive the TcpServer.response.

                // Buffer to store the response bytes.
                Byte[] data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Debug.Log("Received: " + responseData);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}