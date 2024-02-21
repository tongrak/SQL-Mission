using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ChapterLayer
{
    public class URLChapterController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _chapterTitle;
        [SerializeField] private TMP_Text _chapterIndex;

        private ChapterButtonManager _chapterButtonManager;
        public void Initiate(int chapterIndex, string title, bool interactable,ChapterButtonManager chapterButtonManager)
        {
            _chapterIndex.text = chapterIndex.ToString();
            _chapterTitle.text = title;
            _chapterButtonManager = chapterButtonManager;
            gameObject.GetComponent<Button>().interactable = interactable;
        }

        public void ButtonClicked()
        {
            _chapterButtonManager.URLButtonClicked();
        }
    }
}