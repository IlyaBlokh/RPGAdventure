namespace DamageSystem
{
    public interface IMessageReceiver
    {
        enum MessageType
        {
            DAMAGED,
            DEAD,
            QUEST_COMPLETE
        }

        void OnMessageReceive(MessageType messageType, object messageData);
    }
}
