using System;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DinkToPdf;
using HTML2PDF.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HTML2PDF
{
    public class Function
    {

        public APIGatewayProxyResponse FunctionHandler(HtmlInputModel htmlInput, ILambdaContext context)
        {
            try
            {
                var converter = new BasicConverter(new PdfTools());

                var inputAsString = htmlInput.HtmlInput;
                if (htmlInput.Is64BitEncoded)
                {
                    inputAsString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(htmlInput.HtmlInput));
                }
                var pdfDocument = new HtmlToPdfDocument
                {
                    GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                    },
                    Objects = {
                        new ObjectSettings() {
                            PagesCount = true,
                            HtmlContent = inputAsString,
                            WebSettings = { DefaultEncoding = "utf-8", LoadImages = true, Background = true},
                            FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                        }
                    }
                };

                var pdfBuf = converter.Convert(pdfDocument);

                var converted = Convert.ToBase64String(pdfBuf);

                var response = new APIGatewayProxyResponse
                {
                    Body = converted,
                    IsBase64Encoded = true,
                    StatusCode = 200
                };

                return response;
            }
            catch (Exception e)
            {
                context.Logger.Log("Error in PDFGenerator:");
                context.Logger.Log(e.Message);

                APIGatewayProxyResponse response = new APIGatewayProxyResponse
                {
                    StatusCode = 500
                };

                return response;
            }
        }
    }
}
