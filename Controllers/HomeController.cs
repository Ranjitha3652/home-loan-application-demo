using System.Data;
using System.Diagnostics;
using HomeLoanDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BoldSign.Api;
using BoldSign.Model;
using Microsoft.Extensions.Caching.Distributed;
using BoldSign.Model.Webhook;


namespace HomeLoanDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly ApiClient apiClient = new ApiClient("https://api.boldsign.com",Environment.GetEnvironmentVariable("APIKEY"));
        private readonly string templateId = Environment.GetEnvironmentVariable("TEMPLATEID");
        private readonly DocumentClient documentClient = new DocumentClient(apiClient);
        private readonly TemplateClient templateClient = new TemplateClient(apiClient);
        private readonly IDistributedCache _cache;
        public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        [HttpGet("home/getStatus/{id}")]
        public async Task<IActionResult> GetStatus(string id)
        {
            // Check if the document exists in the cache and its status is "completed"
            var status = await _cache.GetStringAsync(id);
            if (status == null || status != "completed")
            {
                // Document does not exist in the cache or its status is not "completed"
                return NotFound();
            }

            // Document has been completed and is ready for download
            return Ok();
        }

        [HttpPost("Home/Webhook")]
        [IgnoreAntiforgeryToken]
        // Action for Webhook
        public async Task<IActionResult> Webhook()
        {
            var sr = new StreamReader(this.Request.Body);
            var json = await sr.ReadToEndAsync();

            if (this.Request.Headers[WebhookUtility.BoldSignEventHeader] == "Verification")
            {
                return this.Ok();
            }

            // TODO: Update your webhook secret key
            var SECRET_KEY = Environment.GetEnvironmentVariable("WEBHOOKKEY");
            if (string.IsNullOrEmpty(SECRET_KEY))
            {
                _logger.LogError("Webhook secret key is not configured");
                return this.BadRequest("Webhook secret key is not configured.");
            }
            try
            {
                WebhookUtility.ValidateSignature(json, this.Request.Headers[WebhookUtility.BoldSignSignatureHeader], SECRET_KEY);
            }
            catch (BoldSignSignatureException ex)
            {
                _logger.LogError(ex, "Webhook signature validation failed");

                return this.Forbid();
            }

            var eventPayload = WebhookUtility.ParseEvent(json);
            var doc = eventPayload.Data as DocumentEvent;
            if (eventPayload.Event.EventType == WebHookEventType.Completed && doc != null)
            {
                _logger.LogInformation("Signing process completed for document {DocumentId}", doc.DocumentId);
                // Store the results in the cache with the same document ID
                _cache.SetString(doc.DocumentId, "completed", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
                // Return the ID of the document to the client
                return RedirectToAction("SignCompleted", new { doc.DocumentId });
            }
            return this.Ok();
        }
        public IActionResult SignCompleted()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoadEmploymentInfo(TemplateDetails templateDetails)
        {
            ViewBag.FormData = templateDetails;
			//TempData.Keep("FormData");
			return PartialView("~/Views/Home/_EmploymentInfo.cshtml", templateDetails);
        }
        [HttpPost]
        public IActionResult LoadPersonalInfo(TemplateDetails templateDetails)
        {
            return PartialView("~/Views/Home/_PersonalInfo.cshtml", templateDetails);
        }
        [HttpPost]
        public IActionResult LoadLoanInfo(TemplateDetails templateDetails)
        {
            return PartialView("~/Views/Home/_LoanInfo.cshtml", templateDetails);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Download the document using DocumentId
        [HttpGet]
        public async Task<IActionResult> DownloadDocument(string id)
        {
            var document = await documentClient.DownloadDocumentAsync(id).ConfigureAwait(false);
            var contentType = "application/pdf"; // Set the content type of the file
            var fileName = "Copy_LoanApplicationForm.pdf"; // Set the file name
            Response.Headers.Add("Content-Disposition", "Attachment; filename=" + fileName);
            return File(document, contentType, fileName);
        }

        [HttpPost]
        // Create EmbedSignLink for the document
        public async Task<IActionResult> SignDocument(TemplateDetails templateDetails)
        {
            if (this.templateId == null)
            {
                _logger.LogError("Template ID is not configured");
                return BadRequest("Template ID is not configured. Please set the TEMPLATEID environment variable.");
            }
            templateDetails.TemplateId = this.templateId;
            var documentDetails = new SendForSignFromTemplate()
            {
                Title = "Cubeflakes - Home Loan Application Form",
                TemplateId = templateDetails.TemplateId,
                DisableEmails = true,
                Roles = new List<Roles>()
                {
                new Roles
                {
                    SignerName = templateDetails.FirstName + templateDetails.LastName,
                    SignerEmail = templateDetails.EmailAddress,
                    RoleIndex = 1,
                    SignerType = SignerType.Signer,
                    ExistingFormFields = new List<ExistingFormField>()
                    {
                        new ExistingFormField()
                        {
                            Id = "FirstName",
                            Value = templateDetails.FirstName,
                        },
                        new ExistingFormField()
                        {
                            Id = "LastName",
                            Value = templateDetails.LastName,
                        },
                        new ExistingFormField()
                        {
                            Id = "DOB",
                            Value = templateDetails.DOB.ToString("yyy/MM/dd"),
                        },
                        new ExistingFormField()
                        {
                            Id = "SSN",
                            Value = templateDetails.SSN,
                        },
                        new ExistingFormField()
                        {
                            Id = "PhoneNumber",
                            Value = templateDetails.PhoneNumber,
                        },
                        new ExistingFormField()
                        {
                            Id = "Email",
                            Value = templateDetails.EmailAddress,
                        },
                        new ExistingFormField()
                        {
                            Id = "EmployerName",
                            Value = templateDetails.EmployerName,
                        },
                        new ExistingFormField()
                        {
                            Id = "JobTitle",
                            Value = templateDetails.JobTitle,
                        },
                        new ExistingFormField()
                        {
                            Id = "Years",
                            Value = templateDetails.Years,
                        },
                        new ExistingFormField()
                        {
                            Id = "AnnualIncome",
                            Value = templateDetails.AnnualIncome,
                        },
                        new ExistingFormField()
                        {
                            Id = "LoanAmount",
                            Value = templateDetails.LoanAmount,
                        },
                        new ExistingFormField()
                        {
                            Id = "Purpose",
                            Value = templateDetails.SelectedPurpose,
                        },
                        new ExistingFormField()
                        {
                            Id = "PropAddr",
                            Value = templateDetails.PropertyAddr,
                        },
                        new ExistingFormField()
                        {
                            Id = "PropValue",
                            Value = templateDetails.EstimatedValue,
                        }
                    }
                }
            }
            };
            // Create document from Template with the new form fields
            DocumentCreated? documentCreated = null;
            try
            {
                documentCreated = await this.templateClient
                    .SendUsingTemplateAsync(sendForSignFromTemplate: documentDetails).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create document from template");
                return BadRequest("Failed to create document. Please try again.");
            }

            if (documentCreated == null)
            {
                _logger.LogError("Document creation returned null");
                return BadRequest("Failed to create document. Please try again.");
            }

            templateDetails.DocumentId = documentCreated.DocumentId; // created in the previous step
                                                                     //Create embedded Sign URL from the document created
            var scheme = this.Request.Scheme;
            var host = this.Request.Host;
            EmbeddedSigningLink embeddedSignUrl = this.documentClient.GetEmbeddedSignLink(
                documentId: templateDetails.DocumentId,
                signerEmail: templateDetails.EmailAddress,
                DateTime.Now.AddDays(30),
                redirectUrl: $"{scheme}://{host}/Home/Responses");
            templateDetails.SignLink = embeddedSignUrl.SignLink; // This SignLink will be loaded into the iframe
            return View(templateDetails);
        }
    }
}
