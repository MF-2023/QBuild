using System.Runtime.CompilerServices;

namespace QBuild.Const
{
    public static class EditorConst
    {
        /// <summary>
        /// QBuild用のScriptableObjectのパスの先頭
        /// </summary>
        public const string ScriptablePrePath = "Tools/QBuild/";

        /// <summary>
        /// QBuild用のScriptableObjectのVariableのパスの先頭
        /// </summary>
        public const string VariablePrePath = "Tools/QBuild/Variable/";
        
        /// <summary>
        /// QBuild用のエディタWindowのパスの先頭
        /// </summary>
        public const string WindowPrePath = "Tools/QBuild/";
        
        /// <summary>
        /// 重要度の低い順に並べるための定数
        /// </summary>
        public const int OtherOrder = 11;
    }
}