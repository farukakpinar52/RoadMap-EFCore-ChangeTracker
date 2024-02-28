// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");
MyContext context = new();


#region AsNoTracking Metodu
//Context üzerinden gelen tüm datalar changetracker ile takip edilir.bunu yaparken ortaya bir maliyet çıkar.o yüzden işlem yapılmayacak veriler sadece okunacak/listenecek olan verileri takip etmeye gerek duymamalıyız.
//bu yüzden AsNoTracking metodu changetracker'ın takibini keser
//AsNoTracking ile veriler elde edilip projeksiyon yapılabilir, elde edilen veriler kullanılabilir ama veriler üzerinde update-dekele işlemleri yapılamaz

//var kullanıcılar = await context.Users.AsNoTracking().ToListAsync();
//foreach (var kullanıcı in kullanıcılar)
//{
//    Console.WriteLine(kullanıcı.Name);
//    Eğer burada context.Users.Update yada context.Users.Delete işlemi yaparsak bu tabloya yansır çünkü bu manuel olarak yapılan methodlardır changetracker olmadan da bu işlem veritabanına yansır
//}


#endregion

#region AsNoTrackingWithIdentityResolution
// AsNoTracking ZARARI : AsNoTracking bağlantıları kestiği için ilişkili tablolarda her nesne için ilişkili olduğu yeni bir nesne üretir.
//örnek : 5 kullanıcı varsa 5 rol nesnesi üretir. Ama ChangeTracker mekanizması ise her kullanıcı için bir rol nesnesi oluşturmayabilir eğer bir rol için birden çok kullanıcı varsa o kullanıcılar o rolü ortak kullanırlar. böylece 3 rol ile 5 kişiyi temsil edebliriz.

//Ara bağlantılı tablolarda gereksiz nesne üretmesin istiyorsak AsNoTrackingWithIdentityResolution kullanmalıyız. çalışma hızı olarak AsNoTracking 'Den yavaş tır.

//var kitaplar =await context.Books.Include(b=>b.Authors).AsNoTrackingWithIdentityResolution().ToListAsync();
//ctor 'larda oluşan nesne adedini görüntülemek için her nesne üretildiğinde bir console.writeline çağırdık ve 2 yazara karşılık 5 kitap oluştuğunu gözlemledik


//var kullanicilar =await context.Users.Include(b=>b.Roles).AsNoTrackingWithIdentityResolution().ToListAsync();
//5 user nesnesine karşılık 4 rol nesnesi oluştu ve bir rol nesnesi birden fazla user nesnesi ile eşleşti.

#endregion

#region AsTracking  ile takip mekanizmasını etkinleştirmek
//context üzerinden gelen dataların changetracker tarafından takip edilmesini iradeli bir şekilde takip edilmesini sağlayan fonksiyondur
//UseQueryTrackingBehavior metodunun davranışı gereği uygulama seviyesinde CT'ın default olarak devrede olup olmamasını ayarlıyor olacağız. Eğer ki default olarak pasif hale getirilirse böyle bir durumda takip mekanizmasına ihtiyaç duyulan sorgularda AsTracker fonksiyonu kullanabilir ve bu sorguları takip altına alabiliriz.
//aşağıda eğer CT default olarak kapalı bile olsa kitaplar değişkenine gelen tüm veriler takip edilecektir.
//var kitaplar = await context.Books.AsTracking().ToListAsync();

#endregion

#region UseQueryTrackingBehavior
//Uygulama seviyesinde ilgili contexten gelen verilerin üzerinde CT mekanizmasının aktif mi pasif mi olacağını belirlememizi sağlayan konfigürasyon fonksiyonudur . context sınıfımız içinde optionsbuilder üzerinden çağırılır.


//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//{
//    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-P7KA77K\SQLEXPRESS; Database = TrackingDB; User ID=sa;Password=1234; TrustServerCertificate=true");
#region CT'yi pasif/atkif yapma yöntemi
//optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //burada CT'yi pasif hale getirdik
#endregion
//    
//}
#endregion







public class MyContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-P7KA77K\SQLEXPRESS; Database = TrackingDB; User ID=sa;Password=1234; TrustServerCertificate=true");
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

}

public class Author
{
    public Author()
    {
        Console.WriteLine("Yazar nesnesi oluştu");
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }

}

public class BookAuthor
{
    public int AuthorId { get; set; }
    public int BookId { get; set; }

    public Author Author { get; set; }
    public Book Book { get; set; }


}
public class Book
{
    public Book()
    {
        Console.WriteLine("Kitap nesnesi oluştu");
    }
    

    public int Id { get; set; }
    public string Name { get; set; }
    public int PageNumber { get; set; }

    public ICollection<Author> Authors { get; set; }


}

public class Role
{
    public Role()
    {
        Console.WriteLine("Role nesnesi oluştu");
    }
    public int Id { get; set; }
    public string RolAdi { get; set; }

    public ICollection<User> Users { get; set; }

}

public class User
{
    public User()
    {
        Console.WriteLine("User nesnesi oluştu.");
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Role> Roles { get; set; }

}

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public User User { get; set; }
    public Role Role { get; set; }

}