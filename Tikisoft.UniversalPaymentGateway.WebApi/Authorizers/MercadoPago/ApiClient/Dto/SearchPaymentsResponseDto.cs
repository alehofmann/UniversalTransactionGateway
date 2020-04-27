using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient
{    
    public class SearchPaymentsResponseDto
    {
        public int TotalPayments { get; set; }
        
        public int Status { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }        

        public string RequestUri { get; set; }
        public string ErrorMessage
        {
            get
            {
                if (Status == 404)
                {
                    return ("Route Not Found: " + RequestUri);
                }
                else
                {
                    return Message;
                }
            }
        }
        public IList<PaymentItemDto> Payments { get; set; } = new List<PaymentItemDto>();
        
        public static async Task<SearchPaymentsResponseDto> ParseAsync(System.IO.Stream stream)
        {
            var retVal = new SearchPaymentsResponseDto();
            var doc = await JsonDocument.ParseAsync(stream);
            var root = doc.RootElement;

            JsonElement elem;
            if (root.TryGetProperty("status", out elem))
            {
                //retVal.Status = root.GetProperty("status").GetInt32();
                retVal.Status = elem.GetInt32();
            }

            if (root.TryGetProperty("message", out elem))
            {
                //retVal.Message = root.GetProperty("message").GetString();
                retVal.Message= elem.GetString();
            }
            
            if(retVal.Status!=0)
            {
                retVal.Status = root.GetProperty("status").GetInt32();
                retVal.Message = root.GetProperty("message").GetString();
                return (retVal);
            }
            
            retVal.TotalPayments=root.GetProperty("paging").GetProperty("total").GetInt32();
            retVal.Payments=root.GetProperty("results").EnumerateArray()
                .Select(x =>
                {
                    PaymentItemDto GetItem()
                    {

                        var retVal = new PaymentItemDto();                        

                        retVal.Payment_Status = x.GetProperty("status").GetString();
                        if(retVal.Payment_Status=="approved")
                        {
                        retVal.PaymentType = x.GetProperty("payment_type_id").GetString();
                        retVal.MoneyReleaseDate = x.GetProperty("money_release_date").GetDateTime();
                        retVal.ExtReference = x.GetProperty("external_reference").ToString();
                        retVal.AuthCode = x.GetProperty("authorization_code").ToString();
                        retVal.Payment_Status_Detail = x.GetProperty("status_detail").GetString();
                        retVal.Payment_MethodId = x.GetProperty("payment_method_id").GetString();
                        retVal.Payment_TypeId = x.GetProperty("payment_type_id").GetString();
                        //Payer_Email = x.GetProperty("payer").GetProperty("email").GetString(),
                        //Payer_FirstName = x.GetProperty("payer").GetProperty("first_name").GetString(),
                        //Payer_LastName = x.GetProperty("payer").GetProperty("last_name").GetString(),
                        retVal.TotalPaidAmount = x.GetProperty("transaction_details").GetProperty("total_paid_amount").GetDecimal();
                        retVal.NetReceivedAmount = x.GetProperty("transaction_details").GetProperty("net_received_amount").GetDecimal();
                        retVal.Installments = x.GetProperty("installments").GetInt32();
                        retVal.InstallmentAmount = x.GetProperty("transaction_details").GetProperty("installment_amount").GetDecimal();
                        retVal.FeeDetail = x.GetProperty("fee_details").EnumerateArray()
                                .Select(y =>
                                {
                                    Dto.FeeDetailItemDto GetItem()
                                    {
                                        return new Dto.FeeDetailItemDto()
                                        {
                                            Amount = y.GetProperty("amount").GetDecimal(),
                                            FeePayer = y.GetProperty("fee_payer").GetString(),
                                            FeeType = y.GetProperty("type").GetString()
                                        };

                                    }
                                    return GetItem();
                                }).ToList();
                        if (x.TryGetProperty("card", out JsonElement cardElement))
                        {
                            if (cardElement.TryGetProperty("first_six_digits", out JsonElement prop))
                            {
                                retVal.Card_FirstSix = prop.GetString();
                            }
                            if (cardElement.TryGetProperty("last_four_digits", out prop))
                            {
                                retVal.Card_LastFour = prop.GetString();
                            }

                        }
                        }
                        return retVal;
                    };
                    return GetItem();
                }
                ).ToList();
            return (retVal);

        }
    }
}
