using Graphics2026.Model.Actors;

namespace Graphics2026.Model.Attachments
{
    internal abstract class Attachment<T> : ActiveRoleBehaviour where T : Actor
    {
        public readonly T actor;
        public readonly Transform transform;

        public Attachment(T actor)
        {
            this.actor = actor;
            transform = actor.transform;
        }
    }
}
