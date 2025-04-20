using Microsoft.OpenApi.Models;
using RecipeBook.API.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RecipeBook.API.Filter
{
    public class IdsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
            var encripytedIds = context
                .ApiDescription
                .ParameterDescriptions
                .Where(x => x.ModelMetadata.BinderType == typeof(RecipeBookIdBinder))
                .ToDictionary(d => d.Name, d => d);

            foreach(var parameter in operation.Parameters)
            {
                if (encripytedIds.TryGetValue(parameter.Name, out var apiParameter))
                {
                    parameter.Schema.Format = string.Empty;
                    parameter.Schema.Type = "string";
                }
            }

            foreach (var schema in context.SchemaRepository.Schemas.Values)
            {
                foreach (var property in schema.Properties)
                {
                    if (encripytedIds.TryGetValue(property.Key, out var apiParameter))
                    {
                        property.Value.Format = string.Empty;
                        property.Value.Type = "string";
                    }
                }
            }
        }
    }
}
