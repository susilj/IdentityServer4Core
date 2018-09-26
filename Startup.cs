// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4Core.Data;
using IdentityServer4Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.HttpsPolicy;
using IdentityServer4Core.Managers;

namespace IdentityServer4Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            /// Store assembly for migrations

            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            /// Replace DbContext database from SqLite in template to Postgres
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<CustomUserManager>();
                //.AddRoleManager<CustomRoleManager>();

            services.AddMvc();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                // options.Events.RaiseErrorEvents = true;
                // options.Events.RaiseInformationEvents = true;
                // options.Events.RaiseFailureEvents = true;
                // options.Events.RaiseSuccessEvents = true;
            })
                // .AddInMemoryIdentityResources(Config.GetIdentityResources())
                // .AddInMemoryApiResources(Config.GetApis())
                // .AddInMemoryClients(Config.GetClients())
                /// Use Postgres database for storing configuration data
                .AddConfigurationStore(configDb => {
                    configDb.ConfigureDbContext = db => db.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                /// Use Postgres database for storing operational data
                .AddOperationalStore(operationalDb => {
                    operationalDb.ConfigureDbContext = db => db.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "708996912208-9m4dkjb5hscn7cjrn5u0r4tbgkbj1fko.apps.googleusercontent.com";
                    options.ClientSecret = "wdfPY6t8H8cecgjlxud__4Gh";
                });

            services.AddTransient<TenantManager>();
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);
            
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using(IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                PersistedGrantDbContext persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                persistedGrantDbContext.Database.Migrate();

                ConfigurationDbContext configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();

                if(!configurationDbContext.Clients.Any())
                {
                    foreach(var client in Config.GetClients())
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }

                if(!configurationDbContext.IdentityResources.Any())
                {
                    foreach(var resource in Config.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }

                if(!configurationDbContext.ApiResources.Any())
                {
                    foreach(var api in Config.GetApis())
                    {
                        configurationDbContext.ApiResources.Add(api.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }
            }
        }
    }
}
