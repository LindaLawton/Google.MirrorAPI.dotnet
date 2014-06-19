Google Mirror API .Net
=================================

Sample project for working with the Google Analytics Real-time API to write to Mirror API in C# .net

This is a simple console application:

GoogleAnaltyicsGlass <profileid> <min number of users before sent to glass>


Tutorial 
=================================
This project goes along with the following tutorial

http://www.daimto.com/mirror-api-with-c/


SETUP
=================================
This project will run as is using my client_id and client secret I have left them in there 
becouse the Google Analytics Real-time API is curently beta.  When you create your own
project you will need to apply for beta but it will take 24 hours for you to get access. 
This is a test project if to many people start using my client_id and Secret and the quota blows
out i will not be requesting more access.  :)

Get your own access 

You will need to create your own credentials in the Google Apis console for it to work:
Go to :  https://console.developers.google.com

Create a new Application:  

APIs & Auth -> APIs Enable Google Analytics API  and Mirror API

APIs & Auth -> Credentials -> Create client id for Native application
               it is here you will find the Client id and client secret you will need in order to run the project.
               
APIs & Auth -> Consent screen
                 Be sure to supply an Email Address, and product name. 


Apply for the Real-Time API beta  https://docs.google.com/forms/d/1qfRFysCikpgCMGqgF3yXdUyQW4xAlLyjKuOoOEFN2Uw/viewform

Note:  This Project only Selects from the management API, Real-time API, and posts to the Mirror API.  

Running it
=================================
I recomend adding it as a scheuled task


Links
=================================

Tutorial http://www.daimto.com/mirror-api-with-c/