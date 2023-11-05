using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    
    public class Client
    {
        [Key]
        [Display(Name = "Client ID")]
        [Required]
        public int ClientID { get; set; }

        public string LastName {  get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<ClientAccount>
            ClientAccounts
        { get; set; }
    }

    //client
    //clientID - key
    //lastName
    //firstName
    //email

    public class BankAccount
    {
        [Key]
        [Display(Name = "Account Number")]
        [Required]
        public int AccountNum { get; set; }

        public string AccountType { get; set; }
        public string Balance { get; set; }

        public virtual ICollection<ClientAccount>
            ClientAccounts
        { get; set; }
    }

    //bankaccount
    //accountNum
    //accountType
    //balance

    public class ClientAccount
    {
        [Key, Column(Order = 0)]
        public int ClientID { get; set; }

        [Key, Column(Order = 1)]
        public int AccountNum { get; set; }

        //parents
        public virtual Client Client { get; set; }
        public virtual BankAccount BankAccount { get; set; }

    }

    //clientaccount
    //clientID - foreign key from client
    //accountNum - foreign key from bankaccount
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //define composite primary key
            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.ClientID, ca.AccountNum });

            //define foreign keys
            modelBuilder.Entity<ClientAccount>()
                .HasOne(c => c.Client)
                .WithMany(c => c.ClientAccounts)
                .HasForeignKey(fk => new {fk.ClientID})
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientAccount>()
                .HasOne(b => b.BankAccount)
                .WithMany(b => b.ClientAccounts)
                .HasForeignKey(fk => new {fk.AccountNum})
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}