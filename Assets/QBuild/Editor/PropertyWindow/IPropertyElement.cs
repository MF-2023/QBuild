using System;
using QBuild.Camera;
using UnityEditor;

namespace QBuild.PropertyWindow
{
    public interface IPropertyElement
    {
        public string DisplayName { get; }
    };

    public class ScriptableTreeElement : IPropertyElement
    {
        public Type Type { get; }

        public string DisplayName { get; }

        public ScriptableTreeElement(Type type)
        {
            Type = type;
            DisplayName = type.Name;
        }
        
        public ScriptableTreeElement(Type type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }
        
    }


    public class GenreTreeElement : IPropertyElement
    {
        private string _name;
        public string DisplayName => _name;
        
        public GenreTreeElement(string displayName)
        {
            _name = displayName;
        }
    }
}