using System;
using Silanis.ESL.SDK.Internal;
using Silanis.ESL.SDK.Services;
using Silanis.ESL.SDK.Builder;

namespace Silanis.ESL.SDK
{
    /// <summary>
    /// The EslClient acts as a E-SignLive client.
    /// The EslClient has access to service classes that help create packages and retrieve resources from the client's account.
    /// </summary>
	public class EslClient
	{
		private string baseUrl;
		private PackageService packageService;
		private SessionService sessionService;
		private FieldSummaryService fieldSummaryService;
		private AuditService auditService;
        private EventNotificationService eventNotificationService;
        private CustomFieldService customFieldService;
        private GroupService groupService;
		private AccountService accountService;
		private Services.ReminderService reminderService;

        /// <summary>
        /// EslClient constructor.
        /// Initiates service classes that can be used by the client.
        /// </summary>
        /// <param name="apiKey">The client's api key.</param>
        /// <param name="baseUrl">The staging or production url.</param>
		public EslClient (string apiKey, string baseUrl)
		{
			Asserts.NotEmptyOrNull (apiKey, "apiKey");
			Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
			this.baseUrl = AppendServicePath (baseUrl);

            RestClient restClient = new RestClient(apiKey);
			packageService = new PackageService (restClient, this.baseUrl);
			sessionService = new SessionService (apiKey, this.baseUrl);
			fieldSummaryService = new FieldSummaryService (apiKey, this.baseUrl);
			auditService = new AuditService (apiKey, this.baseUrl);
            eventNotificationService = new EventNotificationService(restClient, this.baseUrl);
            customFieldService = new CustomFieldService( restClient, this.baseUrl );
            groupService = new GroupService(restClient, this.baseUrl);
			accountService = new AccountService(restClient, this.baseUrl);
			reminderService = new ReminderService(restClient, this.baseUrl);

		}

		private String AppendServicePath(string baseUrl)
		{
			if (baseUrl.EndsWith ("/")) 
			{
				baseUrl = baseUrl.Remove (baseUrl.Length - 1);
			}

			return baseUrl;
		}

        /**
         * Facilitates access to the service that could be used to add custom field
         *
         * @return  the custom field service
         */
        public CustomFieldService GetCustomFieldService() {
            return customFieldService;
        }

		public PackageId CreatePackage (DocumentPackage package)
		{
			Silanis.ESL.API.Package packageToCreate = package.ToAPIPackage ();
			PackageId id = packageService.CreatePackage (packageToCreate);

			foreach (Document document in package.Documents.Values)
			{
				packageService.UploadDocument (id, document.FileName, document.Content, document.ToAPIDocument (packageToCreate));
			}

			return id;
		}

		public PackageId CreateAndSendPackage( DocumentPackage package ) 
		{
			PackageId packageId = CreatePackage (package);
			SendPackage (packageId);
			return packageId;
		}

		public void SendPackage (PackageId id)
		{
			packageService.SendPackage (id);
		}

		public SessionToken CreateSenderSessionToken()
		{
			return sessionService.CreateSenderSessionToken();
		}

		[Obsolete]
		public SessionToken CreateSessionToken(PackageId packageId, string signerId)
		{
			return CreateSignerSessionToken(packageId, signerId); 
		}

		public SessionToken CreateSignerSessionToken(PackageId packageId, string signerId)
		{
			return sessionService.CreateSignerSessionToken (packageId, signerId);
		}

		public byte[] DownloadDocument (PackageId packageId, string documentId)
		{
			return packageService.DownloadDocument (packageId, documentId);
		}

		public byte[] DownloadEvidenceSummary (PackageId packageId)
		{
			return packageService.DownloadEvidenceSummary (packageId);
		}

		public byte[] DownloadZippedDocuments (PackageId packageId)
		{
			return packageService.DownloadZippedDocuments (packageId);
		}

		public DocumentPackage GetPackage (PackageId id)
		{
			Silanis.ESL.API.Package package = packageService.GetPackage (id);

			return new PackageBuilder (package).Build ();
		}

		public SigningStatus GetSigningStatus (PackageId packageId, string signerId, string documentId)
		{
			return packageService.GetSigningStatus (packageId, signerId, documentId);
		}

        public void UploadDocument(String fileName, byte[] fileContent, Document document, DocumentPackage documentPackage)
        {
            Silanis.ESL.API.Package packageToCreate = documentPackage.ToAPIPackage();
            packageService.UploadDocument(documentPackage.Id, fileName, fileContent, document.ToAPIDocument(packageToCreate));
        }
        
        /// <summary>
        /// BaseUrl property
        /// </summary>
		public string BaseUrl {
			get {
				return this.baseUrl;
			}
		}

        /// <summary>
        /// PackageService property
        /// </summary>
		public PackageService PackageService {
			get {
				return this.packageService;
			}
		}

        /// <summary>
        /// SessionService property
        /// </summary>
		public SessionService SessionService {
			get {
				return this.sessionService;
			}
		}

        /// <summary>
        /// FieldSummaryService property
        /// </summary>
		public FieldSummaryService FieldSummaryService {
			get {
				return this.fieldSummaryService;
			}
		}

        /// <summary>
        /// AuditService property
        /// </summary>
		public AuditService AuditService {
			get {
				return this.auditService;
			}
		}

        public EventNotificationService EventNotificationService
        {
            get
            {
                return eventNotificationService;
            }
        }

        public GroupService GroupService
        {
            get
            {
                return groupService;
            }
        }

		public AccountService AccountService
		{
			get
			{
				return accountService;
			}
		}

		public ReminderService ReminderService
		{
			get
			{
				return reminderService;
			}
		}
	}
}	
