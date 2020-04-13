using System;

namespace TepayLink.Sdisco.Payment.Dto
{
    
public class Response
{
    public string gatewayCode { get; set; }
    public string message { get; set; }
}

public class Card
{
    public string brand { get; set; }
    public string nameOnCard { get; set; }
    public string issueDate { get; set; }
    public string number { get; set; }
    public string scheme { get; set; }
}

public class TokenResult
{
    public string result { get; set; }
    public Response response { get; set; }
    public string token { get; set; }
    public Card card { get; set; }
    public string deviceId { get; set; }
}

public class Order
{
    public string amount { get; set; }
    public DateTime creationTime { get; set; }
    public string currency { get; set; }
    public string id { get; set; }
}

public class Response2
{
    public string acquirerCode { get; set; }
    public string gatewayCode { get; set; }
    public string message { get; set; }
}

public class Card2
{
    public string brand { get; set; }
    public string nameOnCard { get; set; }
    public string issueDate { get; set; }
    public string number { get; set; }
    public string scheme { get; set; }
}

public class Provided
{
    public Card2 card { get; set; }
}

public class SourceOfFunds
{
    public Provided provided { get; set; }
    public string type { get; set; }
}

public class Acquirer
{
    public string id { get; set; }
    public string transactionId { get; set; }
}

public class Transaction
{
    public Acquirer acquirer { get; set; }
    public string amount { get; set; }
    public string currency { get; set; }
    public string id { get; set; }
    public string type { get; set; }
}

public class PaymentResult
{
    public string apiOperation { get; set; }
    public string merchantId { get; set; }
    public Order order { get; set; }
    public Response2 response { get; set; }
    public string result { get; set; }
    public SourceOfFunds sourceOfFunds { get; set; }
    public Transaction transaction { get; set; }
}

public class PaymentResultDto
{

    public static string CODE_SUCCESS = "SUCCESS";
    public static string CODE_ERROR = "ERROR";
    public static string CODE_PENDING = "PENDING";
    
    public TokenResult tokenResult { get; set; }
    public PaymentResult paymentResult { get; set; }
}
}