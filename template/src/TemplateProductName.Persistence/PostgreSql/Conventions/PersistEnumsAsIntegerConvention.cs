using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace TemplateProductName.Persistence.PostgreSQL.Conventions
{
    public class PersistEnumsAsIntegerConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) =>
            criteria.Expect(x => x.Property.PropertyType.IsEnum);

        public void Apply(IPropertyInstance instance) =>
            instance.CustomType(instance.Property.PropertyType);
    }
}
