using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetClinicAppointmentSystem.Filters
{
    public class DosyaYuklemeOperationFilter : IOperationFilter
    {
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {


        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //if (operation.OperationId.ToLower().Contains("dosyayukleme"))
            {
                //operation.Parameters.Clear();
                //operation.Parameters.Add(new NonBodyParameter
                //{
                //    Name = "dosya",
                //    In = "formData",
                //    Description = "Dosya Yükleme",
                //    Required = true,
                //    Type = "file"
                //});
                //operation.Consumes.Clear();
                //operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
