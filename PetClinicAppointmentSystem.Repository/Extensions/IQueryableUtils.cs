using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetClinicAppointmentSystem.Repository.Extensions
{
    public static class IQueryableUtils
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo queryContextFactoryField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryContextFactory");
        private static readonly FieldInfo loggerField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_logger");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSql<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            //var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            //var queryContextFactory = (IQueryContextFactory)queryContextFactoryField.GetValue(queryCompiler);
            //var logger = (Microsoft.EntityFrameworkCore.Diagnostics.IDiagnosticsLogger<DbLoggerCategory.Query>)loggerField.GetValue(queryCompiler);
            //var queryContext = queryContextFactory.Create();
            //var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            //var newQueryExpression = modelGenerator.ExtractParameters(logger, query.Expression, queryContext);
            //var queryModel = modelGenerator.ParseQuery(newQueryExpression);
            //var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            //var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            //var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            //var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();

            //modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            //var command = modelVisitor.Queries.First().CreateDefaultQuerySqlGenerator()
            //    .GenerateSql(queryContext.ParameterValues);

            //return (command.CommandText, queryContext.ParameterValues);

            return (null, null);
        }
    }
}
