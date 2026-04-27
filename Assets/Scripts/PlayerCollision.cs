using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCollision : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D _other)
        {
            if (_other.gameObject.TryGetComponent<Enemy>(out _))
            {
                //Destroy(gameObject);
                
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }
    }
}