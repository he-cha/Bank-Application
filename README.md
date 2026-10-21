# Banking Console Simulation

**C# · .NET · Object-oriented programming · CSV file input**

An educational console application with customer and administrator menus, account lookup, account creation, deposits, withdrawals, transfers and balance inquiries. `AccountInfo` and `AccountType` represent account data and checking/savings categories.

## Run

The project currently targets .NET 7. With a compatible SDK installed, run from the data directory so the relative CSV/admin paths resolve:

```bash
cd BankApplication
dotnet run
```

The code expects `account_data.csv` and `admins.txt` in the working directory. Use fictional demo data only. Startup reads eight comma-separated fields per account: account number, PIN, first name, last name, balance, deposit count, withdrawal count and account type (`Savings` or `Checking`). Administrator input is a username and numeric password separated by a comma.

## Scope and limitations

This is a programming exercise, not financial software. Credentials are stored in plaintext, monetary values use `double`, and workflows need further validation. The current account-creation flow does not add the returned account to the shared list or assign the advertised opening balance. Transactions are primarily in-memory; persistence and account lifecycle behavior need dedicated tests before any stronger completeness claim.

Compiled `bin/` and `obj/` outputs have been removed from the current source tree so reviewers see the source. Runtime code was not changed or compiled during this cleanup; no .NET SDK was available. Keep this as supporting evidence of C# fundamentals rather than a flagship analytics project.
