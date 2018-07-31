using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Bakery.Models
{
    public class Message
    {
        /// <summary>
        /// this class is just used to pass messages from the controller
        ///to a view. It has a constructor to pass in the message
        /// </summary>
        /// 
        public Message() { }
        public Message(string msg)
        {
            MessageText = msg;
        }

        public string MessageText { set; get; }
    }
}