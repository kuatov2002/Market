using Game.Scoring;
using UnityEngine;
using Zenject;

namespace Game.Interaction
{
    /// <summary>
    /// Component that handles item delivery to the truck
    /// </summary>
    public class Truck : MonoBehaviour
    {
        [SerializeField] private string _acceptableTag = "Pickup";

        private IScoreManager _scoreManager;

        [Inject]
        public void Construct(IScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_acceptableTag))
            {
                Debug.Log($"Truck received item: {other.gameObject.name}");

                // Add points to the score
                _scoreManager.AddPoint();

                // Destroy the game object
                Destroy(other.gameObject);
            }
        }
    }
}