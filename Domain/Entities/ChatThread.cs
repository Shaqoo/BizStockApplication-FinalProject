using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class ChatThread : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        public Guid? AssignedAgentId { get; private set; }
        public User? AssignedAgent { get; private set; }

        public ChatStatus Status { get; private set; } = ChatStatus.Open;

        public ICollection<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

        private ChatThread() { }

        public ChatThread(Guid customerId)
        {
            CustomerId = customerId;
            Status = ChatStatus.Open;
        }

        public void AssignAgent(Guid agentUserId)
        {
            AssignedAgentId = agentUserId;
            Status = ChatStatus.InProgress;
            Modified();
        }

        public void CloseThread()
        {
            Status = ChatStatus.Closed;
            Modified();
        }
    }

}
