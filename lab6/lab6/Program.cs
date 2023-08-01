using System;

namespace lab6
{
    abstract class BankAccounts
    {
        protected Guid ID;
        protected string accountName;
        private readonly string fullName;
        private string IBAN;
        protected double sold;

        public BankAccounts(string accountName, string fullName, string IBAN)
        {
            ID = Guid.NewGuid();
            this.accountName = accountName;
            this.fullName = fullName;
            this.IBAN = IBAN;
            sold = 0;
        }

        public string GetDescripiton()
        {
            return $"ID: {ID}\nFullName: {fullName}\nName: {accountName}\nSold: {sold} RON\n";
        }

        public void AddFounds(double suma)
        {
            sold += suma;
            Console.WriteLine($"Ai depus {suma} RON in contul {accountName}. Soldul nou este {sold} RON");
        }

        public abstract void ExtractFounds(double suma);

        class CurrentAccount: BankAccounts
        {
            private float plafonDescoperire;

            public CurrentAccount(string accountName, string fullName, string IBAN) : base(accountName, fullName, IBAN)
            {
                plafonDescoperire = 5000;
            }

            public override void ExtractFounds(double suma)
            {
                if (suma <= sold)
                {
                    sold -= suma;
                    Console.WriteLine($"Ai extras {suma} RON. Au mai ramas {sold} RON");
                }
                else if (suma <= sold + plafonDescoperire)
                {
                    double neagativeAmount = suma - sold;
                    sold = 0;
                    Console.WriteLine($"Ai extras {suma} RON. Au mai ramas 0 RON");
                    Console.WriteLine($"Ai folsit descoperire in valoare de {neagativeAmount} RON");
                }
                else
                {
                    Console.WriteLine($"Ai incercat sa extragi suma de {suma} RON. Nu ai suficienti bani pentru a efectua operatiunea");
                }
            }
        }

        class EconomyAccount: BankAccounts
        {
            private double interestRate;

            public EconomyAccount(string accountName, string fullName, string IBAN, double interestRate) :base(accountName, fullName, IBAN)
            {
                this.interestRate = interestRate;
            }

            public override void ExtractFounds(double suma)
            {
                if(suma <= sold)
                {
                    sold -= suma;
                    Console.WriteLine($"Ai extras suma de {suma} RON. Au mai ramas in contul {accountName} {sold} RON");
                }
                else
                {
                    Console.WriteLine($"Nu ai suficienti bani in contul {accountName}");
                }
            }

            public void UpdateInterestRate()
            {
                sold *= (1 + interestRate);
                Console.WriteLine($"S-a actualizat rata de dobanda pentru {accountName}, soldul este acum {sold} RON");
            }
        }

        class InvestmentAccount: BankAccounts
        {
            private int valability;

            public InvestmentAccount(string accountName, string fullName, string IBAN, int valability) : base(accountName, fullName, IBAN)
            {
                this.valability = valability;
            }

            public override void ExtractFounds(double suma)
            {
                if(suma <= sold && DateTime.Now.Day == valability)
                {
                    sold -= suma;
                    Console.WriteLine($"Suma de {suma} RON a fost retrasa cu succes. Au mai ramas {sold} RON");
                }
                else if(suma <= sold && DateTime.Now.Day != valability)
                {
                    Console.WriteLine($"Nu s-a putut realiza extragerea de {suma} deoarece termenul de extragere nu a sosit. Au mai ramas {Math.Abs(valability - DateTime.Now.Day)} zile");
                }
                else if(suma >= sold && DateTime.Now.Day == valability)
                {
                    Console.WriteLine("Nu s-a putut efectua operatiunea, fonduri insuficiente");
                }
                else
                {
                    Console.WriteLine($"Operatiunea nu se poate face, fonduri insuficiente si au mai ramas {Math.Abs(valability - DateTime.Now.Day)} zile pana cand operatiunea se va putea realiza cu succes");
                }
            }
        }

        class Program
        {
            static void Main()
            {
                CurrentAccount currentAccount = new CurrentAccount("Cont current 1", "Campean Leon", "RON1288302033");
                Console.WriteLine(currentAccount.GetDescripiton());
                currentAccount.AddFounds(5000);
                currentAccount.ExtractFounds(2000);
                currentAccount.ExtractFounds(9000);
                currentAccount.AddFounds(4000);
                currentAccount.ExtractFounds(9000);
                Console.WriteLine();

                EconomyAccount economyAccount = new EconomyAccount("Cont economii 1", "Campean Leon", "RON1212123233", 0.05);
                Console.WriteLine(economyAccount.GetDescripiton());
                economyAccount.AddFounds(5000);
                economyAccount.UpdateInterestRate();
                economyAccount.ExtractFounds(9000);
                Console.WriteLine();

                InvestmentAccount investmentAccount = new InvestmentAccount("Cont de investitii 1", "Campean Leon", "RON12121213", 17);
                Console.WriteLine(investmentAccount.GetDescripiton());
                investmentAccount.AddFounds(10000);
                investmentAccount.ExtractFounds(5000);
                investmentAccount.ExtractFounds(13000);
                Console.WriteLine();

                Console.WriteLine(currentAccount.GetDescripiton());
                Console.WriteLine(economyAccount.GetDescripiton());
                Console.WriteLine(investmentAccount.GetDescripiton());


                Console.ReadKey();
            }
        }
    }
}


