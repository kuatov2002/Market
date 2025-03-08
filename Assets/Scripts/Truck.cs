using UnityEngine;
using Zenject;
using Game.Scoring;

namespace Game.Interaction
{
    public class Truck : MonoBehaviour
    {
        [SerializeField] private string acceptableTag = "Pickup";
        
        // Reference to the score manager
        private ScoreManager _scoreManager;
        
        [Inject]
        public void Construct(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(acceptableTag))
            {
                Debug.Log($"Truck received item: {other.gameObject.name}");
                
                // Add points to the score
                _scoreManager.AddPoint();
                
                // Destroy the game object (not just the collider)
                Destroy(other.gameObject);
            }
        }
    }
}