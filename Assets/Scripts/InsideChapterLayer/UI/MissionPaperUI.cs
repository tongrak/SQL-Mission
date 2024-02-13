﻿using TMPro;
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
        [SerializeField] private GameObject _passedStamp;

        public void Initiate(string title, string description, bool isUnlock, bool isPass, string[] missionDependencies)
        {
            missionTitle.text = title;
            missionDescription.text = description;
            missionPaper.interactable = isUnlock;

            if (isPass)
            {
                _passedStamp.SetActive(true);
            }

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