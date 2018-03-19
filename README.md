# HTML2PDF
Simple C# .NET Core2 AWS Lambda function that converts html to PDF file using wkhtmltox and DinkToPdf.
It accepts piece of plain or base64 encoded HTML and returns base64 encoded bytes array.

## Input
Input event to this function is based on HtmlInputModel class: 
```
{
        public string HtmlInput { get; set; }
        public bool Is64BitEncoded { get; set; }
}
```
#### Input example:
```
{
    "HtmlInput":"<h1>Hello world</h1>",
    "Is64BitEncoded":false
}
```

## Output
It yields a response as an instance of APIGatewayProxyResponse class, in this way it can be used by AWS API Gateway without any modifications.
### Ouptup example:
```
{
  "statusCode": 200,
  "headers": null,
  "body": "JVBERi0xLjQK... ...KNTk2NQolJUVPRgo=",
  "isBase64Encoded": true
}
```

## Notes:
    - Please dont forget to change lambda details in "aws-lambda-tools-defaults.json": "region" and "function-role";

