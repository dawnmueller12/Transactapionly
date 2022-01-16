using System.ComponentModel.DataAnnotations;

namespace Transact.Data.Models.Common
{
    public class AzureADB2CConfig
    {
        [Required]
        public string Instance { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public string TenantId { get; set; }

        [Required]
        public string SignedOutCallbackPath { get; set; }

        [Required]
        public string SignUpSignInPolicyId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public string B2cExtensionAppClientId { get; set; }
    }
}
