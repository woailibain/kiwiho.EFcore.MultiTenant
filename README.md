# EFcore MultiTenant

### What is EFcore MultiTenant
EFcore MultiTenat is build on the EF core and asp.net core.
It is committed to solve the problem that separated data to different database, schema or table by tenant which have totally different data and resource scope. 

### Workflow

to be continue

### Dependences
- dotnet core 3.1
- EntityFrameworkCore 3.1.1
#### Optional Dependences
- Pomelo.EntityFrameworkCore.MySql 3.1.1
- Npgsql.EntityFrameworkCore.PostgreSQL 3.1.1
- Microsoft.EntityFrameworkCore.SqlServer 3.1.1

### How to install
There is not published to any packages management host.
so if please clone and build the assembly 

1. clone 
``` bash
git clone https://github.com/woailibain/kiwiho.EFcore.MultiTenant.git
```
2. build
``` bash
cd src/kiwiho.EFcore.MultiTenant.Core
dotnet publish
```
3. feel free to choose one of the database implementation.
Mysql as the example:
``` bash
cd src/kiwiho.EFcore.MultiTenant.MySql
dotnet publish
```


### How to use
The library have support three traditional modes to seperate the data. But it can combine two of them into the one what you wanted.
  - connection
  - schema
  - table

There are several scenario can be use
- Only one DbContext is required multiple tenancy.
below example will use sql server.
    1. Add the multi tenant support on function ```ConfigureServices```.
    please one of the line to use.
    ``` C#
    services.AddSqlServerPerConnection<StoreDbContext>(connectionPrefix: "sqlserver_");
    //services.AddSqlServerPerTable<StoreDbContext>(connectionName: "sqlserver_default");
    //services.AddSqlServerPerSchema<StoreDbContext>(connectionName: "sqlserver_default");
    ```
    2. Add the default middleware into pipeline on function ```Configure```
    ``` C#
    app.UseMiddleware<TenantInfoMiddleware>();
    ```
    3. Add the named ```TenantName```on the http request header.
    The database name or table name or schema name will be end of the TenameName which was provided on the header.
    For sure, if the TenantName isn't existing that may occur the some kind of connection error message.

- Multiple DbContext are required.
Multiple DbContext integration were considered in the beginning of the library coding. Hence, use the property ```Key``` on the ```TenantSetting``` to match the different ```ConnectionGernerator``` by the property ```Key``` equality check or the result of function ```MatchTenantKey```.
Meanwhile, different database engine or different mode can be mixed. 
    1. Add multiple lines on function ```ConfigureServices```.
    ``` C#
    services.AddSqlServerPerSchema<StoreDbContext>("store", "sqlserver_default");
    services.AddMySqlPerTable<CustomerDbContext>("customer", "sqlserver_default");
    ```
    2. Add the default middleware in to pipeline on function ```Configure```
    ``` C#
    app.UseMiddleware<TenantInfoMiddleware>();
    ```
    By the way, there is a full example persistence on the source code on ```example/traditional_and_multiple_context``` directory. 

- Combine two modes intended for the same DbContext to reduce the number of database, schema or table. For example, you may have to set up a large quanity of database instances due to the tenant number grow up. But a part of the oridnary tenants are not necessary to have an indenpence database instance, a good way is to group the small tenants into one database and seperate them by table or schema.
To override the ```ConnectionGenerator``` and add the rules caculate which database/schema/tabase it is. For the fully implementation, please check the example source code on directory ```example/mix_mode```

- Exchange to Read/Write splitting.
There is more complex than preceding scenario, please reference the example ```example/read_write_splitting``` directly.

### More information
This library was come here when I am writing the blog.
As you the title you seen, there are writed by Chinese, hope you can get the guidence on the example along with the source code here.
Please feel free to reference below blogs.
[利用多租户模式演化成分库分表和读写分离](https://www.cnblogs.com/woailibian/p/12391163.html)
[EF多租户实例：如何快速实现和同时支持多个DbContext](https://www.cnblogs.com/woailibian/p/12464858.html)
[EF多租户实例：快速实现分库分表](https://www.cnblogs.com/woailibian/p/12632019.html)
[EF多租户实例：演变为读写分离](https://www.cnblogs.com/woailibian/p/12773998.html)