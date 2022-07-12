using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace test2
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public int Age { get;set; }
        public Guid ContactId { get; set; }
        
        [ForeignKey(nameof(ContactId))]
        public Contact Contact { get;set; }
    }

    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Phone { get;set; }
    }
    
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get;set; }
        public DbSet<Contact> Contacts { get;set; }
    }
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            
            var context = new DatabaseContext(options);

            foreach (var i in Enumerable.Range(0, 100))
            {
                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    Phone = i.ToString()
                };

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Age = i,
                    ContactId = contact.Id 
                };

                context.Contacts.Add(contact);
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();

            var phones = await GetPhonesAsync(context);

            foreach (var phone in phones)
            {
                Console.WriteLine(phone);
            }
        }

        // Надо выбрать 10 телефонов самых старших пользователей, которым больше 30
        private static Task<string[]> GetPhonesAsync(DatabaseContext context)
        {
            throw new NotImplementedException();
        }
    }
}