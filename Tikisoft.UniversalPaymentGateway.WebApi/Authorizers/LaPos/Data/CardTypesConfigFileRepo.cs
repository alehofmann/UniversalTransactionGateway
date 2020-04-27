using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Comms.Model;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data
{
    public class CardTypesConfigFileRepo : ICardTypesRepo
    {
        private IConfiguration _config;
        public CardTypesConfigFileRepo(IConfiguration config)
        {
            _config = config;
        }
        public IList<CardType> GetAcceptedCards()
        {
            var section = _config.GetSection("TarjetasAceptadas");
            var res = new List<CardType>();
                
            if (section is null)
            {
                throw (new MissingFieldException("No existe la sección 'TarjetasAceptadas' en el archivo de configuración"));
            }

            var cardTypes = section.GetChildren().Select(
                cardType => new CardType 
                { 
                    CardCode=cardType["CardCode"], 
                    Name = cardType["CardName"], 
                    ProcessorCode=cardType["ProcessorCode"],
                    CommerceNumber= cardType["CommerceNumber"]
                }).ToList();

            //Solo devuelve las que tienen configurado el CommerceNumber
            return cardTypes.Where(i => !string.IsNullOrWhiteSpace(i.CommerceNumber)).ToArray();

        }

        public void UpdateAcceptedCards(IList<CardType> cardsList)
        {
            var section = _config.GetSection("TarjetasAceptadas");

            if (section is null)
            {
                
            }
        }
    }
}
