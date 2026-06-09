using Graphics2026.Model.Actors;

namespace Graphics2026.Model.Attachments
{
    internal abstract class ActorAttachment : Attachment<Actor>
    {
        public ActorAttachment(Actor actor) : base(actor) { }
    }
}
