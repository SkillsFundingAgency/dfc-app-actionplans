using System.Net;
using System.Net.Http;
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
        public static string SuccessfulDSSGoalDetails()
        {
            return
                "[{\r\n    \"GoalId\": \"db02934b-9858-418d-a064-e0449ee095f6\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 1,\r\n    \"GoalStatus\": 2,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:11.2933672Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"99110dec-a240-4a9e-a29e-213159eda4e6\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 2,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:26.7149445Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"209e3b09-a0df-4695-ac86-08360ceecd85\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 3,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:31.4304252Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"66b6d66c-f024-4255-aab4-633b41523c94\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 3,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:45.6488748Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  }\r\n]";
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
