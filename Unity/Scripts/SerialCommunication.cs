using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System;
using System.Text;

/// <summary>
/// System.IO.Ports requires .NET 4 (see Project Settings -> Player). In Unity 2020, I have to also
/// update the Build to x86_64 and then back (not sure why I need to go back).
/// 
/// Following this tutorial: https://sirwilliam.hashnode.dev/serial-communication-between-arduino-and-unity-3d
/// See .NET docs for more on serial ports: https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-7.0
/// </summary


public class SerialCommunication : MonoBehaviour
{
    public Dropdown PortsDropdown;
    public GameObject player;

    private SerialPort _serial;
    private List<string> _ports;

    public static float yaw;
    public static float pitch;
    public static float roll;

    public static int encoderValue;
    public static int isButtonUp;

    void Start()
    {
        RefreshPortsDropdown();
        // ConnectToPort(); 
        yaw = 0;
        pitch = 0;
        roll = 0;
        isButtonUp = 1;
    }

    private void Update()
    {
        if (_serial != null && _serial.IsOpen)
        {
            Ping();
            string[] data = _serial.ReadLine().Split(',');
            if (data.Length == 3)
            {
                yaw = float.Parse(data[0]);
                pitch = float.Parse(data[1]);
                roll = float.Parse(data[2]);
                Debug.Log(string.Format("{0}, {1}, {2}", yaw, pitch, roll));
            } else if (data.Length == 2)
            {
                isButtonUp = int.Parse(data[1]);
                encoderValue = int.Parse(data[0]);
                // Debug.Log(string.Format("{0}, {1}", isButtonUp, encoderValue));
            }

            
        }
    }

    public void RefreshPortsDropdown()
    {
        // Remove all the previous options
        PortsDropdown.ClearOptions();

        // Get port names
        string[] portNames = SerialPort.GetPortNames();
        _ports = portNames.ToList();

        // Add the port names to our options
        PortsDropdown.AddOptions(_ports);
    }

    public void ConnectToPort()
    {
        /*
         * If not automatic, then reactivate PortDropdown in hierarchy.
         */

        string port = _ports[PortsDropdown.value];
        // string port = "COM7";

        try
        {
            _serial = new SerialPort(port, 9600)
            {
                Encoding = System.Text.Encoding.ASCII,
                DtrEnable = true // allows us to read responses back from Arduino
            };

            _serial.Open();
            Debug.Log($"Connected to {port}");
            PortsDropdown.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Disconnect()
    {
        if (_serial != null)
        {
            if (_serial.IsOpen) _serial.Close();

            _serial.Dispose();
            _serial = null;

            Debug.Log("Disconnected");
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    public void Ping()
    {
        byte[] outBuffer = new byte[1];
        outBuffer[0] = 0;
        _serial.Write(outBuffer, 0, 1); // buffer, offset, count(?)
    }


}
