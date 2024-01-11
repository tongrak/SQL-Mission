using Assets.Scripts.BackendComponent.Model;
using TMPro;
//using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.UI
{
    public interface ISchemaDisplayController
    {
        void SetUpDisplay(Schema[] schemas);
    }
    public class SchemaDisplayController : GameplayController, ISchemaDisplayController
    {
        [Header("Configuration")]
        [SerializeField] private GameObject _clickableCellPrefab;
        [SerializeField] private GameObject _selectionListGO;
        [SerializeField] private GameObject _displayListGO;

        private ISchemaAttributesController _schemaAttrCon => mustGetComponent<ISchemaAttributesController>(_displayListGO);

        public void SetUpDisplay(Schema[] schemas)
        {
            var tables = new string[schemas.Length];
            var attributes = new string[schemas.Length][];
            for (int i = 0; i < schemas.Length; i++)
            {
                var created = Instantiate(_clickableCellPrefab, _selectionListGO.transform);
                var createdButton = mustGetComponent<UnityEngine.UI.Button>(created);
                var createdTMP = mustGetComponent<TextMeshProUGUI>(created);

                createdTMP.text = schemas[i].TableName;
                var clonedAttr = (string[])schemas[i].Attributes.Clone();
                createdButton.onClick.AddListener(getOnClickAction(clonedAttr));
            }
        }
        private UnityAction getOnClickAction(string[] attribute) 
        {
            return () => _schemaAttrCon.SetDisplayAttribute(attribute);
        }
        private void removePastSchema()
        {
            //Remove past selection list
            foreach (GameObject option in _selectionListGO.transform) Destroy(option);

            _schemaAttrCon.RemoveDisplayAttribute();
        }

        #region Temporary Unity Basic
        private void Start()
        {
            var testAttr1 = new string[] { "apple", "butter", "cactus" };
            var testSchema1 = new Schema("Table1", testAttr1);
            var testAttr2 = new string[] { "1", "2", "3", "4", "5" };
            var testSchema2 = new Schema("Table2", testAttr2);
            var testAttr3 = new string[] { "Cream" };
            var testSchema3 = new Schema("Table3", testAttr3);

            var testSchemas = new Schema[] { testSchema1, testSchema2, testSchema3 };

            this.SetUpDisplay(testSchemas);
        }
        #endregion
    }
}


