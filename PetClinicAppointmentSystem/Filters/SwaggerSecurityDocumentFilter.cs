using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetClinicAppointmentSystem.Filters
{
    public class SwaggerSecurityDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="context"></param>
        //public void Apply(SwaggerDocument document, DocumentFilterContext context)
        //{
        //    document.Security = new List<IDictionary<string, IEnumerable<string>>>()
        //    {
        //        new Dictionary<string, IEnumerable<string>>()
        //        {
        //            { "Bearer", new string[]{ } },
        //            { "Basic", new string[]{ } },
        //        }
        //    };
        //}

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //swaggerDoc.Security = new List<IDictionary<string, IEnumerable<string>>>()
            //{
            //    new Dictionary<string, IEnumerable<string>>()
            //    {
            //        { "Bearer", new string[]{ } },
            //        { "Basic", new string[]{ } },
            //    }
            //};
        }
    }
}
