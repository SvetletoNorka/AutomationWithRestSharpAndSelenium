
Feature: OnlineShoping

Version 1

Scenario 1
Use the standard user and password (they are prone to change, think how to obtain them)
Log in with the standard user -> https://www.saucedemo.com/
Add the first and the last item in the cart, verify the correct items are added
Remove the first item and add previous to the last item to the cart, verify the content again
Go to checkout
Finish the order
Verify order is placed
Verify cart is empty
Logout from the system

Scenario 2
Log in with the standard user
Verify when for sorting it is selected "Price (high to low)"
Then the items are sorted in the correct manner
Logout from the system

Version 2
Implement the tasks written in Version 1 and do the following as well
Add an ability to filter tests for the test execution
Add custom HTML report for the test execution
Tests will be executed on multiple environments (dev, testing, staging, etc..), add necessary configurations.
Chrome and Firefox should be supported browsers

Scenario: Scenario 1
	Given I log in with the standard user
	When I add "first" item in the cart
		And I add "last" item in the cart
	Then I open the cart
		And I verify "first" item is in the cart
		And I verify "last" item is in the cart
	When I remove "first" item from cart
	Then I continue with shoping
	When I add "previous of last" item in the cart
	Then I open the cart
		And I verify "previous of last" item is in the cart
		And I verify "last" item is in the cart
	When I go to checkout
	Then I fill first name = "Svetlana"
		And I fill last name = "Norka"
		And I fill zip code = "1000"
	When I press continue
	Then I finish the order
		And I verify the checkout is completed
	When I press Back Home
	Then I verify the cart is empty
	When I press burger menu
	Then I logout from the system

Scenario: Scenario 2
	Given I log in with the standard user