﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using s3844648_a2.Converters;

namespace s3844648_a2.Models;

public enum AccountType
{
    Savings = 1,
    Checking = 2
}

public class Account
{
    [JsonProperty("AccountNumber")]
    [Display(Name = "Account Number")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AccountID { get; set; }

    [JsonConverter(typeof(AccountTypeStringToAccountTypeEnumConverter))]
    [Display(Name = "Type")]
    public AccountType AccountType { get; set; }

    public int CustomerID { get; set; }
    public virtual Customer Customer { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    [InverseProperty(nameof(Transaction.Account))]
    public virtual List<Transaction> Transactions { get; set; }

    [InverseProperty(nameof(BillPay.Account))]
    public virtual List<BillPay> BillPays { get; set; }
}
