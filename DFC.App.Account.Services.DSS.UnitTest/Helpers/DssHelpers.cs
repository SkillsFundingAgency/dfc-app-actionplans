using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace DFC.App.ActionPlans.Services.DSS.UnitTest.Helpers
{
      public class DssHelpers
    {
       
        
        public static string SuccessfulDssSessionDetails()
        {
            return
                "[{\r\n  \"SessionId\": \"e3ebf979-4484-4f18-a08f-5f8a4fdde35a\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"InteractionId\": \"2817ea6b-a1d6-4e1a-8eba-46b7d1a427ac\",\r\n  \"DateandTimeOfSession\": \"2020-02-08T12:27:18.0788047Z\",\r\n  \"VenuePostCode\": \"PO15 7AG\",\r\n  \"SessionAttended\": true,\r\n  \"ReasonForNonAttendance\": 2,\r\n  \"LastModifiedDate\": \"2020-06-05T08:35:29.4143499Z\",\r\n  \"LastModifiedTouchpointId\": \"9000000000\",\r\n  \"SubcontractorId\": \"\"\r\n}]";
        }
        public static string SuccessfulDssGoalsList()
        {
            return
                "[{\r\n    \"GoalId\": \"db02934b-9858-418d-a064-e0449ee095f6\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 1,\r\n    \"GoalStatus\": 2,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:11.2933672Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"99110dec-a240-4a9e-a29e-213159eda4e6\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 2,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:26.7149445Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"209e3b09-a0df-4695-ac86-08360ceecd85\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 3,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:31.4304252Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  },\r\n  {\r\n    \"GoalId\": \"66b6d66c-f024-4255-aab4-633b41523c94\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"SubcontractorId\": \"\",\r\n    \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n    \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n    \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n    \"GoalSummary\": \"this is some text\",\r\n    \"GoalType\": 3,\r\n    \"GoalStatus\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:38:45.6488748Z\",\r\n    \"LastModifiedBy\": \"9000000000\"\r\n  }\r\n]";
        }
        public static string SuccessfulDssActionsList()
        {
            return
                "[{\r\n    \"ActionId\": \"1681a3a1-87dc-4d1d-832f-af4ea1723a24\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"DateActionAgreed\": \"2018-06-21T07:20:00Z\",\r\n    \"DateActionAimsToBeCompletedBy\": \"2019-05-01T08:00:00+00:00\",\r\n    \"DateActionActuallyCompleted\": \"2019-05-01T14:38:00Z\",\r\n    \"ActionSummary\": \"this is some text\",\r\n    \"SignpostedTo\": \"ASIST Team (Apprenticeships)\",\r\n    \"SignpostedToCategory\": null,\r\n    \"ActionType\": 3,\r\n    \"ActionStatus\": 1,\r\n    \"PersonResponsible\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:37:18.8333351Z\",\r\n    \"LastModifiedTouchpointId\": \"9000000000\"\r\n  },\r\n  {\r\n    \"ActionId\": \"fd492490-5a5e-458e-a3ea-69a07b347164\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"DateActionAgreed\": \"2018-06-21T07:20:00Z\",\r\n    \"DateActionAimsToBeCompletedBy\": \"2019-05-01T08:00:00+00:00\",\r\n    \"DateActionActuallyCompleted\": \"2019-05-01T14:38:00Z\",\r\n    \"ActionSummary\": \"this is some text\",\r\n    \"SignpostedTo\": \"ASIST Team (Apprenticeships)\",\r\n    \"SignpostedToCategory\": null,\r\n    \"ActionType\": 2,\r\n    \"ActionStatus\": 2,\r\n    \"PersonResponsible\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:37:40.1257353Z\",\r\n    \"LastModifiedTouchpointId\": \"9000000000\"\r\n  },\r\n  {\r\n    \"ActionId\": \"f497d0de-b0a6-4b22-ae29-1674f7d7f246\",\r\n    \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n    \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n    \"DateActionAgreed\": \"2018-06-21T07:20:00Z\",\r\n    \"DateActionAimsToBeCompletedBy\": \"2019-05-01T08:00:00+00:00\",\r\n    \"DateActionActuallyCompleted\": \"2019-05-01T14:38:00Z\",\r\n    \"ActionSummary\": \"this is some text\",\r\n    \"SignpostedTo\": \"ASIST Team (Apprenticeships)\",\r\n    \"SignpostedToCategory\": null,\r\n    \"ActionType\": 1,\r\n    \"ActionStatus\": 3,\r\n    \"PersonResponsible\": 1,\r\n    \"LastModifiedDate\": \"2020-06-05T08:37:51.7896288Z\",\r\n    \"LastModifiedTouchpointId\": \"9000000000\"\r\n  }\r\n]";
        }
        public static string SuccessfulDssInteractionDetails()
        {
            return "{\r\n  \"InteractionId\": \"2817ea6b-a1d6-4e1a-8eba-46b7d1a427ac\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"TouchpointId\": \"9000000000\",\r\n  \"AdviserDetailsId\": \"bb940afb-1423-4999-a234-5a64a5c00831\",\r\n  \"DateandTimeOfInteraction\": \"2019-10-01T16:52:10Z\",\r\n  \"Channel\": 1,\r\n  \"InteractionType\": 2,\r\n  \"LastModifiedDate\": \"2019-06-22T16:52:10\",\r\n  \"LastModifiedTouchpointId\": \"9000000000\"\r\n}";
        }
        
        public static string SuccessfulDssAdviserDetails()
        {
            return "{\r\n  \"AdviserDetailId\": \"bb940afb-1423-4999-a234-5a64a5c00831\",\r\n  \"AdviserName\": \"this is some text\",\r\n  \"AdviserEmailAddress\": \"adviser2@test.com\",\r\n  \"AdviserContactNumber\": \"012345 678901\",\r\n  \"LastModifiedDate\": \"2019-02-22T16:02:47.8843571Z\",\r\n  \"LastModifiedTouchpointId\": \"0000000104\",\r\n  \"SubcontractorId\": \"21323234\"\r\n}";
        }

        public static string SuccessfulDssGoalDetails()
        {
            return "{\r\n  \"GoalId\": \"db02934b-9858-418d-a064-e0449ee095f6\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n  \"DateGoalCaptured\": \"2018-06-21T11:31:00Z\",\r\n  \"DateGoalShouldBeCompletedBy\": \"2018-06-23T12:01:00Z\",\r\n  \"DateGoalAchieved\": \"2018-06-22T19:53:00Z\",\r\n  \"GoalSummary\": \"this is some text\",\r\n  \"GoalType\": 1,\r\n  \"GoalStatus\": 2,\r\n  \"LastModifiedDate\": \"2020-06-05T08:38:11.2933672Z\",\r\n  \"LastModifiedBy\": \"9000000000\"\r\n}";
        }

        public static string SuccessfulUpdateActionPlan()
        {
            return
                "{\r\n  \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"InteractionId\": \"2817ea6b-a1d6-4e1a-8eba-46b7d1a427ac\",\r\n  \"SessionId\": \"e3ebf979-4484-4f18-a08f-5f8a4fdde35a\",\r\n  \"SubcontractorId\": \"\",\r\n  \"DateActionPlanCreated\": \"2020-05-10T08:30:00+00:00\",\r\n  \"CustomerCharterShownToCustomer\": true,\r\n  \"DateAndTimeCharterShown\": \"2020-05-10T08:30:00+00:00\",\r\n  \"DateActionPlanSentToCustomer\": \"2020-05-10T08:30:00+00:00\",\r\n  \"ActionPlanDeliveryMethod\": 2,\r\n  \"DateActionPlanAcknowledged\": \"2020-06-15T07:55:00\",\r\n  \"CurrentSituation\": \"this is some text 1st ActionPlan\",\r\n  \"LastModifiedDate\": \"2020-06-17T09:50:05.4998502Z\",\r\n  \"LastModifiedTouchpointId\": \"0000000997\"\r\n}";
        }
        public static string SuccessfulDssActionDetails()
        {
            return
                "{\r\n  \"ActionId\": \"1681a3a1-87dc-4d1d-832f-af4ea1723a24\",\r\n  \"CustomerId\": \"53f904b3-77c8-4c94-9a15-c259b518336c\",\r\n  \"ActionPlanId\": \"a6676a45-7cd8-4257-96cb-bc9388f9c149\",\r\n  \"DateActionAgreed\": \"2018-06-21T07:20:00Z\",\r\n  \"DateActionAimsToBeCompletedBy\": \"2019-05-01T08:00:00+00:00\",\r\n  \"DateActionActuallyCompleted\": \"2019-05-01T14:38:00Z\",\r\n  \"ActionSummary\": \"this is some text\",\r\n  \"SignpostedTo\": \"ASIST Team (Apprenticeships)\",\r\n  \"ActionType\": 3,\r\n  \"ActionStatus\": 1,\r\n  \"PersonResponsible\": 1,\r\n  \"LastModifiedDate\": \"2020-06-05T08:37:18.8333351Z\",\r\n  \"LastModifiedTouchpointId\": \"9000000000\"\r\n}";
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
