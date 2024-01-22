#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Linq;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class MoveLine : MonoBehaviour
    {
        [SerializeField] private GameObject _lineElementPrefab;

        public void GenerateLine(Vector3 start, Vector3 end)
        {
#if UNITY_EDITOR
            foreach(var child in transform.OfType<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
            
            Vector3 direction = end - start;
            float distance = direction.magnitude;
            int count = (int)(distance + 1);
            Vector3 unit = direction.normalized;
            for (int i = 0; i < count; i++)
            {
                var lineElement = PrefabUtility.InstantiatePrefab(_lineElementPrefab) as GameObject;
                lineElement.transform.SetParent(transform);
                lineElement.transform.position = start + unit * i;
            }
#endif
        }
    }
}