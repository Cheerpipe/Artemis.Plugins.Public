using Artemis.Core.Modules;
using Artemis.Plugins.DataModelExpansions.DynamicExternalDataModelExpansions.DataModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Artemis.Plugins.DataModelExpansions.DynamicExternalData
{
    public class JsonDataModelBridge
    {
        private JObject _jObject;
        private DataModel _dataModel;
        private string _key;

        public string Key { get => _key; }
        public DataModel DataModel { get => _dataModel; }
        public JObject JObject { get => _jObject; }

        public JsonDataModelBridge(JObject jObject)
        {
            _jObject = jObject;
            _key = GetJsonKey(_jObject);
            _dataModel = GetJsonDataModel(_jObject);
        }

        public void Merge(JsonDataModelBridge jsonDataModel)
        {
            _jObject.Merge(jsonDataModel.JObject);
            _dataModel = GetJsonDataModel(_jObject);
        }

        public JsonDataModelBridge(string json)
        {
            _jObject = JObject.Parse(json);
            _key = GetJsonKey(_jObject);
            _dataModel = GetJsonDataModel(_jObject);
        }

        private DataModel GetJsonDataModel(JObject json)
        {
            DataModel dynamicDataModel = new DynamicJsonDataModel();
            return JObjectToDataModel(json.Children<JProperty>().FirstOrDefault().Value, dynamicDataModel);
        }

        private string GetJsonKey(JObject json)
        {
            DataModel dynamicDataModel = new DynamicJsonDataModel();
            JToken root = json.Children<JProperty>().FirstOrDefault();
            var x = json.Root;
            // We need to be sure that we can use the root as the datamodel key validating that the root is not a property and has just one child
            if (root.First.Type != JTokenType.Object || root.Next != null)
            {
                throw new FormatException("First element of the Json must be root element with just one unique object child.");
            }
            return root.Path;
        }

        public DataModel JObjectToDataModel(JToken node, DataModel dataModel)
        {
            var x = node.FirstOrDefault();
            if (node.Type == JTokenType.Object)
            {
                DynamicJsonDataModel dynamicDataModel = new DynamicJsonDataModel();
                dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault(), dynamicDataModel);
                foreach (JProperty child in node.Children<JProperty>())
                {
                    JObjectToDataModel(child.Value, dynamicDataModel);
                }
                return dynamicDataModel;
            }
            else if (node.Type == JTokenType.Array)
            {
                DynamicJsonDataModel dynamicDataModel = new DynamicJsonDataModel();
                dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault(), dynamicDataModel);
                foreach (JToken child in node.Children())
                {
                    JObjectToDataModel(child, dynamicDataModel);
                }
                return dynamicDataModel;
            }
            else
            {
                dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault(), ((JValue)node).Value);
                return dataModel;
            }
        }
    }
}
