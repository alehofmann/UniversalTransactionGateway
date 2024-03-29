﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public class ConfigItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ConfigItemId")]
        [JsonIgnore()]
        public int ConfigItemId { get; set; }

        [JsonProperty("terminal_id")]
        [Required]
        public string TerminalId { get; set; }

        [JsonProperty("processor")]
        public string Processor { get; set; }

        [JsonProperty("key")]
        [Required]
        public string Key { get; set; }

        [JsonProperty("value")]
        [Required]
        public string Value { get; set; }

    }
}
