# Database Scripts Guide

This folder contains a collection of database migration scripts. Each script is run in sequence by [flyway](https://flywaydb.org/) when you run the `setup.ps1` script. This document describes how to safely make schema changes so that the changes can be automatically run in the test and prod environments.

# Getting Started

If you've just cloned this product you need to run these scripts to set up your development environment. Follow the steps below:

* Install the command line version of [flyway](https://flywaydb.org/). If you have chocolatey installed, you can invoke this command to install flyway: `cinst flyway.commandline`
* Run `setup.ps1` by right-clicking it and choosing "Run with PowerShell"
* Follow the steps presented to you when the script runs.

# Making Schema Changes

Schema changes are performed by writing new migration scripts. Do not edit existing migration scripts! By treating migration scripts as forward-only it enables us to automatically migrate test and prod databases without having to manually backup and restore data.

Migration scripts should be named like "V042__add_dinosaur_tables.sql". Flyway will run each migration script from the lowest to highest version number. Find the highest numbered migration script and create a new migration script with the next available number, for example "V043__delete_pterodactyls.sql". Inside this migration script you can write normal SQL, but remember to prefix new structures with the schema name because these scripts are run by the postgres user:

```
create table template_product_name.dinosaurs
(
    id serial not null
        constraint attributes_pkey
            primary key,
    code text not null
        constraint attributes_code_key
            unique,
    name text not null
)
```

Here are a few important tips for writing migration scripts:

* Never edit an existing migration script if it has been committed and pushed! Migration scripts are only run forwards, they are never re-run or run backwards. If you made a mistake write a new migration script to correct the errors in the previous migration script. This is important because a script might have been automatically run by flyway against a test or prod environment. If you change that script flyway won't know how to handle it.

* Always prefix your table names with the correct schema.

* Read and understand the `setup.ps1` script so you'll know how to work directly with flyway should you need to use some of flyway's more advanced features.

# Environment-Specific Scripting

Flyway can provide variables to the migration scripts. An environment variable will be provided if you use the `setup.ps1` script. It can be used in any migration script like this:

```
do $$
begin
if '${environment}' = 'dev' then
    
    -- Invoke some commands that should only be run in dev
    
end if;
end
$$;
```