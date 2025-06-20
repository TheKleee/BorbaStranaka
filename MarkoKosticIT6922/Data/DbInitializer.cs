using MarkoKosticIT6922.Constants;
using MarkoKosticIT6922.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace MarkoKosticIT6922.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<Glasac>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetRequiredService<ApplicationDbContext>();

            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            var user = new Glasac
            {
                UserName = "admin@raf.rs",
                Email = "admin@raf.rs",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);

            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin123$");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }

            if (!context.Stranke.Any())
            {
                var stranke = new List<Stranka>
                {
                    new Stranka { Naziv = "SLN", Opis = "Stranka losih namera: Sve poseduju politicari, pa nema drugih lopova u zemlji!" },
                    new Stranka { Naziv = "DEX", Opis = "Diktator express: Jedan covek sve bira, zato ne moramo da razmisljamo uopste!" },
                    new Stranka { Naziv = "KKK", Opis = "Komsija kod kuce: Borimo se da svi ostanemo kod kuce i niko ne ide na posao!" },
                };
                context.Stranke.AddRange(stranke);
                await context.SaveChangesAsync();

                var argumenti = new List<Argument>
                {
                    new Argument { Tekst = "Ukinimo porez za politicare!", StrankaId = stranke[0].StrankaId },
                    new Argument { Tekst = "Zabranimo skole, svi cemo imati misljenje!", StrankaId = stranke[0].StrankaId},
                    new Argument { Tekst = "Vodja je uvek u pravu!", StrankaId = stranke[1].StrankaId},
                    new Argument { Tekst = "Zaposlenje? Zasto kad mozemo svi biti kod kuce!", StrankaId = stranke[2].StrankaId },
                    new Argument { Tekst = "Ako su svi lopovi, barem znamo na cemu smo.", StrankaId = stranke[0].StrankaId },
                    new Argument { Tekst = "Uklanjanjem konkurencije, lopovluk je pod kontrolom.", StrankaId = stranke[0].StrankaId },
                    new Argument { Tekst = "Stabilnost kroz nepromenjivost – ne moze biti gore!", StrankaId = stranke[0].StrankaId },
                    new Argument { Tekst = "Brze se odlucuje kada odlucuje samo jedan covek.", StrankaId = stranke[1].StrankaId },
                    new Argument { Tekst = "Nema sukoba misljenja – mir u zemlji!", StrankaId = stranke[1].StrankaId },
                    new Argument { Tekst = "Vodja zna najbolje, demokratija samo usporava.", StrankaId = stranke[1].StrankaId },
                    new Argument { Tekst = "Manje saobracaja, vise vremena sa porodicom.", StrankaId = stranke[2].StrankaId },
                    new Argument { Tekst = "Ako svi ostanu kod kuce, nema guzve ni stresa.", StrankaId = stranke[2].StrankaId },
                    new Argument { Tekst = "Rad od kuce za sve – niko ne kasni na posao!", StrankaId = stranke[2].StrankaId },
                };

                context.Argumenti.AddRange(argumenti);
                await context.SaveChangesAsync();
            }
        }
    }
}
