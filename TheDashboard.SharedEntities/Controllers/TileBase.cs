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
    public interface ITileBaseController
    {



        /// <returns>Success</returns>

        System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TileDto>> GetDashboardTilesAsync(System.Guid dashboardId);


        /// <returns>Success</returns>

        System.Threading.Tasks.Task<TileDto> GetTileAsync(int id);



        /// <returns>Accepted</returns>

        System.Threading.Tasks.Task<TileDto> UpdateTileAsync(int id, TileDto body);


        /// <returns>No Content</returns>

        System.Threading.Tasks.Task<TileDto> DeleteTileAsync(int id);


        /// <returns>Created</returns>

        System.Threading.Tasks.Task<TileDto> AddTileAsync(TileDto body);


        /// <returns>Success</returns>

        System.Threading.Tasks.Task<bool> HasTilesAsync(System.Guid? body);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class TileBaseController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private ITileBaseController _implementation;

        public TileBaseController(ITileBaseController implementation)
        {
            _implementation = implementation;
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/ts/tile/{dashboardId}")]
        public System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TileDto>> GetDashboardTiles(System.Guid dashboardId)
        {

            return _implementation.GetDashboardTilesAsync(dashboardId);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/ts/{id}")]
        public System.Threading.Tasks.Task<TileDto> GetTile(int id)
        {

            return _implementation.GetTileAsync(id);
        }

        /// <returns>Accepted</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/ts/{id}")]
        public System.Threading.Tasks.Task<TileDto> UpdateTile(int id, [Microsoft.AspNetCore.Mvc.FromBody] TileDto body)
        {

            return _implementation.UpdateTileAsync(id, body);
        }

        /// <returns>No Content</returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete, Microsoft.AspNetCore.Mvc.Route("api/ts/{id}")]
        public System.Threading.Tasks.Task<TileDto> DeleteTile(int id)
        {

            return _implementation.DeleteTileAsync(id);
        }

        /// <returns>Created</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/ts")]
        public System.Threading.Tasks.Task<TileDto> AddTile([Microsoft.AspNetCore.Mvc.FromBody] TileDto body)
        {

            return _implementation.AddTileAsync(body);
        }

        /// <returns>Success</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/ts/hastiles")]
        public System.Threading.Tasks.Task<bool> HasTiles([Microsoft.AspNetCore.Mvc.FromBody] System.Guid? body)
        {

            return _implementation.HasTilesAsync(body);
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.19.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TileDto
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("title")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("subTitle")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string SubTitle { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("url")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Url { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("dashboardId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Guid DashboardId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("dataSourceId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Guid DataSourceId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("visualizerId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int VisualizerId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("xOffset")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int XOffset { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("yOffset")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int YOffset { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("width")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int Width { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("height")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int Height { get; set; }

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