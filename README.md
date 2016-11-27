# Iugu examples

Summary

I went over the iugu.net .NET library provided in their website and did a couple of things:
- Fixed the areas where it was broken
- Added methods, models, messages, that were missing/not implemented
- Deleted all the stuff that we will not be using at first so we don't have to get distracted.

Use Cases

Summary

Our primary use case is to charge a restaurant's credit card when they purchase an item from our website. The result of that charge is an invoice (paid in full). 
For that to happen, we need to:
 1. Create a 'Customer' object in iugu. 
 2. Create a 'PaymentMethod' object in Iugu
 3. Associate the PaymentMethod with the Customer
 4. Charge the card.
 5. Invoice gets generated automatically
 6. Receipt email gets generated automatically.

From an Administrative perspective, we have to be able to perform the following tasks

1. Create/Modify/Delete a Customer.
2. Create/Modify/Delete a PaymentMethod (Modify only is allowed for the card description).
3. Associate PaymentMethods with Customers.
4. List all PaymentMethods for one Customer to display on the web portal.
5. Change which PaymentMethod is the primary/default PaymentMethod for a Customer.
6. List all Customer invoices on the web portal

