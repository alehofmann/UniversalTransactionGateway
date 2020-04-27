using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TikiSoft.UniversalPaymentGateway.Domain.Model;


namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    public class ErrorResponseDto
    {
        /// <summary>
        /// El commando se ejecutó exitosamente (siempre false)
        /// </summary>   
        [JsonProperty("success")]
        public bool Success { get { return false; } }

        /// <summary>
        /// Código de error
        /// </summary>
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Origen del error
        /// 'utg' si viene del Gateway, 
        /// 'processor' si viene del Processor (MercadoPago, LaPos, Posnet)
        /// 'framework' si se origina en el Framework .NET
        /// </summary>
        [JsonProperty("error_source")]
        public string ErrorSource { get; set; }

        /// <summary>
        /// Lista de errores de validación
        /// </summary>        
        [JsonProperty("error_list", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> ErrorList { get; set; }

        /// <summary>
        /// Descripción del error
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        public ErrorResponseDto()
        {

        }

        public static ErrorResponseDto FromException(ApplicationException ex)
        {
            switch (ex.GetType().ToString())
            {
                case "system.ApplicationException":
                    return new ErrorResponseDto
                    {
                        ErrorCode = "ConfigError",
                        ErrorList = new List<string>() { ex.Message },
                        ErrorMessage = "Configuración inválida o faltante inicializando el processor",
                        ErrorSource = "utg"
                    };
                default:
                    return new ErrorResponseDto
                    {
                        ErrorCode = "AlmostUnhandledException",
                        ErrorList = new List<string>() { ex.Message },
                        ErrorMessage = "Error inesperado (exception) dentro del processor",
                        ErrorSource = "utg"
                    };
            }
        }

        public static ErrorResponseDto CustomInvalidRequest(string errorDescription)
        {
            return new ErrorResponseDto
            {
                ErrorCode = "InvalidRequest",
                ErrorMessage = errorDescription,
                ErrorSource = "utg"
            };
        }

        public static ErrorResponseDto FromNotFoundInDb(string paramName, string paramValue)
        {
            return new ErrorResponseDto
            {
                ErrorCode = "NotFoundInDb",
                ErrorMessage = "El " + paramName + " '" + paramValue + "' no fue encontrado en la base de datos",
                ErrorSource = "utg"
            };
        }
        public static ErrorResponseDto FromMissingProcessor(string processorName)
        {
            return new ErrorResponseDto
            {
                ErrorCode = "MissingProcessor",
                ErrorList = processorName switch
                {
                    "" => new List<string>() { "'processor' no ha sido especificado en la URL" },
                    _ => new List<string>() { "el processor '" + processorName + "' no existe" }
                },
                ErrorMessage = "'processor' invalido o inexistente",
                ErrorSource = "utg"
            };
        }

        public static ErrorResponseDto FromProcessorResponse(BaseResponse response)
        {
            return new ErrorResponseDto
            {
                ErrorCode = response.ResultCodeString,
                ErrorList = response.ErrorList,
                ErrorMessage = response.ResultDescription,
                ErrorSource = response.ErrorSource.ToString().ToLower()
            };
        }
        public ErrorResponseDto(BaseResponse response)
        {
            ErrorCode = response.ResultCodeString;
            ErrorList = response.ErrorList;
            ErrorMessage = response.ResultDescription;
            ErrorSource = response.ErrorSource.ToString().ToLower();
        }
    }
}
