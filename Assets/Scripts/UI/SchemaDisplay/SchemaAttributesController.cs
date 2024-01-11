//using Gameplay.UI.Table;
using System.Collections;
using UnityEngine;

namespace Gameplay.UI
{
    public interface ISchemaAttributesController
    {
        void SetDisplayAttribute(string[] attributes);
        void RemoveDisplayAttribute();
    }
    public class SchemaAttributesController : Table.ColumnController, ISchemaAttributesController
    {
        public void SetDisplayAttribute(string[] attributes)
        {
            RemoveDisplayAttribute();
            base.setColumnDisplay(attributes);
        }

        public void RemoveDisplayAttribute()
        {
            foreach (Transform child in this.transform) Destroy(child.gameObject);
        }
    }
}