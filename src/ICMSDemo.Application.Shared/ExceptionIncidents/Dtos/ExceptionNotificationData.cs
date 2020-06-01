using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.ExceptionIncidents.Dtos
{

    [Serializable]
    public class ExceptionNotificationData : NotificationData
    {
        public string SenderUserName { get; set; }

        public string Message { get; set; }

        public ExceptionNotificationData(string senderUserName, string message)
        {
            SenderUserName = senderUserName;
            Message = message;
        }
    }
}
