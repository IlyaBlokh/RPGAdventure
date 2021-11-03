using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGAdventure
{
    public interface IMessageReceiver
    {
        enum MessageType
        {
            DAMAGED,
            DEAD
        }

        void OnMessageReceive(MessageType messageType, object messageData);
    }
}
