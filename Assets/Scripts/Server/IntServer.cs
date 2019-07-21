public class IntServer : Server
{
    protected override void OnMessage(string msg)
    {
        base.OnMessage(msg);

        int num;

        if (int.TryParse(msg, out num))
        {
            SendMessageToClient("Accept:" + num + "\n");
        }
        else
        {
            SendMessageToClient("Error\n");
        }
    }
}

