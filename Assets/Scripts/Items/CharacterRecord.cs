
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { } 
}

namespace Items
{
    /*
     * Contains the records of the Pawn.
     *  - Character Type
     *  - Action sub-categories,
     *  - Action Instances - including their details and materials needed for the actions 
     */


    public record CharacterRecord
    {
        public CharacterRecord(string name, ECharacterType type)
        {
            PawnName = name;
            PawnType = type;
        }
        public string PawnName { get; init; }
        public ECharacterType PawnType { get; init; }
    };
}