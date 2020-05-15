using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace TemplateProductName.Persistence.PostgreSQL.Conventions
{
    /// <summary>
    /// Use PostgreSQL's text type rather than varchar. Text is equally as
    /// performant as varchar and does not impose a restriction on string
    /// length. This prevents issues where missing string length validation in
    /// the application causes a database error when a long string is inserted.
    /// All responsibility for string length validation is left to the
    /// application when using this convention.
    /// </summary>
    public class PersistStringsAsTextConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) =>
            criteria.Expect(x => x.Type == typeof(string));

        public void Apply(IPropertyInstance instance) =>
            instance.CustomSqlType("text");
    }
}
