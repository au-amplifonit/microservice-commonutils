using Fox.Microservices.CommonUtils.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fox.Microservices.CommonUtils
{
	public interface IFoxDataService
	{
		T GetGlobalParameterValue<T>(string parameterName, T defaultValue = default(T), string shopCode = "*");
		CM_B_COUNTER GetNewCounter(string entityName, string fieldName, string shopCode, string userName);
		bool CheckAddressCityData(string StateCode, string City, ref string AreaCode, ref string ZipCode, out short? cityCounter, out string errorMessage);
		void InsertCity(dynamic address);
		string[] GetScreeningServiceCodes(string shopCode);
		string[] GetSpecialAvailabilityServiceCodes(string shopCode);
		string[] GetUnavailabilityServiceCodes(string shopCode);
	}
}
