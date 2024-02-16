using Gameplay.UI.Table;
using TMPro;
//using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.UI
{
    public interface ISchemaDisplayController
    {
        void SetUpDisplay(SchemaDTO[] schemas);
    }
    public class SchemaDisplayController : Table.GenericGenerator, ISchemaDisplayController
    {
        [Header("Configuration")]
        [SerializeField] private GameObject _clickableCellPrefab;
        [SerializeField] private GameObject _selectionListGO;
        [SerializeField] private GameObject _displayListGO;

        private ICellsGenerator _schemaAttrCon => mustGetComponent<ICellsGenerator>(_displayListGO);

        public void SetUpDisplay(SchemaDTO[] schemas)
        {
            removePastSchema();

            var tables = new string[schemas.Length];
            var attributes = new string[schemas.Length][];
            for (int i = 0; i < schemas.Length; i++)
            {
                var created = Instantiate(_clickableCellPrefab, _selectionListGO.transform);
                var createdButton = mustGetComponent<UnityEngine.UI.Button>(created);
                var createdTMP = mustGetComponent<TextMeshProUGUI>(created);

                createdTMP.text = schemas[i].tableName;
                createdButton.onClick.AddListener(getOnClickAction(schemas[i].attribuites));
            }
            //Display first schema
            _schemaAttrCon.setCellsDisplay(schemas[0].attribuites);
        }
        private UnityAction getOnClickAction(string[] attribute)
        {
            return () => _schemaAttrCon.setCellsDisplay(attribute);
        }
        private void removePastSchema()
        {
            //Remove past selection list
            foreach (Transform child in _selectionListGO.transform) Destroy(child.gameObject);
            _schemaAttrCon.destroyAllCells();
        }
    }
}


