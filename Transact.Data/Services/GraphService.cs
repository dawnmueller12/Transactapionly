using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using Transact.Data.Abstractions.Services;
using Transact.Data.Models.Common;
using Transact.Data.Models.Graph;
using Transact.Data.ViewModels;

namespace Transact.Data.Services
{
    public class GraphService : IGraphService
    {
        private readonly AppSettings _appSettings;
        private GraphServiceClient graphClient { get; set; }

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

        public CustomResponse CreateUser(UserVM vmObj)
        {
            CustomResponse response = new CustomResponse();
            try
            {
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
                userModel.SetB2CProfile(_appSettings.AzureADB2CConfig.Domain);

                User user = graphClient.Users
                                    .Request()
                                    .AddAsync(userModel).Result;
                response.IS_SUCCESS = true;
                response.RESPONSE = user;
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
