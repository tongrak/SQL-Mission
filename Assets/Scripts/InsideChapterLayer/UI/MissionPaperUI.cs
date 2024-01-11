using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BackendComponent.UI
{
    public class MissionPaperUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text missionTitle;
        [SerializeField] private TMP_Text missionDescription;
        [SerializeField] private Button missionPaper;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initiate(string title, string description, bool isUnlock, bool isPass)
        {
            missionTitle.text = title;
            missionDescription.text = description;
            missionPaper.interactable = isUnlock;
        }
    }
}