using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    // один общий экземпляр сервиса для всех клиентов
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            var user = new ServerUser()
            {
                Id = nextId++,
                Name = name,
                operationContext = OperationContext.Current
            };

            SendMsg(user.Name + " connected to the chat!", 0);

            users.Add(user);
            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SendMsg(user.Name + " left the chat!", 0);
            }   
        }

        public void SendMsg(string msg, int userId)
        {
            string answer = DateTime.Now.ToShortTimeString();
            ServerUser user = users.FirstOrDefault(i => i.Id == userId);
            if (user != null)
                answer += ": " + user.Name;

            answer += ": " + msg;

            foreach (var item in users)
                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
        }
    }
}