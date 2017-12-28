using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models.dto
{
    public class Message
    {
        public bool success { get; set; }// 是否成功

        public  string msg { get; set; }// 提示信息

        public Object obj { get; set; }// 其他信息

        public Dictionary<string, object> attributes { get; set; }

        public Message(bool success, string msg, object obj,Dictionary<string, object> attributes)
        {
            this.success = success;
            this.msg = msg;
            this.obj = obj;
            this.attributes = attributes;
        }
    }
}