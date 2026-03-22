using UnityEditor;

namespace LumosLib.RPG.Editor
{
    public static class EditorCreateAssetMenu
    {
        [MenuItem("Assets/Create/[ LumosLib ]/Prefabs/Manager/Buff", false, int.MinValue)]
        public static void CreateAudioPlayerPrefab()
        {
            LumosLib.Editor.EditorCreateAssetMenu.CreatePrefab<BuffManager>();
        }
    }
}