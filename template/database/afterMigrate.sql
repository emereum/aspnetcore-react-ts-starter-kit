DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles  WHERE rolname = 'template_product_name') THEN
        create user template_product_name with password 'template_product_name';
    END IF;
END
$$;

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

