using System;
using System.Linq;
using System.IO;
using Microsoft.VisualBasic;
public enum AccountType
{
    Savings, Checking
}
class AccountInfo
{
    public long accountNum { get; set; }
    public int PIN { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public double balance { get; set; }
    public double numDeposits { get; set; }
    public double numWithdrawals { get; set; }
    public AccountType accountType { get; set; }
}

class Program
{
    static string accountDataPath = "account_data.csv";
    static string adminFilePath = "admins.txt";
    static List<AccountInfo>? accounts;
    static Dictionary<string, string> admin = new Dictionary<string, string>();
    static void Main(string[] args)
    {
        accounts = ReadData().ToList();
        admin = ReadAdminFile();
        
        WelcomePage(accounts, null, admin);
        
    }

    //Read data from the account file
    static IEnumerable<AccountInfo> ReadData()
    {
        try
        {
            var lines = File.ReadAllLines(accountDataPath);
            return lines.Select(line =>
            {
                var split = line.Split(',');
                return new AccountInfo
                {
                    accountNum = long.Parse(split[0]),
                    PIN = int.Parse(split[1]),
                    firstName = split[2],
                    lastName = split[3],
                    balance = double.Parse(split[4]),
                    numDeposits = double.Parse(split[5]),
                    numWithdrawals = double.Parse(split[6]),
                    accountType = (AccountType)Enum.Parse(typeof(AccountType), split[7])
                };
            });
        }
        catch
        {
            Console.WriteLine("Error while reading file.");
            return Enumerable.Empty<AccountInfo>();
        }
    }
    /********Welcome Page*************/
    static void WelcomePage(List<AccountInfo> accounts, AccountInfo? loggedIn, Dictionary<string,string> admin)
    {
        Console.WriteLine("Welcome to your Online Banking Application!");
        Console.WriteLine("1. Account Login");
        Console.WriteLine("2. Create Account");
        Console.WriteLine("3. Administrator Login");
        Console.WriteLine("4. Quit");
        Console.WriteLine("Select Option: ");
        int choice;

        if(int.TryParse(Console.ReadLine(), out choice))
        {
            switch(choice)
            {
                case 1:
                    AccountLogin(accounts, null);
                    break;
                case 2:
                    CreateAccount(accounts,AccountType.Savings);
                    break;
                case 3:
                    AdminLogin(admin,accounts);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        Console.WriteLine("Failed to load.");
        
    }

    static AccountInfo CreateAccount(List<AccountInfo> accounts,AccountType accountType)
    {
        Console.WriteLine("-------------------");
        Console.WriteLine("CREATE ACCOUNT");
        Console.WriteLine("-------------------");

        
        string enteredFirst, enteredLast, enteredPin;
        int pin;
        AccountType selectedType;
        
       
        while(true)
        {
                Console.WriteLine("Enter your first name:");;
                enteredFirst = Console.ReadLine()!;
                if(enteredFirst != null)
                {
                    break;
                }
                Console.WriteLine("Error: You must enter a first name");
        }
        while(true)
        {
                Console.WriteLine("Enter your Last name:");
                enteredLast = Console.ReadLine()!;
                if(enteredLast != null)
                {
                    break;
                }
                Console.WriteLine("Error: You must enter a Last name");
        }
        while(true)
        {
                Console.WriteLine("Select a PIN: Must be 4 digits");;
                enteredPin = Console.ReadLine()!;
                if(enteredPin != null && enteredPin.Length == 4 && int.TryParse(enteredPin, out pin))
                {
                    break;
                }
                Console.WriteLine("Error: You must enter a four digit pin");
        }
        Random random = new Random();
        long randomSuffix = random.Next(999999999, int.MaxValue);

        long accNum = long.Parse($"183977{randomSuffix}");
        while(true)
        {
            Console.WriteLine("What type of Account do you want to open?");
            Console.WriteLine("1. savings \n 2. checking");
            Console.WriteLine("SelectOption");

            if(int.TryParse(Console.ReadLine(), out int choice))
            {
                switch(choice)
                {
                    case 1:
                        selectedType = AccountType.Savings;
                        break;
                    case 2:
                        selectedType = AccountType.Checking;
                        break;
                    default:
                        Console.WriteLine("ERROR: Please enter a valid number.");
                        continue;
                }

                AccountInfo newAccount = new AccountInfo 
                {
                    firstName = enteredFirst, lastName = enteredLast, PIN = pin, accountNum = accNum, accountType = selectedType
                };
                
                Console.WriteLine($"Congratulations {enteredFirst} {enteredLast}! Your accounts is open with an initial deposit of $100.");
                Console.WriteLine($"Your account number is: {accNum}. You can access account services now.");
                WelcomePage(accounts,newAccount, admin);
                
                return newAccount;
            }
            else
            {
                Console.WriteLine(" ");
            }
            
        }
    }

    //method for the Account Logn
    //Ask for an account number and PIN
    //Validate inputs
    static void AccountLogin(List<AccountInfo> accounts, AccountInfo? loggedInAccount)
    {
        Console.WriteLine("------------------");
        Console.WriteLine("ACCOUNT LOGIN");
        Console.WriteLine("------------------");
        long accountNum;

        while(true)
        {
                Console.WriteLine("Enter your account number: ");

                if(long.TryParse(Console.ReadLine(), out  accountNum))
                {
                    break;
                }

                Console.WriteLine("Error: You must enter a valid account number.");
        }
        while (true)
            {
                Console.WriteLine("Enter your PIN: ");
                if (int.TryParse(Console.ReadLine(), out int pin))
                {
                    var logIn = ValidateAccount(accountNum, pin);
                    if (logIn != null)
                    {
                        Console.WriteLine($"Welcome back {logIn.firstName} {logIn.lastName}. How can we help you today?");
                        Menu(logIn, accounts);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: You must Enter a PIN.");
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: You must Enter a PIN. ");
                }
            }
            Console.WriteLine(" ");
        }

    //check if the account exists
     static AccountInfo? ValidateAccount(long accNum, int EnteredPin)
        {
            return accounts?.FirstOrDefault(account => account.accountNum == accNum && account.PIN == EnteredPin);
        }
    
    //Main Menu
    //Include first and last name of the user with a welcoming statement
    //Ask the user what they would like to do with the menu options
    static void Menu(AccountInfo loggedInAccount, List<AccountInfo> accountsData)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1. Make a Withdrawal");
        Console.WriteLine("2. Make a Deposit");
        Console.WriteLine("3. Transfer Funds to another user Account");
        Console.WriteLine("4. Balance Inquiry");
        Console.WriteLine("5. Back to Main Menu");

        int choice;
        Console.WriteLine("Select Option: ");
        if(int.TryParse(Console.ReadLine(), out choice))
                {
                    switch(choice)
                    {
                        case 1:
                            Withdrawal(loggedInAccount, accountsData);
                            break;
                        
                        case 2:
                            Deposit(loggedInAccount, accountsData);
                            break;
                        
                        case 3:
                            Transfer(loggedInAccount, accountsData);
                            break;
                        
                        case 4:
                            BalanceInquiry(loggedInAccount, accountsData);
                            break;
                        case 5:
                            WelcomePage(accountsData, loggedInAccount, admin);
                            break;
                        
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Error: You must choose from the options.");

                    Console.ReadLine();
                }

    }

    static void Withdrawal(AccountInfo account, List<AccountInfo> accountsData)
    {
        Console.WriteLine("How much would you like to withdraw?");

        if(double.TryParse(Console.ReadLine(), out double amount))
        {
            if(amount > 0)
            {
                if(account.balance >= amount)
                {
                    account.balance = account.balance - amount ;
                    account.numWithdrawals++;

                    Console.WriteLine($"Withdrawal Successful. New balance: {account.balance}");
                }
                else
                {
                    Console.WriteLine("Error:You have Insufficient funds. ");
                }
            }
            else
            {
                Console.WriteLine("Error: Please Enter a valid number.");
            }
        
        }
        else
        {
            Console.WriteLine("Error: Invalid input.");
        }

        Menu(account, accountsData);
    
    }
    static void Deposit(AccountInfo account, List<AccountInfo> accountsData)
    {
        Console.WriteLine("How much would you like to Deposit?");

        if(double.TryParse(Console.ReadLine(), out double amount))
        {
            if(amount > 0)
            {
                
                    account.balance = amount + account.balance;
                    account.numDeposits++;

                    Console.WriteLine($"Deposit Successful. New balance: {account.balance}");
            }
            else
            {
                Console.WriteLine("Error: Please Enter a valid number.");
            }
        
        }
        else
        {
            Console.WriteLine("Error: Invalid input.");
        }
        Menu(account, accountsData);
        
    }
    //Use the receiver's accountnum 
    static void Transfer(AccountInfo loggedInAccount, List<AccountInfo> accountsData)
    {
        
       Console.WriteLine("How much would you like to Transfer?");
        if(double.TryParse(Console.ReadLine(), out double transferAmount))
        {
            Console.WriteLine("Enter Debit Card Number of the person to transfer to: ");
            if(long.TryParse(Console.ReadLine(), out long receiverAccountNum))
            {
                AccountInfo? recieverAccount = accountsData.FirstOrDefault(account => account.accountNum == receiverAccountNum);
                if (recieverAccount != null)
                {
                    if(transferAmount > 0)
                    {
                        if(loggedInAccount.balance >= transferAmount)
                        {
                            loggedInAccount.balance -= transferAmount;
                            recieverAccount.balance += transferAmount;

                            loggedInAccount.numWithdrawals += 1;
                            recieverAccount.numDeposits += 1;

                            Console.WriteLine($"Transfer Successful: Your new balance is ${loggedInAccount.balance}");
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Insufficent funds.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Please Enter a valid amount.");
                    }
                }
            }
            else
            {
            Console.WriteLine("Transfer Failed: Account Number is incorrect.");
            }
        }
        Menu(loggedInAccount, accountsData);
        
    }
    static void BalanceInquiry(AccountInfo account, List<AccountInfo> accountsData)
    {
        if(account != null)
        {
        Console.WriteLine($"Current Balance: ${account.balance}");
        Menu(account, accountsData);
        }
        else
        {
            Console.WriteLine("Couldn't fetch account details.");
        }
        
    }


    /********************************/
    /*******Admin Login*************/

    //Read admins.txt
    static Dictionary<string,string> ReadAdminFile()
    {
        Dictionary<string, string> adminData = new Dictionary<string,string>();

        try
        {
            if(File.Exists(adminFilePath))
            {
                var lines = File.ReadAllLines(adminFilePath);
            
                foreach(string items in lines)
                {
                    string[] por = items.Split(',');

                    if(int.TryParse(por[1], out int password))
                    {
                    admin[por[0]] = password.ToString();
                    }
                    else
                    {
                        Console.WriteLine("Error reading admin File.");
                    }
                }
            }
        }
        catch
        {
            Console.WriteLine("Error reading admin File.");
        }

        return admin;
    }
    static void AdminLogin(Dictionary<string, string> admin, List<AccountInfo> accounts)
    {
        Console.WriteLine("------------------");
        Console.WriteLine("ADMIN LOGIN");
        Console.WriteLine("------------------");

        while(true)
        {
                Console.WriteLine("Enter admin name: ");
                string name = Console.ReadLine()!;
                Console.WriteLine("Enter password");
                string inputPassword = Console.ReadLine()!;
                
                    if(admin.ContainsKey(name))
                    {
                        string filePassword = admin[name];

                        if(inputPassword == filePassword)
                        {
                            AdminReport(accounts);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid username/password combination");
                        }
                    }
                
                else
                {
                    Console.WriteLine("Invalid username/password combination");
                    
                }
        }
    }
    static void AdminReport(List<AccountInfo> accounts)
    {
        Console.WriteLine("Select a report to review");
        Console.WriteLine("1. Show average Savings Account balance");
        Console.WriteLine("2. Show total Savings Account balance");
        Console.WriteLine("3. Show average Checking Account balance");
        Console.WriteLine("4. Show total Checking Account balance");
        Console.WriteLine("5. Show the number of accounts for each account type");
        Console.WriteLine("6. Show the 10 accounts with the most deposits");
        Console.WriteLine("7. Show the 10 accounts  with the most withdrawals");
            Console.WriteLine("8. Back to Main Menu");    

        int choice;
        Console.WriteLine("Select Option: ");
        if(int.TryParse(Console.ReadLine(), out choice))
        {
            switch(choice)
            {
                case 1:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Average Savings Account Balance");
                      Console.WriteLine("--------------------");

                      List<AccountInfo> savings = accounts.Where(account => account.accountType == AccountType.Savings).ToList();

                      double avgSavingsBalance = savings.Average(account => account.balance);
                      Console.WriteLine($"Average Savings Account Balance: ${avgSavingsBalance:F2}");    
                    break;
                        
                case 2:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Total Savings Account Balance");
                      Console.WriteLine("--------------------");

                      List<AccountInfo> savingsAcc = accounts.Where(account => account.accountType == AccountType.Savings).ToList();

                      double totalSavingsBalance = savingsAcc.Sum(account => account.balance);
                      Console.WriteLine($"Total Savings Account Balance: ${totalSavingsBalance:F2}");
                            
                    break;
                        
                case 3:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Average Checking Account Balance");
                      Console.WriteLine("--------------------");

                      List<AccountInfo> checkings = accounts.Where(account => account.accountType == AccountType.Checking).ToList();

                      double avgCheckingBalance = checkings.Average(account => account.balance);
                      Console.WriteLine($"Average Checking Account Balance: ${avgCheckingBalance:F2}");  
                    break;
                        
                case 4:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Total Cheking Account Balance");
                      Console.WriteLine("--------------------");

                      List<AccountInfo> checkingsAcc = accounts.Where(account => account.accountType == AccountType.Checking).ToList();

                      double totalCheckingBalance = checkingsAcc.Sum(account => account.balance);
                      Console.WriteLine($"Total Checking Account Balance: ${totalCheckingBalance:F2}");
                            
                    break;
                case 5:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Total Number of Accounts");
                      Console.WriteLine("--------------------");

                      int savingsNumAccounts = accounts.Count(account => account.accountType == AccountType.Savings);
                      int checkingNumAccounts = accounts.Count(account => account.accountType == AccountType.Checking);

                      Console.WriteLine($"Saving Accounts: ${savingsNumAccounts}"); 
                      Console.WriteLine($"Checking Accounts: ${checkingNumAccounts}");  

                    break; 
                case 6:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Total 10 Deposit Accounts");
                      Console.WriteLine("--------------------");
                      
                      var tenDepositAccounts = accounts.OrderByDescending(account => account.numDeposits). Take(10);

                      foreach (var account in tenDepositAccounts)
                      {
                        Console.WriteLine($"{account.firstName} {account.lastName}: {account.numDeposits} Deposits");
                      }

                    break;

                case 7:
                      Console.WriteLine("--------------------");
                      Console.WriteLine("Total 10 Withdrawal Accounts");
                      Console.WriteLine("--------------------");

                      
                      var tenWithdrawalAccounts = accounts.OrderByDescending(account => account.numWithdrawals). Take(10);

                      foreach (var account in tenWithdrawalAccounts)
                      {
                        Console.WriteLine($"{account.firstName} {account.lastName}: {account.numWithdrawals} Withdrawals");
                      }
                    break;

                case 8:
                    WelcomePage(accounts,null, admin);
                    break;
                default:
                    break;       
                    
            }
        }
        else
        {
            Console.WriteLine("Error: You must choose from the options.");

            Console.ReadLine();

        }
        AdminReport(accounts);

    }
}
    


