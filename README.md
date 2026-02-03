# Home Loan Application Demo

A comprehensive ASP.NET Core web application demonstrating a multi-step home loan application process with integrated electronic signatures using **BoldSign API**.

## Overview

This demo showcases:
- **Multi-step form** with personal, employment, and loan information collection
- **Embedded signature workflow** using BoldSign for document signing
- **Real-time document status tracking** via webhooks
- **Template-based document generation** with pre-filled form fields
- **Responsive UI** with progress indicators and form validation

## Prerequisites

Before running this application, ensure you have:

- **.NET SDK 7.0** or later ([Download](https://dotnet.microsoft.com/download))
- **BoldSign Account** with API access ([Sign up](https://www.boldsign.com))
- **API Key** from BoldSign account
- **Template ID** of your home loan template in BoldSign
- **Webhook Secret Key** for webhook signature validation

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd HomeLoanDemo
```

### 2. Set Up Environment Variables

Create a `.env` file in the project root (or set system environment variables):

```env
APIKEY=your_boldsign_api_key_here
TEMPLATEID=your_template_id_here
WEBHOOKKEY=your_webhook_secret_key_here
```

**Or set environment variables in your shell:**

**Windows (PowerShell):**
```powershell
[Environment]::SetEnvironmentVariable("APIKEY", "your_key", "User")
[Environment]::SetEnvironmentVariable("TEMPLATEID", "your_template_id", "User")
[Environment]::SetEnvironmentVariable("WEBHOOKKEY", "your_webhook_key", "User")
```

**macOS/Linux (Bash):**
```bash
export APIKEY="your_boldsign_api_key_here"
export TEMPLATEID="your_template_id_here"
export WEBHOOKKEY="your_webhook_secret_key_here"
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run the Application

**Development:**
```bash
dotnet run
```

The application will start at `https://localhost:7179` (or `http://localhost:5183`)

**Production Build:**
```bash
dotnet publish -c Release
```

## Project Structure

```
HomeLoanDemo/
├── Controllers/
│   └── HomeController.cs          # Main application logic
├── Models/
│   ├── TemplateDetails.cs         # Form data model
│   └── ErrorViewModel.cs          # Error page model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Main application form
│   │   ├── SignDocument.cshtml    # Document signing page
│   │   ├── SignCompleted.cshtml   # Completion confirmation
│   │   ├── _PersonalInfo.cshtml   # Personal details partial
│   │   ├── _EmploymentInfo.cshtml # Employment details partial
│   │   └── _LoanInfo.cshtml       # Loan details partial
│   └── Shared/
│       ├── _Layout.cshtml         # Main layout
│       └── Error.cshtml           # Error page
├── wwwroot/
│   ├── css/                       # Stylesheets
│   ├── js/                        # Client-side scripts
│   └── assets/                    # Images and fonts
├── Program.cs                     # Application startup
├── appsettings.json              # Configuration
└── HomeLoanDemo.csproj           # Project file

```

## Features

### Form Workflow
1. **Personal Information** - Collect name, date of birth, SSN, contact details
2. **Employment Information** - Gather employer details, job title, income
3. **Loan Information** - Capture loan amount, property details, loan purpose
4. **Document Signing** - Generate and display embedded signature link
5. **Completion** - Confirm signed document and provide download option

### Key Technologies
- **ASP.NET Core 7.0** - Web framework
- **BoldSign API** - Electronic signature integration
- **Razor Views** - Server-side templating
- **Bootstrap** - Responsive UI framework
- **jQuery** - Client-side interactions

## Configuration

Key settings in `appsettings.json`:
- **AllowedHosts** - Configure for your deployment domain
- **Logging** - Adjust log levels for debugging

## Webhook Integration

The application includes a webhook endpoint (`/Home/Webhook`) to track document signing completion:

- **Endpoint**: `POST /Home/Webhook`
- **Signature Validation**: All webhook requests are validated using the `WEBHOOKKEY`
- **Completion Tracking**: Signed documents are cached for download availability

## Files of Interest

- `Controllers/HomeController.cs` - Contains document creation, signing, and webhook handling logic
- `Views/Home/_LoanInfo.cshtml` - Loan details form with numeric input validation
- `Views/Home/Index.cshtml` - Main multi-step form with progress tracking
- `Models/TemplateDetails.cs` - Data model for loan application form

## Error Handling

The application includes:
- Null reference validation for environment variables
- Try-catch blocks for API operations
- User-friendly error messages
- Logging for debugging

## Security Considerations

✅ **Best Practices Implemented:**
- **Environment variables** for sensitive data (API keys, secrets)
- **Webhook signature validation** using BoldSign's `WebhookUtility.ValidateSignature()`
- **HTTPS enforcement** in production via `app.UseHttpsRedirection()` and HSTS
- **CSRF token protection** on all POST forms using `@Html.AntiForgeryToken()`
- **Input validation** on form fields with HTML5 attributes and client-side validation
- **Null reference checks** for safe environment variable handling
- **Exception handling** for API operations with user-friendly error messages

## Troubleshooting

**"Template ID is not configured"**
- Ensure the `TEMPLATEID` environment variable is set correctly
- Verify the template exists in your BoldSign account

**Webhook validation failed**
- Check that the `WEBHOOKKEY` environment variable matches your BoldSign webhook secret
- Verify webhook headers are being sent correctly

**API connection errors**
- Confirm your `APIKEY` is valid and has appropriate permissions
- Check your internet connection

## Support

For issues with BoldSign API:
- [BoldSign Documentation](https://developers.boldsign.com/api-overview/getting-started/?region=us)
- [BoldSign Support](https://support.boldsign.com)

## License

This demo application is provided as-is for learning and demonstration purposes.

## Next Steps

- Customize the loan application form to match your requirements
- Add additional form fields to the template in BoldSign
- Deploy to Azure App Service or your preferred hosting platform
- Integrate with your backend loan processing system
