using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;

namespace PollGenerator.Poll
{
    public class PollModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));

            container.RegisterTypeMapping<IPollController, PollController>();
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            // TODO: add a service that will be visible to any module
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            // TODO: add a service that will be visible to this module
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("ManoApklausos", "~/Poll/ManoApklausos.aspx", "Apklausos");
            siteMapBuilderService.AddNode(moduleNode, "Registruotas");

            SiteMapNodeInfo moduleNode2 = new SiteMapNodeInfo("NaujaApklausa", "~/Poll/NaujaApklausa.aspx", "Kurti");
            siteMapBuilderService.AddNode(moduleNode2, "Registruotas");

            SiteMapNodeInfo KontaktaiModuleNode = new SiteMapNodeInfo("Kontaktai", "~/Poll/Kontaktai.aspx", "Kontaktai");
            siteMapBuilderService.AddNode(KontaktaiModuleNode, "Registruotas");


            // TODO: register other site map nodes that Poll module might provide            
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
