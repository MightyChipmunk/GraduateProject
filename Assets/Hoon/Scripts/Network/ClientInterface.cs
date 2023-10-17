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
    TcpClient socketConnection;
    NetworkStream stream;

    public bool isReady;

    public void Start()
    {
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        if (socketConnection != null) return;

        try
        {
            socketConnection = new TcpClient("15.164.245.14", 7777);
            Debug.Log("서버 연결 완료");
            isReady = true;

            stream = socketConnection.GetStream();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    /// Send message to server using socket connection.     
    public void SendMessage(byte[] buffer)
    {
        if (socketConnection == null && stream == null)
        {
            Debug.Log("소켓 연결 없음");
            return;
        }
        try
        {
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.                 
                stream.Write(buffer, 0, buffer.Length);// Receive the TcpServer.response.
                Debug.Log("데이터 전송 완료");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public string RecvMessage()
    {
        if (socketConnection == null || stream == null)
        {
            return null;
        }
        try
        {
            if (stream.CanRead && stream.DataAvailable)
            {
                // Buffer to store the response bytes.
                byte[] data = new byte[4096];

                // String to store the response ASCII representation.
                string responseData = string.Empty;

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(data, 0, data.Length);
                byte[] strByte = new byte[4096];
                Array.Copy(data, 6, strByte, 0, bytes);
                responseData = Encoding.Unicode.GetString(strByte, 0, bytes - 6);

                if (responseData.Length > 0)
                {
                    Debug.Log("Received: " + responseData);
                    return responseData;
                }
                else
                    return null;
            }
            else
                return null;
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
            return null;
        }
    }

    public void CloseAll()
    {
        socketConnection.Close();
        stream.Close();
    }
}