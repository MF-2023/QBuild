using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QBuild
{
    /// <summary>
    /// シーンのビルドインデックスを定数で管理するクラスを作成するスクリプト
    /// </summary>
    public static class SceneBuildIndexCreator
    {
        // 無効な文字を管理する配列
        private static readonly string[] INVALID_CHARS =
        {
            " ", "!", "\"", "#", "$",
            "%", "&", "\'", "(", ")",
            "-", "=", "^", "~", "\\",
            "|", "[", "{", "@", "`",
            "]", "}", ":", "*", ";",
            "+", "/", "?", ".", ">",
            ",", "<"
        };

        private const string ITEM_NAME = "Tools/Create/Scene Build Index"; // コマンド名
        private const string PATH = "Assets/QBuild/GameCycle/Scene/SceneBuildIndex.cs"; // ファイルパス

        private static readonly string FILENAME = Path.GetFileName(PATH); // ファイル名(拡張子あり)

        private static readonly string
            FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH); // ファイル名(拡張子なし)

        /// <summary>
        /// シーン名を定数で管理するクラスを作成します
        /// </summary>
        [MenuItem(ITEM_NAME)]
        public static void Create()
        {
            if (!CanCreate())
            {
                return;
            }

            CreateScript();

            EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
        }

        /// <summary>
        /// スクリプトを作成します
        /// </summary>
        public static void CreateScript()
        {
            var builder = new StringBuilder();

            builder.AppendLine("/// <summary>");
            builder.AppendLine("/// シーンのビルドインデックスを定数で管理するクラス");
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
            builder.AppendLine("{");

            foreach (var n in EditorBuildSettings.scenes
                         .Select((val, index) => new
                             { var = RemoveInvalidChars(Path.GetFileNameWithoutExtension(val.path)), val = index }))
            {
                builder.Append("\t").AppendFormat(@"public const int {0} = {1};", n.var, n.val).AppendLine();
            }

            builder.AppendLine("}");

            var directoryName = Path.GetDirectoryName(PATH);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// シーン名を定数で管理するクラスを作成できるかどうかを取得します
        /// </summary>
        [MenuItem(ITEM_NAME, true)]
        public static bool CanCreate()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }

        /// <summary>
        /// 無効な文字を削除します
        /// </summary>
        public static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALID_CHARS, c => str = str.Replace(c, string.Empty));
            return str;
        }
    }
}