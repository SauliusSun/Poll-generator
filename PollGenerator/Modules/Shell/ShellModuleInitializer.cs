using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;

namespace PollGenerator.Shell
{
    public class ShellModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            globalServices.AddNew<EnterpriseLibraryAuthorizationService, IAuthorizationService>();
            globalServices.AddNew<SiteMapBuilderService, ISiteMapBuilderService>();
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            // TODO: register services that can be accesed only by the Shell module
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Apie", "~/Apie.aspx", "Apie", "Apie Klausiu.lt");
            siteMapBuilderService.AddNode(moduleNode);

            SiteMapNodeInfo AtsakykModuleNode = new SiteMapNodeInfo("Atsakyk", "~/Atsakyk.aspx", "Atsakyk!", "Naujausiø apklausø sàraðas");
            siteMapBuilderService.AddNode(AtsakykModuleNode);

            SiteMapNodeInfo PrisijungtiModuleNode = new SiteMapNodeInfo("Prisijungti", "~/Prisijungti.aspx", "Prisijungti", "Vartotojo prisijungimas");
            siteMapBuilderService.AddNode(PrisijungtiModuleNode, "Anonimai"); 

            SiteMapNodeInfo RegistracijaModuleNode = new SiteMapNodeInfo("Registracija", "~/Registracija.aspx", "Registracija", "Vartotojo registracija");
            siteMapBuilderService.AddNode(RegistracijaModuleNode, "Anonimai");
            
            siteMapBuilderService.RootNode.Url = "~/Apie.aspx";
            siteMapBuilderService.RootNode.Title = "Klausiu.lt";

            // TODO: register other site map nodes for pages that belong to the website root
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            IAuthorizationRulesService authorizationRuleService = services.Get<IAuthorizationRulesService>();
            if (authorizationRuleService != null)
            {
                AuthorizationConfigurationSection authorizationSection = moduleConfiguration.GetSection(AuthorizationSection) as AuthorizationConfigurationSection;
                if (authorizationSection != null)
                {
                    foreach (AuthorizationRuleElement ruleElement in authorizationSection.ModuleRules)
                    {
                        authorizationRuleService.RegisterAuthorizationRule(ruleElement.AbsolutePath, ruleElement.RuleName);
                    }
                }
            }
        }
    }
}
