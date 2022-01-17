using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using Transact.Data.Abstractions.Services;
using Transact.Data.Helper;
using Transact.Data.Models.Common;
using Transact.Data.Models.Graph;
using Transact.Data.ViewModels;

namespace Transact.Data.Services
{
    public class GraphService : IGraphService
    {
        private readonly AppSettings _appSettings;
        private GraphServiceClient graphClient { get; set; }

        // Declare the names of the custom attributes
        private const string customAttributeName1 = "RoleID";
        private const string customAttributeName2 = "RoleName";

        public GraphService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            // Initialize the client credential auth provider
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(
                _appSettings.AzureADB2CConfig.TenantId,
                _appSettings.AzureADB2CConfig.ClientId,
                _appSettings.AzureADB2CConfig.ClientSecret);
            graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }

        public CustomResponse CreateUser(UserVM vmObj, RoleVM roleVMObj)
        {
            CustomResponse response = new CustomResponse();
            try
            {
                // Get the complete name of the custom attribute (Azure AD extension)
                B2cCustomAttributeHelper helper = new B2cCustomAttributeHelper(_appSettings.AzureADB2CConfig.B2cExtensionAppClientId);
                string roleIdAttributeName = helper.GetCompleteAttributeName(customAttributeName1);
                string roleNameAttributeName = helper.GetCompleteAttributeName(customAttributeName2);


                // Fill custom attributes
                IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
                extensionInstance.Add(roleIdAttributeName, roleVMObj.Id);
                extensionInstance.Add(roleNameAttributeName, roleVMObj.Name);

                UserModel userModel = new UserModel();
                userModel.Password = vmObj.Password;
                userModel.DisplayName = vmObj.FirstName + " " + vmObj.LastName;
                userModel.Surname = vmObj.LastName;
                userModel.GivenName = vmObj.FirstName;
                ObjectIdentity objIdn = new ObjectIdentity();
                userModel.Identities = new List<ObjectIdentity>(){
                    new ObjectIdentity() {
                    SignInType = "emailAddress",
                    IssuerAssignedId = vmObj.Email
                    }
                };
                userModel.AdditionalData = extensionInstance;

                userModel.SetB2CProfile(_appSettings.AzureADB2CConfig.Domain);

                User user = graphClient.Users
                                    .Request()
                                    .AddAsync(userModel).Result;
                response.IS_SUCCESS = true;
                response.RESPONSE = user;
                var result = graphClient.Users[user.Id]
                   .Request()
                   .Select($"id,givenName,surName,displayName,identities,{roleIdAttributeName},{roleNameAttributeName}")
                   .GetAsync().Result;

            }
            catch (Exception exp)
            {
                if (exp.InnerException is ServiceException)
                {
                    response.MESSAGE = "Unable to add user in AD due to " + ((ServiceException)exp.InnerException).Error.Message;
                }
                else
                {
                    response.MESSAGE = "Unable to add user in AD due to " + exp.ToString();
                }
                response.IS_SUCCESS = false;
            }
            return response;
        }
    }
}
