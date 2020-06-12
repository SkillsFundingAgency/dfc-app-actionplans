using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace DFC.App.Account.Services.DSS.UnitTest.Helpers
{
      public class DssHelpers
    {
       
        
        public static string SuccessfulDSSSessionDetails()
        {
            return
                "[{\r\n  \"SessionId\": \"e3ebf979-4484-4f18-a08f-5f8a4fdde35a\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"InteractionId\": \"2817ea6b-a1d6-4e1a-8eba-46b7d1a427ac\",\r\n  \"DateandTimeOfSession\": \"2020-02-08T12:27:18.0788047Z\",\r\n  \"VenuePostCode\": \"PO15 7AG\",\r\n  \"SessionAttended\": true,\r\n  \"ReasonForNonAttendance\": 2,\r\n  \"LastModifiedDate\": \"2020-06-05T08:35:29.4143499Z\",\r\n  \"LastModifiedTouchpointId\": \"9000000000\",\r\n  \"SubcontractorId\": \"\"\r\n}]";
        }
        
        public static Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn = "{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn)
                })
                .Verifiable();
            return handlerMock;
        }

    }
}
