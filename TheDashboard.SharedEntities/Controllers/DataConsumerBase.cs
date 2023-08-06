//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace TheDashboard.SharedEntities
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface IDataConsumerBaseController
    {

        /// <returns>Success</returns>

        System.Threading.Tasks.Task<System.Collections.Generic.ICollection<DataSourceDto>> GetAllAsync();


        /// <returns>Created</returns>

        System.Threading.Tasks.Task<DataSourceDto> AddAsync(DataSourceDto body);


        /// <returns>Success</returns>

        System.Threading.Tasks.Task<DataSourceDto> UpdateAsync(DataSourceDto body);


        /// <returns>Success</returns>

        System.Threading.Tasks.Task<DataSourceDto> GetAsync(int id);


        /// <returns>Accepted</returns>

        System.Threading.Tasks.Task<DataSourceDto> DeleteAsync(int id);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class DataConsumerBaseController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private IDataConsumerBaseController _implementation;

        public DataConsumerBaseController(IDataConsumerBaseController implementation)
        {
            _implementation = implementation;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/dc")]
        public System.Threading.Tasks.Task<System.Collections.Generic.ICollection<DataSourceDto>> GetAll()
        {

            return _implementation.GetAllAsync();
        }

        /// <returns>Created</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/dc")]
        public System.Threading.Tasks.Task<DataSourceDto> Add([Microsoft.AspNetCore.Mvc.FromBody] DataSourceDto body)
        {

            return _implementation.AddAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/dc")]
        public System.Threading.Tasks.Task<DataSourceDto> Update([Microsoft.AspNetCore.Mvc.FromBody] DataSourceDto body)
        {

            return _implementation.UpdateAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/dc/{id}")]
        public System.Threading.Tasks.Task<DataSourceDto> Get(int id)
        {

            return _implementation.GetAsync(id);
        }

        /// <returns>Accepted</returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete, Microsoft.AspNetCore.Mvc.Route("api/dc/{id}")]
        public System.Threading.Tasks.Task<DataSourceDto> Delete(int id)
        {

            return _implementation.DeleteAsync(id);
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DataSourceDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("name")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("description")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Description { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("url")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Url { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("dashboardId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Guid DashboardId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("sourceType")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string SourceType { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ProblemDetails
    {

        [System.Text.Json.Serialization.JsonPropertyName("type")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Type { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("title")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("status")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Status { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("detail")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Detail { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("instance")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Instance { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }


}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603