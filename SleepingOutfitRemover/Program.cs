using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

namespace SleepingOutfitRemover
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SleepingOutfitRemover.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var npc in state.LoadOrder.PriorityOrder.Npc().WinningContextOverrides())
            {
                var newNpc = state.PatchMod.Npcs.GetOrAddAsOverride(npc.Record);
                newNpc.SleepingOutfit.SetToNull();
                if (newNpc.Equals(npc.Record))
                    state.PatchMod.Npcs.Remove(npc.Record);
            }
        }
    }
}
