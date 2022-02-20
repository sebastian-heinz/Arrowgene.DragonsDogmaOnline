using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageAttribute
    {
        // Please note i haven't checked if the fields are in the same order as in the PS4 build
        public bool IsSolo { get; set; }
        public bool IsEnablePartyFunc { get; set; }
        public bool IsAdventureCountKeep { get; set; }
        public bool IsEnableCraft { get; set; }
        public bool IsEnableStorage { get; set; }
        public bool IsEnableStorageInCharge { get; set; }
        public bool IsNotSessionReturn { get; set; }
        public bool IsEnableBaggage { get; set; }
        public bool IsClanBase { get; set; }
        public bool Unk0 { get; set; }
        public bool Unk1 { get; set; }

        public CDataStageAttribute()
        {
            IsSolo=false;
            IsEnablePartyFunc=false;
            IsAdventureCountKeep=false;
            IsEnableCraft=false;
            IsEnableStorage=false;
            IsEnableStorageInCharge=false;
            IsNotSessionReturn=false;
            IsEnableBaggage=false;
            IsClanBase=false;
            Unk0=false;
            Unk1=false;
        }

        public class Serializer : EntitySerializer<CDataStageAttribute>
        {
            public override void Write(IBuffer buffer, CDataStageAttribute obj)
            {
                WriteBool(buffer,obj.IsSolo);
                WriteBool(buffer,obj.IsEnablePartyFunc);
                WriteBool(buffer,obj.IsAdventureCountKeep);
                WriteBool(buffer,obj.IsEnableCraft);
                WriteBool(buffer,obj.IsEnableStorage);
                WriteBool(buffer,obj.IsEnableStorageInCharge);
                WriteBool(buffer,obj.IsNotSessionReturn);
                WriteBool(buffer,obj.IsEnableBaggage);
                WriteBool(buffer,obj.IsClanBase);
                WriteBool(buffer,obj.Unk0);
                WriteBool(buffer,obj.Unk1);
            }

            public override CDataStageAttribute Read(IBuffer buffer)
            {
                CDataStageAttribute obj = new CDataStageAttribute();
                obj.IsSolo = ReadBool(buffer);
                obj.IsEnablePartyFunc = ReadBool(buffer);
                obj.IsAdventureCountKeep = ReadBool(buffer);
                obj.IsEnableCraft = ReadBool(buffer);
                obj.IsEnableStorage = ReadBool(buffer);
                obj.IsEnableStorageInCharge = ReadBool(buffer);
                obj.IsNotSessionReturn = ReadBool(buffer);
                obj.IsEnableBaggage = ReadBool(buffer);
                obj.IsClanBase = ReadBool(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
