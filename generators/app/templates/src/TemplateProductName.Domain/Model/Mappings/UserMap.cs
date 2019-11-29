using FluentNHibernate.Mapping;

namespace TemplateProductName.Domain.Model.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");

            Id(x => x.Id, "id").GeneratedBy.Assigned();
            Map(x => x.Email, "email").Not.Nullable().Length(64);
            Map(x => x.LastLogin, "last_login").Nullable();
            Component(x => x.Password, p =>
            {
                p.Map(x => x.Hash, "password_hash").Not.Nullable().Length(128);
                p.Map(x => x.Salt, "password_salt").Not.Nullable().Length(128);
            });
        }
    }
}
