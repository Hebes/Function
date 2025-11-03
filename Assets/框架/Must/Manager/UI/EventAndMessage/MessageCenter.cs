using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    消息（传递）中心
    负责UI框架中，所有UI窗体中间的数据传值。

-----------------------*/


namespace Framework.Core
{
    public class MessageCenter
    {
        //委托：消息传递
        public delegate void DelMessageDelivery(KeyValuesUpdate kv);
        //消息中心缓存集合
        //<string : 数据大的分类，DelMessageDelivery 数据执行委托>
        public static Dictionary<string, DelMessageDelivery> _dicMessages = new Dictionary<string, DelMessageDelivery>();

        /// <summary>
        /// 增加消息的监听。
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void AddMsgListener(string messageType, DelMessageDelivery handler)
        {
            if (!_dicMessages.ContainsKey(messageType))
                _dicMessages.Add(messageType, null);
            _dicMessages[messageType] += handler;
        }

        /// <summary>
        /// 取消消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handele">消息委托</param>
	    public static void RemoveMsgListener(string messageType, DelMessageDelivery handele)
        {
            if (_dicMessages.ContainsKey(messageType))
                _dicMessages[messageType] -= handele;
        }

        /// <summary>
        /// 取消所有指定消息的监听
        /// </summary>
	    public static void ClearALLMsgListener()
        {
            _dicMessages?.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息的分类</param>
        /// <param name="kv">键值对(对象)</param>
	    public static void SendMessage(string messageType, KeyValuesUpdate kv)
        {
            DelMessageDelivery del;                         //委托
            if (_dicMessages.TryGetValue(messageType, out del))
                del?.Invoke(kv); //调用委托
        }
    }

    /// <summary>
    /// 键值更新对
    /// 功能： 配合委托，实现委托数据传递
    /// </summary>
    public class KeyValuesUpdate
    {
        private string _Key;//键
        private object _Values;//值

        public string Key => _Key;
        public object Values => _Values;

        public KeyValuesUpdate(string key, object valueObj)
        {
            _Key = key;
            _Values = valueObj;
        }
    }
}

#region MyRegion
///// <summary>发送消息</summary>
///// <param name="msgType">消息的类型</param>
///// <param name="msgName">消息名称</param>
///// <param name="msgContent">消息内容</param>
//protected void SendMessage(string msgType, string msgName, object msgContent)
//{
//    KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
//    MessageCenter.SendMessage(msgType, kvs);
//}
///// <summary> 接收消息 </summary> 
///// <param name="messagType">消息分类</param>
///// <param name="handler">消息委托</param>
//public void ReceiveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
//{
//    MessageCenter.AddMsgListener(messagType, handler);
//}
#endregion