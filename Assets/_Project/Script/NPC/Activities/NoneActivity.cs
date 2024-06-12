// namespace Activities
// {
//     public class NoneActivity : Activity
//     {
//         public override bool isEnded { get; protected set; }
//         public override bool isCanceled { get; protected set; }
//
//         public override void StartActivity(New_NPC.NPC npc)
//         {
//             if (isCanceled) return;
//             isCanceled = true;
//         }
//
//         public override void UpdateActivity(New_NPC.NPC npc)
//         {
//             if (isCanceled) return;
//             isCanceled = true;
//         }
//
//         public override void EndActivity(New_NPC.NPC npc)
//         {
//             if (isCanceled) return;
//             isCanceled = true;
//         }
//     }
// }