using Dawn;
using Fox.Microservices.CommonUtils.Models.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WebAPITools.EntityMapper;
using WebAPITools.Models.Configuration;

namespace Fox.Microservices.CommonUtils
{
	public class FoxDataService : IFoxDataService
	{
		private readonly IOptions<AppSettings> Settings;
		private readonly CommonUtilsContext DBContext;

		public FoxDataService(IOptions<AppSettings> ASettings, CommonUtilsContext ADBContext)
		{
			Settings = ASettings;
			DBContext = ADBContext;
		}

		public T GetGlobalParameterValue<T>(string parameterName, T defaultValue = default(T), string shopCode = "*")
		{
			T Result = defaultValue;

			//Search for shop param
			SY_GLOBAL_PARAMETER param = DBContext.SY_GLOBAL_PARAMETER.SingleOrDefault(E => E.PARAMETER_NAME == parameterName && E.DT_START <= DateTime.Today && E.DT_END >= DateTime.Today && E.SHOP_CODE == shopCode);
			if (param == null) //Search for global param
				param = DBContext.SY_GLOBAL_PARAMETER.SingleOrDefault(E => E.PARAMETER_NAME == parameterName && E.DT_START <= DateTime.Today && E.DT_END >= DateTime.Today && E.SHOP_CODE == "*");

			if (param != null)
			{
				ValueString paramValue = ValueString.Of(param.PARAMETER_VALUE);
				Result = paramValue.As<T>();
			}
				
			return Result;
		}
		
		public CM_B_COUNTER GetNewCounter(string entityName, string fieldName, string shopCode, string userName)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			parameters.Add(new SqlParameter("COMPANY_CODE", Settings.Value.CompanyCode));
			parameters.Add(new SqlParameter("DIVISION_CODE", Settings.Value.DivisionCode));
			parameters.Add(new SqlParameter("SHOP_CODE", shopCode));
			parameters.Add(new SqlParameter("LAPTOP_CODE", shopCode));
			parameters.Add(new SqlParameter("TABLE_NAME", entityName));
			parameters.Add(new SqlParameter("FIELD_NAME", fieldName));
			parameters.Add(new SqlParameter("USERUPDATE", userName));
			/*
			SqlParameter param = new SqlParameter("DATECOSTRAINT", SqlDbType.Date);
			param.Value = null;
			parameters.Add(param);
			parameters.Add(new SqlParameter("RESULTPREFIX", null));
			*/

			CM_B_COUNTER Result = DBContext.CM_B_COUNTER.FromSql("EXECUTE dbo.p_CM_B_COUNTER_GetNextId  @COMPANY_CODE, @DIVISION_CODE, @SHOP_CODE, @LAPTOP_CODE, @TABLE_NAME, @FIELD_NAME, @USERUPDATE, @DATECOSTRAINT=NULL, @RESULTPREFIX=NULL", parameters.ToArray()).FirstOrDefault();
			return Result;
		}

		public bool CheckAddressCityData(string StateCode, string City, ref string AreaCode, ref string ZipCode, out short? cityCounter, out string errorMessage)
		{
			if (string.IsNullOrWhiteSpace(StateCode))
				throw new ArgumentNullException(nameof(StateCode));
			if (string.IsNullOrWhiteSpace(ZipCode))
				throw new ArgumentNullException(nameof(ZipCode));

			cityCounter = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(AreaCode))
			{
				AreaCode = DBContext.CM_S_STATE_EXT_AUS.FirstOrDefault(E => E.STATE_CODE == StateCode)?.DEFAULT_AREA_CODE;

				if (string.IsNullOrWhiteSpace(AreaCode))
					throw new InvalidOperationException($"Cannot find State for code '{StateCode}'");
			};

