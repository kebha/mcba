﻿namespace s3844648_a2.Models;

public class Account
{

    public int AccountNumber { get; set; }


    public string AccountType { get; set; }


    public int CustomerID { get; set; }


    public decimal Balance { get; set; }


    public List<Transaction> Transactions { get; set; }
}
