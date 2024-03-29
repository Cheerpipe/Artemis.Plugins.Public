﻿using System.Linq;
using Artemis.Core.Modules;
using Artemis.Plugins.Modules.Json.DataModels;
using Newtonsoft.Json.Linq;
using SkiaSharp;

namespace Artemis.Plugins.Modules.Json.Services.JsonDataModelServices
{
    public class JsonDataModelBridge
    {
        public DataModel DataModel { get; private set; }

        public JObject JObject { get; }

        public void Merge(JsonDataModelBridge jsonDataModel)
        {
            JObject.Merge(jsonDataModel.JObject);
            DataModel = GetJsonDataModel(JObject);
        }

        public JsonDataModelBridge(string json)
        {
            JObject = JObject.Parse(json);
            DataModel = GetJsonDataModel(JObject);
        }

        private DataModel GetJsonDataModel(JObject json)
        {
            DataModel dynamicDataModel = new JsonDataModel();
            return JObjectToDataModel(json, dynamicDataModel);
        }

        public DataModel JObjectToDataModel(JToken node, DataModel dataModel)
        {
            switch (node.Type)
            {
                case JTokenType.Object:
                    {
                        JsonDataModel dynamicDataModel = new();
                        dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault() ?? string.Empty, dynamicDataModel);
                        foreach (var child in node.Children<JProperty>())
                        {
                            JObjectToDataModel(child.Value, dynamicDataModel);
                        }
                        return dynamicDataModel;
                    }
                case JTokenType.Array:
                    {
                        JsonDataModel dynamicDataModel = new();
                        dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault() ?? string.Empty, dynamicDataModel);
                        foreach (var child in node.Children())
                        {
                            JObjectToDataModel(child, dynamicDataModel);
                        }
                        return dynamicDataModel;
                    }
                default:
                    // Special Types
                    object nodeValue;
                    if (SKColor.TryParse(((JValue)node).ToString(), out SKColor colorNodeValue) && ((JValue)node).ToString().StartsWith('#'))
                    {
                        nodeValue = colorNodeValue;
                    }
                    else
                    {
                        nodeValue = ((JValue)node).Value;
                    }
                    dataModel.AddDynamicChild(node.Path.Split('.').LastOrDefault() ?? string.Empty, nodeValue);
                    return dataModel;
            }
        }
    }
}
