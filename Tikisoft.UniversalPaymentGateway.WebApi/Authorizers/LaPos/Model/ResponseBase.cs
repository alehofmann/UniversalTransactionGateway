using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class ResponseBase
    {
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public string HostCode { get; set; }
        public string HostText
        {
            get
            {
                return ParseCode(HostCode);
            }
        }
        public string ResponseText
        {
            get
            {
                return ParseCode(ResponseCode is null?HostCode:ResponseCode);
            }
        }
        public string ParseCode(string code)
        {                     
                switch (code)
                {
                    case "00":
                        return  "APROBADA";
                    case "01":
                    case "02":
                        return  "PEDIR AUTORIZACION";

                    case "03":

                        return "COMERCIO INVALIDO";

                    case "04":

                        return "CAPTURAR TARJETA";

                    case "05":

                        return "DENEGADA";

                    case "07":

                        return "RETENGA Y LLAME";

                    case "08":

                        return "APROBADA PEDIR IDENTIFICACION";

                    case "11":

                        return "APROBADA";

                    case "12":

                        return "TRANSAC. INVALIDA";

                    case "13":

                        return "MONTO INVALIDO";

                    case "14":

                        return "TARJETA INVALIDA";

                    case "19":

                        return "REINICIE OPERACIÓN";
                    case "25":
                        return "NO EXISTE ORIGINAL";
                    case "30":
                        return "ERROR EN FORMATO";
                    case "38":
                        return "EXCEDE ING.DE PIN";
                    case "43":
                        return "RETENER TARJETA";
                    case "45":
                        return "NO OPERA EN CUOTAS";
                    case "46":
                        return "TARJETA NO VIGENTE";
                    case "47":
                        return "PIN REQUERIDO";
                    case "48":
                        return "EXCEDE MAX. CUOTAS";
                    case "49":
                        return "ERROR FECHA VENCIM.";
                    case "50":
                        return "ENTREGA SUPERA LIMITE";
                    case "51":
                        return "FONDOS INSUFICIENTES";
                    case "53":
                        return "CUENTA INEXISTENTE";
                    case "54":
                        return "TARJETA VENCIDA";
                    case "55":
                        return "PIN INCORRECTO";
                    case "56":
                        return "TARJ. NO HABILITADA";
                    case "57":
                        return "TRANS. NO PERMITIDA";
                    case "58":
                        return "SERVICIO INVALIDO";
                    case "61":
                        return "EXCEDE LIMITE";
                    case "65":
                        return "EXCEDE LIM. TARJETA";
                    case "76":
                        return "LLAMAR AL EMISOR – ERROR – DESC. PROD.";
                    case "77":
                        return "ERROR PLAN/CUOTAS – ERROR RECONCILIACIÓN";
                    case "85":
                        return "APROBADA – LOTE NO ENCONTRADA";
                    case "88":
                        return "APROB. CLIENTE LLAME";
                    case "89":
                        return "TERMINAL INVALIDA";
                    case "91":
                        return "EMISOR FUERA LINEA";
                    case "94":
                        return "NRO. SEC. DUPLICAD";
                    case "95":
                        return "RE-TRANSMITIENDO";
                    case "96":
                        return "ERROR EN SISTEMA – MENSAJE INVALIDO";
                    case "BB":
                        return "COMP NO DISPONIBLE INTENTE MAS TARDE";
                    case "000":
                        return "Operación exitosa";
                    case "001":
                        return "Quedan registros por enviar";
                    case "011":
                        return "Sin lotes abiertos";
                    case "102":
                        return "El número de ticket no existe";
                    case "103":
                        return "El código de plan no existe";
                    case "104":
                        return "El índice de registro no existe";
                    case "201":
                        return "Transacción cancelada por el usuario";
                    case "202":
                        return "La tarjeta deslizada por el usuario no coincide con la pedida";
                    case "203":
                        return "La tarjeta deslizada no es válida";
                    case "204":
                        return "La tarjeta deslizada está vencida";
                    case "205":
                        return "Transacción original inexistente";
                    case "206":
                        return "No hay transacciones en el lote";
                    case "301":
                        return "El POS no pudo comunicarse con el host";
                    case "302":
                        return "El POS no pudo imprimir el ticket";
                    case "901":
                        return "Nombre del comando inexistente";
                    case "902":
                        return "Largo de los parámetros inválido para este comando";
                    case "903":
                        return "Formato de algún parámetro no es correcto";
                    case "909":
                        return "Error general en la operación";
                    default:
                        return "RECHAZADA (" + HostCode + ")";

                }                
        }

        public int AuthCode { get; set; }
        public int VoucherCode { get; set; }
        public int BatchNumber { get; set; }
        public string CustomerName { get; set; }
        public int CardLast4 { get; set; }
        public int CardFirst6 { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TerminalId { get; set; }
    }
}
