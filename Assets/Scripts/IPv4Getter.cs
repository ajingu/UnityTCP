using System.Net.NetworkInformation;
using System.Net.Sockets;

public class IPv4Getter
{
    public static string GetIP(string adapterName)
    {
        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.Name == adapterName && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        output = ip.Address.ToString();
                        UnityEngine.Debug.LogFormat("Name: {0}, Address: {1}", item.Name, output);
                    }
                }
            }
        }
        return output;
    }
}

