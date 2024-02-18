using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChapterLayer.UI
{
    public class ChapterButtonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _chapterTitle;
        [SerializeField] private TMP_Text _chapterIndex;
        [SerializeField] private GameObject _prerequisite;
        [SerializeField] private GameObject _chapterDependencyTMP_Prefab;
        [SerializeField] private GameObject _passedStamp;

        /// <summary>
        /// Init chapter button on UI.
        /// </summary>
        /// <param name="chapterTitle"></param>
        /// <param name="isUnlock"></param>
        /// <param name="index">Must be above 0.</param>
        /// <param name="isPass"></param>
        /// <param name="chapterDependencies"></param>
        public void Initiate(string chapterTitle, bool isUnlock, int index, bool isPass, string[] chapterDependencies)
        {
            _chapterTitle.text = chapterTitle;
            _chapterIndex.text = index.ToString();
            _passedStamp.SetActive(isPass);
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