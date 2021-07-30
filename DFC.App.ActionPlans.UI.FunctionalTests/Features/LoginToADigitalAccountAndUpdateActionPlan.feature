Feature: Login to a digital account and update action plan

Background:
	Given I am on the action plans landing page
	When I click the Go to your action plans button
	And I enter the email address in the Email address field
	And I enter the password in the Password field
	And I click the Sign in button
	Then I am taken to the Your account page
	When I click the View link to view my Action plan
	Then I am taken to the Your action plan page

@ActionPlans @Smoke
Scenario: Login to citizen account and update goal due date
	When I click the View link to view my Goal
	Then I am taken to the View or update goal page
	When I click the Change due date link to update due date
	Then I am taken to the When would you like to achieve this goal? page
	When I enter 01 in the Day field
	And I enter 01 in the Month field
	And I enter 2030 in the Year field
	And I click the Continue button
	Then I am taken to the Due date changed page

@ActionPlans
Scenario: Login to citizen account and update goal status
	When I click the View link to view my Goal
	Then I am taken to the View or update goal page
	When I click the Update status link to update status
	Then I am taken to the Change goal status page
	When I select the radio button option In Progress
	And I click the Update status button
	Then I am taken to the Goal status updated page

@ActionPlans
Scenario: Login to citizen account and update action due date
	When I click the View link to view my Action
	Then I am taken to the View or update action page
	When I click the Change due date link to update due date
	Then I am taken to the When would you like to complete this action by? page
	When I enter 01 in the Day field
	And I enter 01 in the Month field
	And I enter 2030 in the Year field
	And I click the Continue button
	Then I am taken to the Due date changed page

@ActionPlans
Scenario: Login to citizen account and update action status
	When I click the View link to view my Action
	Then I am taken to the View or update action page
	When I click the Update status link to update status
	Then I am taken to the Change action status page
	When I select the radio button option In Progress
	And I click the Update status button
	Then I am taken to the Action status updated page