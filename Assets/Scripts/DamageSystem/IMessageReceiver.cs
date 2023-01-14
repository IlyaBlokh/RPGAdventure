namespace DamageSystem
{
    public interface IMessageReceiver
    {
        enum MessageType
        {
            Damaged,
            Dead,
            QuestComplete
        }

        void OnMessageReceive(MessageType messageType, object messageData);
    }
}