			if (City != null)
			{
				string Area = AreaCode;
				string Zip = ZipCode;
				/*
				var predicate = PredicateBuilder.New<CM_S_CITY_BOOK>(E => E.CITY_NAME == City.ToUpperInvariant());
				if (!string.IsNullOrWhiteSpace(AreaCode))
					predicate = predicate.And(E => E.AREA_CODE == Area);
				if (!string.IsNullOrWhiteSpace(ZipCode))
					predicate = predicate.And(E => E.ZIP_CODE == Zip);
				*/
				var predicate = PredicateBuilder.New<CM_S_CITY_BOOK>(true);
				if (!string.IsNullOrWhiteSpace(AreaCode))
					predicate = predicate.And(E => E.AREA_CODE == Area);
				if (!string.IsNullOrWhiteSpace(ZipCode))
					predicate = predicate.And(E => E.ZIP_CODE == Zip);

				CM_S_CITY_BOOK city = DBContext.CM_S_CITY_BOOK.FirstOrDefault(predicate);
				if (city == null)
				{
					errorMessage = $"City '{City}'";
					if (!string.IsNullOrWhiteSpace(AreaCode))
						errorMessage += $" and Area Code '{AreaCode}'";
					if (!string.IsNullOrWhiteSpace(ZipCode))
						errorMessage += $"and Zip code '{ZipCode}'";
					errorMessage += $" are not correct, please check data.";
					return false;
				}

				AreaCode = city?.AREA_CODE;
				ZipCode = city?.ZIP_CODE;
				cityCounter = city?.CITY_COUNTER ?? 0;
			}

			return true;
		}

		public void InsertCity(dynamic address)
		{
			CM_S_CITY_BOOK Item = new CM_S_CITY_BOOK();
			EntityMapper.UpdateEntity(address, Item);
			Item.DT_INSERT = Item.DT_UPDATE = DateTime.UtcNow;
			Item.USERINSERT = Item.USERUPDATE = Settings.Value.Username;
			Item.CITY_NAME = Item.CITY_NAME.ToUpperInvariant();
			Item.ROWGUID = Guid.NewGuid();
			if (string.IsNullOrWhiteSpace(Item.COUNTRY_CODE))
				Item.COUNTRY_CODE = Settings.Value.CountryCode;

			CM_S_AREA_BOOK Area = null;
			if (!string.IsNullOrWhiteSpace(Item.AREA_CODE))
				Area = DBContext.CM_S_AREA_BOOK.FirstOrDefault(E => E.AREA_CODE == Item.AREA_CODE);

			if (Area == null) //Revert to default state area, if possibile
			{
				string StateCode = address.StateCode;
				CM_S_STATE_EXT_AUS State = DBContext.CM_S_STATE_EXT_AUS.FirstOrDefault(E => E.STATE_CODE == StateCode);
				if (State == null)
					throw new InvalidOperationException($"Cannot find a state with state code = '{StateCode}'");

				Item.AREA_CODE = State.DEFAULT_AREA_CODE;
			}
			Area = DBContext.CM_S_AREA_BOOK.FirstOrDefault(E => E.AREA_CODE == Item.AREA_CODE);
			if (Area == null)
				throw new InvalidOperationException($"Cannot find area with code = '{Item.AREA_CODE}' for state '{address.StateCode}'");

			CM_S_CITY_BOOK City = DBContext.CM_S_CITY_BOOK.Where(E => E.COUNTRY_CODE == Item.COUNTRY_CODE && E.AREA_CODE == Item.AREA_CODE && E.ZIP_CODE == Item.ZIP_CODE).OrderByDescending(E => E.CITY_COUNTER).FirstOrDefault();
			Item.CITY_COUNTER = (short)(City != null ? City.CITY_COUNTER + 1 : 1);

			DBContext.CM_S_CITY_BOOK.Add(Item);
			DBContext.SaveChanges();
		}

		public string[] GetScreeningServiceCodes(string shopCode)
		{
			string serviceCodes = GetGlobalParameterValue<string>("APPOINTMENT_TYPE_FOR_SCREENING_ROOM", "SCREEN01", shopCode);
			string[] Result = serviceCodes.Split("|", StringSplitOptions.RemoveEmptyEntries);
			return Result;
		}

		public string[] GetSpecialAvailabilityServiceCodes(string shopCode)
		{
			string serviceCodes = GetGlobalParameterValue<string>("AG_S_SERVICE_TYPE_DISPONIBILITASPECIALI", "02", shopCode);
			string[] Result = serviceCodes.Split("|", StringSplitOptions.RemoveEmptyEntries);
			return Result;
		}

		public string[] GetUnavailabilityServiceCodes(string shopCode)
		{
			string serviceCodes = GetGlobalParameterValue<string>("AG_S_SERVICE_TYPE_INDISPONIBILITA", "01", shopCode);
			string[] Result = serviceCodes.Split("|", StringSplitOptions.RemoveEmptyEntries);
			return Result;
		}
	}
}
