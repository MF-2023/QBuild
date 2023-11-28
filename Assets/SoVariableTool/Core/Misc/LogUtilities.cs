using UnityEngine;

namespace SoVariableTool
{
    public static class LogUtilities
    {
        public static string GenerateTag(Color color, string tag)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>[{tag}] </color>";
        }
        
        public static string ColorTagMessage(Color color, string tag,string message)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>[{tag}] </color> {message}";
        }
    }
}