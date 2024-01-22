using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.SQLComponent;
using System.Linq;

namespace Assets.Scripts.DataPersistence.BlankBlockComponent
{
    public class UpToConfigTemplateService : IUpToConfigTemplateService
    {
        ISQLService _sqlService;

        public UpToConfigTemplateService(ISQLService sqlService)
        {
            _sqlService = sqlService;
        }

        public string[] GetAttributesTemplate(string dbConn, string table)
        {
            return _sqlService.GetSchemas(dbConn, new string[] { table }, false)[0].Attributes;
        }

        public string[] GetSchemaTemplate(string dbConn, string table)
        {
            Schema[] schemas = _sqlService.GetSchemas(dbConn, new string[] {table}, false);

            string[] schema = new string[] {table};
            schema = schema.Concat(schemas[0].Attributes.ToArray()).ToArray();
            schema = schema.Append("*").ToArray();

            return schema;
        }

        public string[] GetTablesTemplate(string dbConn)
        {
            return _sqlService.GetAllTable(dbConn);
        }
    }
}
