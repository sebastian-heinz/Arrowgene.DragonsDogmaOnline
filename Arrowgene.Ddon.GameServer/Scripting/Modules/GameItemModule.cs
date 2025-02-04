using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class GameItemModule : ScriptModule
    {
        public override string ModuleRoot => "game_items";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        private Dictionary<ItemId, IGameItem> Items { get; set; }

        public bool HasItem(ItemId itemId)
        {
            return Items.ContainsKey(itemId);
        }

        public bool HasItem(uint itemId)
        {
            return HasItem((ItemId)itemId);
        }

        public IGameItem? GetItemInterface(ItemId itemId)
        {
            if (!Items.ContainsKey(itemId))
            {
                return null;
            }
            return Items[itemId];
        }

        public IGameItem? GetItemInterface(uint itemId)
        {
            return GetItemInterface((ItemId)itemId);
        }

        public GameItemModule()
        {
            Items = new Dictionary<ItemId, IGameItem>();
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            IGameItem item = (IGameItem)result.ReturnValue;
            Items[item.ItemId] = item;

            return true;
        }
    }
}
