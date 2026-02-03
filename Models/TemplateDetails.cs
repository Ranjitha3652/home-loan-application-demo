using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace HomeLoanDemo.Models
{
    public class TemplateDetails
    {
        //Personal Info
        [JsonPropertyName("firstName")] public string FirstName { get; set; }
        [JsonPropertyName("lastName")] public string LastName { get; set; }
        [JsonPropertyName("dateOfBirth")] public DateTime DOB { get; set; }
        [JsonPropertyName("socialSecurityNo")] public string SSN { get; set; }
        [JsonPropertyName("phoneNo")] public string PhoneNumber { get; set; }

        //Employment Info
        [JsonPropertyName("employerName")] public string EmployerName { get; set; }
        [JsonPropertyName("jobTitle")] public string JobTitle { get; set; }
        [JsonPropertyName("currentYearsAtWork")] public string Years { get; set; }
        [JsonPropertyName("annualIncome")] public string AnnualIncome { get; set; }

        //Loan Info
        [JsonPropertyName("loanAmount")] public string LoanAmount { get; set; }
        [JsonPropertyName("propertyAddr")] public string PropertyAddr { get; set; }
        [JsonPropertyName("estimatedValue")] public string EstimatedValue { get; set; }
        public List<SelectListItem> LoanOptions { get; set; }
        [JsonPropertyName("selectedPurpose")] public string SelectedPurpose { get; set; }
        [JsonPropertyName("signLink")] public string SignLink { get; set; }
        [JsonPropertyName("documentId")] public string DocumentId { get; set; }
        [JsonPropertyName("templateId")] public string TemplateId { get; set; }
        [JsonPropertyName("emailAddress")] public string EmailAddress { get; set; }
        public TemplateDetails()
        {
            LoanOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Purchase", Value = "Purchase" },
            new SelectListItem { Text = "Refinance", Value = "Refinance" },
            };
        }
    }
}
