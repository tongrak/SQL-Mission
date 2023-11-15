using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Table
{
    interface ICellController
    {
        TextMeshProUGUI TextMeshPro { get; }
    }

    public class CellController : MonoBehaviour, ICellController
    {
        [SerializeField] private TextMeshProUGUI _cellText;
        public TextMeshProUGUI TextMeshPro => _cellText;
    }

}
