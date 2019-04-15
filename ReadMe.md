## Project Assessment
Cross Exchange is an arbitrarily trading game developed by a startup in a very short period called "Super Traders". 
The purpose of the application is to educate users on the terminology used in the trading of shares.

### Notes
- The project registers share and allows admins to add the new price of the share on an hourly basis and update last price value at any time.
- The share registered should have unique Symbol to identify it and should be all capital letters with three characters.
- The trade action can be BUY or SELL uppercase.
- The rate of the share should be up to 2 decimal digits.
- The daily, weekly and monthly summary contains four rates Open / High / Low / Close. Open is a first price in the given period; High is the highest price in the given period; Low is the lowest price in the given period and Close is the last price in the given period.
- The frontend application is excluded from the current scope. It is a separate, fully-functional application handled by another team, and we do not want to modify it.
- The frontend team expect API endpoints as agreed earlier and won't accept changes.
- The frontend team accept API response status codes 200, 201, 400, 404. Other response codes are not allowed.
- Product Chief Software Architect requires to stick with current system design and will not accept any changes to VS solution file and project architecture.

### Tasks

1. For a given symbol of share, get the daily, weekly and monthly summary for that particular share calculating the Open / High / Low / Close prices for the given period for that share.\ 
Your goal is to complete the above task for a given three API endpoints. The API specifications are already there in the code (AnalyticsController) as agreed with the frontend team. 

2. There are a few bugs in the application that we would like you to fix. Even though the project might not be in a great structure, please do not spend your valuable time on structural modifications     - focus on fixing the bugs. Please note that there are some issues already reported by the team: 
    - The team noticed that sometimes there are duplicate records for the hourly rate.
    - The team identified shares with unexpected rates which do not match with data from other sources.
    - The team found trade records in the database with an unexpected value in the action column.
    - The team reports that they get 500 response code sometimes from "/api/share/{symbol}/latest" call.


3. Increase unit test coverage to reach code coverage up to 60%, achieving more than 60% will only consume your valuable time without any extra score.

4. Some unit tests failing now, make sure all unit tests are passing.
