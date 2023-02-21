namespace Infrastructure.AssetProvider
{
    public abstract class AssetPaths
    {
        public const string BlankChunk = "Prefabs/Chunks/BlankChunk";
        public const string PlayerPath = "Prefabs/Player/Player";
        public const string Block = "Prefabs/Player/Block";
        public const string UIRoot = "Prefabs/UI/UIRoot";
        public const string UIHUD = "Prefabs/UI/HUD";
        public const string Curtain = "Prefabs/UI/Curtain";

        public const string FoodData = "Data/ChunkData";
        public const string PlayerData = "Data/PlayerData";
        public const string CameraData = "Data/CameraData";
        public const string UIData = "Data/UIData";

        public static readonly string[] ChunkPaths = {
            "Prefabs/Chunks/Chunk_1",
            "Prefabs/Chunks/Chunk_2",
            "Prefabs/Chunks/Chunk_3",
            "Prefabs/Chunks/Chunk_4",
            "Prefabs/Chunks/Chunk_5"
        };

        public const string PlusOneText = "Prefabs/Player/CollectCubeText";
        public const string WarpEffect = "Prefabs/Environment/WarpEffect";
    }
}