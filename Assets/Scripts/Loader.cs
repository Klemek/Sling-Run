using UnityEngine;

namespace SlingRun
{
    public class Loader : MonoBehaviour
    {
        public GameManager GameManager;

        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(GameManager);
        }
    }
}