using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace CracklingLeafTools.AudioSystem.Core.Editor
{
    public static class SoundKeyGenerator
    {
        const string GeneratedFolder = "Assets/CracklingLeafTools/AudioSystem/Core/Generated/";
        const string FileName = "SoundKey.cs";
        public const char Separator = ':';

        [MenuItem("CracklingLeafTools/Audio System/ Generate SoundKey Constants")]
        public static void GenerateConstants()
        {   //find all sound definitions
            string[] guids = AssetDatabase.FindAssets("t:SoundLibrary");
            //create directory if it doesn't exist.
            if((!Directory.Exists(GeneratedFolder))) { Directory.CreateDirectory(GeneratedFolder); }

            //begin writing file
            StringBuilder sb = new();
            sb.AppendLine($"namespace CracklingLeafTools.AudioSystem.Core");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class SoundKey");
            sb.AppendLine("    {");

            foreach(string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundLibrary def = AssetDatabase.LoadAssetAtPath<SoundLibrary>(path);

                //SoundDefinition def = AssetDatabase.LoadAssetAtPath<SoundDefinition>(path);
                if(def == null) { continue; }
                string libraryName = MakeStringSafe(def.libraryName);

                for (int i = 0; i < def.sounds.Count; i++)
                {
                    string key = MakeStringSafe(def.sounds[i].soundKey);
                    sb.AppendLine($"        public const string {libraryName}_{key} = \"{libraryName}{Separator}{key}\";");
                }
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            //write the file
            string fullPath = Path.Combine(GeneratedFolder, FileName);
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

            //refresh unity
            AssetDatabase.Refresh();
            Debug.Log($"SoundKey file generated with {guids.Length} libraries.");
        }

        //converts a definition name into a valid c# string
        public static string MakeStringSafe(string key)
        {
            if(string.IsNullOrWhiteSpace(key)) { return "_UnknownKey"; }
            //replace separators with underscores
            string safe = key.Replace(" ", "_").Replace("-", "_");
            if(safe.Contains(Separator))
            {
                Debug.LogWarning($"Key \"{key}\" contains reserved separator '{Separator}', replacing with '_'.");
                safe = safe.Replace(":", "_");
            }
            
            //remove invalid characters
            StringBuilder sb = new();
            foreach(char c in safe)
            {
                if(char.IsLetterOrDigit(c) || c == '_') { sb.Append(c); }
            }

            safe = sb.ToString();
            //ensure it begins with a letter for valid c# identifier
            if(!char.IsLetter(safe, 0)) { safe = "_" + safe; }
            return safe;
        }
    }

}


