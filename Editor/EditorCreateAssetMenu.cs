using UnityEditor;

namespace LLib.RPG.Editor
{
    public static class EditorCreateAssetMenu
    {
        [MenuItem("Assets/Create/[ LumosLib ]/Prefabs/Manager/Buff", false, int.MinValue)]
        public static void CreateAudioPlayerPrefab()
        {
            LLib.Editor.EditorCreateAssetMenu.CreatePrefab<BuffManager>();
        }
    }
}