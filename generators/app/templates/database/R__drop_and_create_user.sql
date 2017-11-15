-- Drop (destructive!)

-- Revoke connect privs from template_product_name user if it exists.
-- Needs to be done or "drop user" will fail
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles  WHERE rolname = 'template_product_name') THEN
        revoke connect on database template_product_name from template_product_name;
        revoke all privileges on schema template_product_name from template_product_name;
        revoke all privileges on all tables in schema template_product_name from template_product_name;
    END IF;
END
$$;

drop user if exists template_product_name;

-- Create
create user template_product_name with password 'template_product_name';

-- Grant r+w to template_product_name on template_product_name schema
grant connect on database template_product_name to template_product_name;

do $$
begin
if '${environment}' = 'dev' then
    grant all privileges on schema template_product_name to template_product_name;
end if;
end
$$;

grant select, insert, update, delete on all tables in schema template_product_name to template_product_name;
grant usage on all sequences in schema template_product_name to template_product_name;
grant select on all tables in schema template_product_name to template_product_name;

