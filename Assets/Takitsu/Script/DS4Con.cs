using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class DS4Con : MonoBehaviour
{
	UdpClient client;
	Thread receiveThread;
	public Vector3 accelData;

	void Start()
	{
		client = new UdpClient(26760); // DS4WindowsÇÃUDPÉ|Å[Ég
		receiveThread = new Thread(ReceiveData);
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	void ReceiveData()
	{
		while (true)
		{
			try
			{
				IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref remote);
				string dataStr = Encoding.UTF8.GetString(data);

				// ó·: "A:-0.01,0.98,-0.05"
				if (dataStr.StartsWith("A:"))
				{
					string[] parts = dataStr.Substring(2).Split(',');
					if (parts.Length == 3)
					{
						float.TryParse(parts[0], out accelData.x);
						float.TryParse(parts[1], out accelData.y);
						float.TryParse(parts[2], out accelData.z);
					}
				}
			}
			catch (System.Exception e)
			{
				Debug.Log("UDP Error: " + e.Message);
			}
		}
	}

	void OnApplicationQuit()
	{
		receiveThread?.Abort();
		client?.Close();
	}
}
