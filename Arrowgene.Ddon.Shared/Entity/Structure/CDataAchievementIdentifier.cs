using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

/// <summary>
///     Achivement identification is based on a unique ID which can be found in the client in
///     ui\gui_cmn\ui\00_param\achievement.acv.json
///     e.g. UId 2532 == All You Need Is Love, Category 4 / Collecting
/// </summary>
public class CDataAchievementIdentifier
{
    public uint UId;

    /// <summary>
    ///     Achievement UI list index, no correlation to client files
    /// </summary>
    public uint Index;


    public class Serializer : EntitySerializer<CDataAchievementIdentifier>
    {
        public override void Write(IBuffer buffer, CDataAchievementIdentifier obj)
        {
            WriteUInt32(buffer, obj.UId);
            WriteUInt32(buffer, obj.Index);
        }

        public override CDataAchievementIdentifier Read(IBuffer buffer)
        {
            CDataAchievementIdentifier obj = new CDataAchievementIdentifier();
            obj.UId = ReadUInt32(buffer);
            obj.Index = ReadUInt32(buffer);
            return obj;
        }
    }
}
