using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChapterLayer.UI
{
    public class ChapterButtonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _chapterTitle;
        [SerializeField] private GameObject _prerequisite;
        [SerializeField] private GameObject _chapterDependencyTMP_Prefab;

        public void Initiate(string chapterTitle, bool isUnlock, bool isPass, string[] chapterDependencies)
        {
            _chapterTitle.text = chapterTitle;
            gameObject.GetComponent<Button>().interactable = isUnlock;
            foreach (string chapterDependency in chapterDependencies)
            {
                GameObject chapterDependencyObject = Instantiate(_chapterDependencyTMP_Prefab, _prerequisite.transform);
                chapterDependencyObject.GetComponent<TMP_Text>().text = chapterDependency;
            }
        }

        public void ChapterHovered()
        {
            if (!gameObject.GetComponent<Button>().interactable)
            {
                _prerequisite?.SetActive(true);
            }
        }

        public void ChapterUnHovered()
        {
            _prerequisite?.SetActive(false);
        }
    }
}