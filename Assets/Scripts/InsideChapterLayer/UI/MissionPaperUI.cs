using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DataPersistence.UI
{
    public class MissionPaperUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text missionTitle;
        [SerializeField] private TMP_Text missionDescription;
        [SerializeField] private Button missionPaper;
        [SerializeField] private GameObject _prerequisite;
        [SerializeField] private GameObject _missionDependencyTMP;

        // Use this for initialization
        void Start()
        {
               
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Initiate(string title, string description, bool isUnlock, bool isPass, string[] missionDependencies)
        {
            missionTitle.text = title;
            missionDescription.text = description;
            missionPaper.interactable = isUnlock;
            foreach (string missionDependency in missionDependencies)
            {
                GameObject missionDependencyObject = Instantiate(_missionDependencyTMP, _prerequisite.transform);
                missionDependencyObject.GetComponent<TMP_Text>().text = missionDependency;
            }
        }

        public void MissionPaperHovered()
        {
            if (!missionPaper.interactable)
            {
                _prerequisite?.SetActive(true);
            }
        }

        public void MissionPaperUnHovered()
        {
            _prerequisite?.SetActive(false);
        }
    }
}