using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public class BaseResponse
    {
        public enum ResultCodesEnum
        {
            Approved = 1,
            Timeout = 2,
            UserCancel = 3,
            Rejected = 4,
            InvalidParametersError = 6,
            CommunicationsError = 7,
            ProcessorError = 8,
            InternalProcessorError = 9,
            OperationNotSupported=10,
            NotReady=11

        }

        public enum ErrorSourceEnum
        {
            NoError = 0,
            Utg = 1,
            Processor = 2
        }

        public BaseResponse(ResultCodesEnum resultCode, IList<string> errorList = null)
        {
            ResultCode = resultCode;
            ErrorList = errorList;

            switch (resultCode)
            {
                case ResultCodesEnum.InvalidParametersError:
                    ResultDescription = "Uno o más parámetros inválidos";
                    break;
                case ResultCodesEnum.Approved:
                    ResultDescription = "Transacción aprobada";
                    break;
                case ResultCodesEnum.Timeout:
                    ResultDescription = "Timeout procesando la transacción";
                    break;
                case ResultCodesEnum.Rejected:
                    ResultDescription = "Transacción rechazada";
                    break;
                case ResultCodesEnum.OperationNotSupported:
                    ResultDescription = "Operación no compatible con este processor";
                    break;
                case ResultCodesEnum.NotReady:
                    ResultDescription = "El autorizador no está listo para operar";
                    break;
            }
        }


        public BaseResponse(ResultCodesEnum resultCode, string errorDescription)
        {
            ResultCode = resultCode;
            ResultDescription = errorDescription;
        }

        public BaseResponse() { }
        public bool Success
        {
            get
            {
                return (ResultCode == ResultCodesEnum.Approved ||
                    ResultCode == ResultCodesEnum.Rejected ||
                    ResultCode == ResultCodesEnum.UserCancel ||
                    ResultCode == ResultCodesEnum.Timeout);
            }
        }

        public ErrorSourceEnum ErrorSource
        {
            get
            {
                switch (ResultCode)
                {
                    case ResultCodesEnum.ProcessorError:
                        return (ErrorSourceEnum.Processor);
                    case ResultCodesEnum.InvalidParametersError:
                        return (ErrorSourceEnum.Utg);
                    case ResultCodesEnum.CommunicationsError:
                        return (ErrorSourceEnum.Utg);
                    case ResultCodesEnum.OperationNotSupported:
                        return (ErrorSourceEnum.Utg);
                    case ResultCodesEnum.NotReady:
                        return (ErrorSourceEnum.Utg);
                    default:
                        return (ErrorSourceEnum.NoError);
                }
            }
        }
        public ResultCodesEnum ResultCode { get; set; }

        public string ResultCodeString
        {
            get
            {
                return ResultCode.ToString();
            }
        }
        public IList<string> ErrorList { get; set; }
        public string ResultDescription { get; set; }
        public string ProcessorResponse { get; set; }
    }
}
