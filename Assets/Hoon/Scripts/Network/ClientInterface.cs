using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientInterface
{
    TcpClient socketConnection;
    NetworkStream stream;
    Thread thread;

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
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }

        Tcp_Client_Load();
    }

    /// Send message to server using socket connection.     
    public void SendMessage(byte[] buffer)
    {
        if (socketConnection == null)
        {
            Debug.Log("소켓 연결 없음");
            return;
        }
        try
        {
            stream = socketConnection.GetStream();
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

    //public string RecvMessage()
    //{
    //    if (socketConnection == null || stream == null)
    //    {
    //        return null;
    //    }
    //    try
    //    {
    //        stream = socketConnection.GetStream();
    //        if (stream.CanRead && stream.DataAvailable)
    //        {
    //            // Buffer to store the response bytes.
    //            byte[] data = new byte[4096];

    //            // String to store the response ASCII representation.
    //            string responseData = string.Empty;

    //            // Read the first batch of the TcpServer response bytes.
    //            int bytes = stream.Read(data, 0, data.Length);
    //            byte[] strByte = new byte[4096];
    //            Array.Copy(data, 6, strByte, 0, bytes);
    //            responseData = Encoding.Unicode.GetString(strByte, 0, bytes - 6);

    //            if (responseData.Length > 0)
    //            {
    //                Debug.Log("Received: " + responseData);
    //                return responseData;
    //            }
    //            else
    //                return null;
    //        }
    //        else
    //            return null;
    //    }
    //    catch (SocketException socketException)
    //    {
    //        Debug.Log("Socket exception: " + socketException);
    //        return null;
    //    }
    //}

    private void Tcp_Client_Load()
    {
        thread = new Thread(new ThreadStart(Receive));
        thread.IsBackground = true;
        thread.Start();
    }

    private void Receive()
    {
        while (true)
        {
            if (socketConnection.Connected)
            {
                try
                {
                    NetworkStream stream = socketConnection.GetStream();
                    byte[] buffer = new byte[4096];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes <= 0)
                    {
                        continue;
                    }

                    byte[] strByte = new byte[4096];
                    Array.Copy(buffer, 6, strByte, 0, bytes);
                    string responseData = Encoding.Unicode.GetString(strByte, 0, bytes - 6);
                    Debug.Log("Received " + responseData);
                    Call(responseData);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }
    }

    void Call(string data)
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            NetworkManager.Instance.Receive(data);
        });
    }

    public void CloseAll()
    {
        socketConnection.Close();
        stream.Close();
        thread.Abort();
    }
}