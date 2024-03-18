public class EmptyActivity : Activity
{
    public override bool isEnded { get; protected set; }
    public override bool isCanceled { get; protected set; }

    public override void StartActivity(NPC npc)
    {
        if (isCanceled) return;
        isCanceled = true;
    }

    public override void UpdateActivity(NPC npc)
    {
        if (isCanceled) return;
        isCanceled = true;
    }

    public override void EndActivity(NPC npc)
    {
        if (isCanceled) return;
        isCanceled = true;
    }
}