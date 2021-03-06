using System;
using Silanis.ESL.SDK.Internal;

namespace Silanis.ESL.SDK.Builder
{
	public class SMSAuthenticationBuilder : AuthenticationBuilder
	{
		private readonly string phoneNumber;

		public SMSAuthenticationBuilder (string phoneNumber)
		{
			this.phoneNumber = phoneNumber;
		}

		public override Authentication Build()
		{
			Support.LogMethodEntry();
			Asserts.NotEmptyOrNull (phoneNumber, "phoneNumber");
			Authentication result = new Authentication(phoneNumber);
			Support.LogMethodExit(result);
			return result;
		}
	}
}