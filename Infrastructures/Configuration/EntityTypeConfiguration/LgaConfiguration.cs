using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class LgaConfiguration : IEntityTypeConfiguration<Lga>
    {
        public void Configure(EntityTypeBuilder<Lga> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);

            builder.HasOne(l => l.State)
                   .WithMany(s => s.Lgas)
                   .HasForeignKey(l => l.StateId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                    new Lga { Id = 1, Name = "Aba North", StateId = 1 },
                    new Lga { Id = 2, Name = "Aba South", StateId = 1 },
                    new Lga { Id = 3, Name = "Arochukwu", StateId = 1 },
                    new Lga { Id = 4, Name = "Bende", StateId = 1 },
                    new Lga { Id = 5, Name = "Ikwuano", StateId = 1 },
                    new Lga { Id = 6, Name = "Isiala Ngwa North", StateId = 1 },
                    new Lga { Id = 7, Name = "Isiala Ngwa South", StateId = 1 },
                    new Lga { Id = 8, Name = "Isuikwuato", StateId = 1 },
                    new Lga { Id = 9, Name = "Obi Ngwa", StateId = 1 },
                    new Lga { Id = 10, Name = "Ohafia", StateId = 1 },
                    new Lga { Id = 11, Name = "Osisioma Ngwa", StateId = 1 },
                    new Lga { Id = 12, Name = "Ugwunagbo", StateId = 1 },
                    new Lga { Id = 13, Name = "Ukwa East", StateId = 1 },
                    new Lga { Id = 14, Name = "Ukwa West", StateId = 1 },
                    new Lga { Id = 15, Name = "Umuahia North", StateId = 1 },
                    new Lga { Id = 16, Name = "Umuahia South", StateId = 1 },
                    new Lga { Id = 17, Name = "Umu Nneochi", StateId = 1 }
             );

            builder.HasData(
                        new Lga { Id = 18, Name = "Demsa", StateId = 2 },
                        new Lga { Id = 19, Name = "Fufore", StateId = 2 },
                        new Lga { Id = 20, Name = "Ganye", StateId = 2 },
                        new Lga { Id = 21, Name = "Girei", StateId = 2 },
                        new Lga { Id = 22, Name = "Gombi", StateId = 2 },
                        new Lga { Id = 23, Name = "Guyuk", StateId = 2 },
                        new Lga { Id = 24, Name = "Hong", StateId = 2 },
                        new Lga { Id = 25, Name = "Jada", StateId = 2 },
                        new Lga { Id = 26, Name = "Lamurde", StateId = 2 },
                        new Lga { Id = 27, Name = "Madagali", StateId = 2 },
                        new Lga { Id = 28, Name = "Maiha", StateId = 2 },
                        new Lga { Id = 29, Name = "Mayo-Belwa", StateId = 2 },
                        new Lga { Id = 30, Name = "Michika", StateId = 2 },
                        new Lga { Id = 31, Name = "Mubi North", StateId = 2 },
                        new Lga { Id = 32, Name = "Mubi South", StateId = 2 },
                        new Lga { Id = 33, Name = "Numan", StateId = 2 },
                        new Lga { Id = 34, Name = "Shelleng", StateId = 2 },
                        new Lga { Id = 35, Name = "Song", StateId = 2 },
                        new Lga { Id = 36, Name = "Toungo", StateId = 2 },
                        new Lga { Id = 37, Name = "Yola North", StateId = 2 },
                        new Lga { Id = 38, Name = "Yola South", StateId = 2 }
            );

            builder.HasData(
            new Lga { Id = 39, Name = "Abak", StateId = 3 },
            new Lga { Id = 40, Name = "Eastern Obolo", StateId = 3 },
            new Lga { Id = 41, Name = "Eket", StateId = 3 },
            new Lga { Id = 42, Name = "Esit Eket", StateId = 3 },
            new Lga { Id = 43, Name = "Essien Udim", StateId = 3 },
            new Lga { Id = 44, Name = "Etim Ekpo", StateId = 3 },
            new Lga { Id = 45, Name = "Etinan", StateId = 3 },
            new Lga { Id = 46, Name = "Ibeno", StateId = 3 },
            new Lga { Id = 47, Name = "Ibesikpo Asutan", StateId = 3 },
            new Lga { Id = 48, Name = "Ibiono Ibom", StateId = 3 },
            new Lga { Id = 49, Name = "Ika", StateId = 3 },
            new Lga { Id = 50, Name = "Ikono", StateId = 3 },
            new Lga { Id = 51, Name = "Ikot Abasi", StateId = 3 },
            new Lga { Id = 52, Name = "Ikot Ekpene", StateId = 3 },
            new Lga { Id = 53, Name = "Ini", StateId = 3 },
            new Lga { Id = 54, Name = "Itu", StateId = 3 },
            new Lga { Id = 55, Name = "Mbo", StateId = 3 },
            new Lga { Id = 56, Name = "Mkpat Enin", StateId = 3 },
            new Lga { Id = 57, Name = "Nsit Atai", StateId = 3 },
            new Lga { Id = 58, Name = "Nsit Ibom", StateId = 3 },
            new Lga { Id = 59, Name = "Nsit Ubium", StateId = 3 },
            new Lga { Id = 60, Name = "Obot Akara", StateId = 3 },
            new Lga { Id = 61, Name = "Okobo", StateId = 3 },
            new Lga { Id = 62, Name = "Onna", StateId = 3 },
            new Lga { Id = 63, Name = "Oron", StateId = 3 },
            new Lga { Id = 64, Name = "Oruk Anam", StateId = 3 },
            new Lga { Id = 65, Name = "Udung Uko", StateId = 3 },
            new Lga { Id = 66, Name = "Ukanafun", StateId = 3 },
            new Lga { Id = 67, Name = "Uruan", StateId = 3 },
            new Lga { Id = 68, Name = "Urue-Offong/Oruko", StateId = 3 },
            new Lga { Id = 69, Name = "Uyo", StateId = 3 }
        );

            builder.HasData(
               new Lga { Id = 70, Name = "Aguata", StateId = 4 },
               new Lga { Id = 71, Name = "Anambra East", StateId = 4 },
               new Lga { Id = 72, Name = "Anambra West", StateId = 4 },
               new Lga { Id = 73, Name = "Anaocha", StateId = 4 },
               new Lga { Id = 74, Name = "Awka North", StateId = 4 },
               new Lga { Id = 75, Name = "Awka South", StateId = 4 },
               new Lga { Id = 76, Name = "Ayamelum", StateId = 4 },
               new Lga { Id = 77, Name = "Dunukofia", StateId = 4 },
               new Lga { Id = 78, Name = "Ekwusigo", StateId = 4 },
               new Lga { Id = 79, Name = "Idemili North", StateId = 4 },
               new Lga { Id = 80, Name = "Idemili South", StateId = 4 },
               new Lga { Id = 81, Name = "Ihiala", StateId = 4 },
               new Lga { Id = 82, Name = "Njikoka", StateId = 4 },
               new Lga { Id = 83, Name = "Nnewi North", StateId = 4 },
               new Lga { Id = 84, Name = "Nnewi South", StateId = 4 },
               new Lga { Id = 85, Name = "Ogbaru", StateId = 4 },
               new Lga { Id = 86, Name = "Onitsha North", StateId = 4 },
               new Lga { Id = 87, Name = "Onitsha South", StateId = 4 },
               new Lga { Id = 88, Name = "Orumba North", StateId = 4 },
               new Lga { Id = 89, Name = "Orumba South", StateId = 4 },
               new Lga { Id = 90, Name = "Oyi", StateId = 4 }
           );

            builder.HasData(
                new Lga { Id = 91, Name = "Alkaleri", StateId = 5 },
                new Lga { Id = 92, Name = "Bauchi", StateId = 5 },
                new Lga { Id = 93, Name = "Bogoro", StateId = 5 },
                new Lga { Id = 94, Name = "Damban", StateId = 5 },
                new Lga { Id = 95, Name = "Darazo", StateId = 5 },
                new Lga { Id = 96, Name = "Dass", StateId = 5 },
                new Lga { Id = 97, Name = "Gamawa", StateId = 5 },
                new Lga { Id = 98, Name = "Ganjuwa", StateId = 5 },
                new Lga { Id = 99, Name = "Giade", StateId = 5 },
                new Lga { Id = 100, Name = "Itas/Gadau", StateId = 5 },
                new Lga { Id = 101, Name = "Jama'are", StateId = 5 },
                new Lga { Id = 102, Name = "Katagum", StateId = 5 },
                new Lga { Id = 103, Name = "Kirfi", StateId = 5 },
                new Lga { Id = 104, Name = "Misau", StateId = 5 },
                new Lga { Id = 105, Name = "Ningi", StateId = 5 },
                new Lga { Id = 106, Name = "Shira", StateId = 5 },
                new Lga { Id = 107, Name = "Tafawa Balewa", StateId = 5 },
                new Lga { Id = 108, Name = "Toro", StateId = 5 },
                new Lga { Id = 109, Name = "Warji", StateId = 5 },
                new Lga { Id = 110, Name = "Zaki", StateId = 5 }
            );

            builder.HasData(
            new Lga { Id = 111, Name = "Brass", StateId = 6 },
            new Lga { Id = 112, Name = "Ekeremor", StateId = 6 },
            new Lga { Id = 113, Name = "Kolokuma/Opokuma", StateId = 6 },
            new Lga { Id = 114, Name = "Nembe", StateId = 6 },
            new Lga { Id = 115, Name = "Ogbia", StateId = 6 },
            new Lga { Id = 116, Name = "Sagbama", StateId = 6 },
            new Lga { Id = 117, Name = "Southern Ijaw", StateId = 6 },
            new Lga { Id = 118, Name = "Yenagoa", StateId = 6 }
           );

            builder.HasData(
            new Lga { Id = 119, Name = "Ado", StateId = 7 },
            new Lga { Id = 120, Name = "Agatu", StateId = 7 },
            new Lga { Id = 121, Name = "Apa", StateId = 7 },
            new Lga { Id = 122, Name = "Buruku", StateId = 7 },
            new Lga { Id = 123, Name = "Gboko", StateId = 7 },
            new Lga { Id = 124, Name = "Guma", StateId = 7 },
            new Lga { Id = 125, Name = "Gwer East", StateId = 7 },
            new Lga { Id = 126, Name = "Gwer West", StateId = 7 },
            new Lga { Id = 127, Name = "Katsina-Ala", StateId = 7 },
            new Lga { Id = 128, Name = "Konshisha", StateId = 7 },
            new Lga { Id = 129, Name = "Kwande", StateId = 7 },
            new Lga { Id = 130, Name = "Logo", StateId = 7 },
            new Lga { Id = 131, Name = "Makurdi", StateId = 7 },
            new Lga { Id = 132, Name = "Obi", StateId = 7 },
            new Lga { Id = 133, Name = "Ogbadibo", StateId = 7 },
            new Lga { Id = 134, Name = "Oju", StateId = 7 },
            new Lga { Id = 135, Name = "Okpokwu", StateId = 7 },
            new Lga { Id = 136, Name = "Otukpo", StateId = 7 },
            new Lga { Id = 137, Name = "Tarka", StateId = 7 },
            new Lga { Id = 138, Name = "Ukum", StateId = 7 },
            new Lga { Id = 139, Name = "Ushongo", StateId = 7 },
            new Lga { Id = 140, Name = "Vandeikya", StateId = 7 }
        );

            builder.HasData(
                new Lga { Id = 141, Name = "Abadam", StateId = 8 },
                new Lga { Id = 142, Name = "Askira/Uba", StateId = 8 },
                new Lga { Id = 143, Name = "Bama", StateId = 8 },
                new Lga { Id = 144, Name = "Bayo", StateId = 8 },
                new Lga { Id = 145, Name = "Biu", StateId = 8 },
                new Lga { Id = 146, Name = "Chibok", StateId = 8 },
                new Lga { Id = 147, Name = "Damboa", StateId = 8 },
                new Lga { Id = 148, Name = "Dikwa", StateId = 8 },
                new Lga { Id = 149, Name = "Gubio", StateId = 8 },
                new Lga { Id = 150, Name = "Guzamala", StateId = 8 },
                new Lga { Id = 151, Name = "Gwoza", StateId = 8 },
                new Lga { Id = 152, Name = "Hawul", StateId = 8 },
                new Lga { Id = 153, Name = "Jere", StateId = 8 },
                new Lga { Id = 154, Name = "Kaga", StateId = 8 },
                new Lga { Id = 155, Name = "Kala/Balge", StateId = 8 },
                new Lga { Id = 156, Name = "Konduga", StateId = 8 },
                new Lga { Id = 157, Name = "Kukawa", StateId = 8 },
                new Lga { Id = 158, Name = "Kwaya Kusar", StateId = 8 },
                new Lga { Id = 159, Name = "Mafa", StateId = 8 },
                new Lga { Id = 160, Name = "Magumeri", StateId = 8 },
                new Lga { Id = 161, Name = "Maiduguri", StateId = 8 },
                new Lga { Id = 162, Name = "Marte", StateId = 8 },
                new Lga { Id = 163, Name = "Mobbar", StateId = 8 },
                new Lga { Id = 164, Name = "Monguno", StateId = 8 },
                new Lga { Id = 165, Name = "Ngala", StateId = 8 },
                new Lga { Id = 166, Name = "Nganzai", StateId = 8 },
                new Lga { Id = 167, Name = "Shani", StateId = 8 }
            );


            builder.HasData(
                new Lga { Id = 168, Name = "Akpabuyo", StateId = 9 },
                new Lga { Id = 169, Name = "Bakassi", StateId = 9 },
                new Lga { Id = 170, Name = "Bekwara", StateId = 9 },
                new Lga { Id = 171, Name = "Biase", StateId = 9 },
                new Lga { Id = 172, Name = "Boki", StateId = 9 },
                new Lga { Id = 173, Name = "Calabar Municipal", StateId = 9 },
                new Lga { Id = 174, Name = "Calabar South", StateId = 9 },
                new Lga { Id = 175, Name = "Etung", StateId = 9 },
                new Lga { Id = 176, Name = "Ikom", StateId = 9 },
                new Lga { Id = 177, Name = "Obanliku", StateId = 9 },
                new Lga { Id = 178, Name = "Obubra", StateId = 9 },
                new Lga { Id = 179, Name = "Obudu", StateId = 9 },
                new Lga { Id = 180, Name = "Odukpani", StateId = 9 },
                new Lga { Id = 181, Name = "Ogoja", StateId = 9 },
                new Lga { Id = 182, Name = "Yakuur", StateId = 9 },
                new Lga { Id = 183, Name = "Yala", StateId = 9 },
                new Lga { Id = 184, Name = "Akampka", StateId = 9 },
                new Lga { Id = 185, Name = "Obudu (Obanliku)", StateId = 9 }
            );


            builder.HasData(
                new Lga { Id = 186, Name = "Aniocha North", StateId = 10 },
                new Lga { Id = 187, Name = "Aniocha South", StateId = 10 },
                new Lga { Id = 188, Name = "Bomadi", StateId = 10 },
                new Lga { Id = 189, Name = "Burutu", StateId = 10 },
                new Lga { Id = 190, Name = "Ethiope East", StateId = 10 },
                new Lga { Id = 191, Name = "Ethiope West", StateId = 10 },
                new Lga { Id = 192, Name = "Ika North East", StateId = 10 },
                new Lga { Id = 193, Name = "Ika South", StateId = 10 },
                new Lga { Id = 194, Name = "Isoko North", StateId = 10 },
                new Lga { Id = 195, Name = "Isoko South", StateId = 10 },
                new Lga { Id = 196, Name = "Ndokwa East", StateId = 10 },
                new Lga { Id = 197, Name = "Ndokwa West", StateId = 10 },
                new Lga { Id = 198, Name = "Okpe", StateId = 10 },
                new Lga { Id = 199, Name = "Oshimili North", StateId = 10 },
                new Lga { Id = 200, Name = "Oshimili South", StateId = 10 },
                new Lga { Id = 201, Name = "Patani", StateId = 10 },
                new Lga { Id = 202, Name = "Sapele", StateId = 10 },
                new Lga { Id = 203, Name = "Udu", StateId = 10 },
                new Lga { Id = 204, Name = "Ughelli North", StateId = 10 },
                new Lga { Id = 205, Name = "Ughelli South", StateId = 10 },
                new Lga { Id = 206, Name = "Ukwuani", StateId = 10 },
                new Lga { Id = 207, Name = "Uvwie", StateId = 10 },
                new Lga { Id = 208, Name = "Warri North", StateId = 10 },
                new Lga { Id = 209, Name = "Warri South", StateId = 10 },
                new Lga { Id = 210, Name = "Warri South West", StateId = 10 }
            );


            builder.HasData(
            new Lga { Id = 211, Name = "Abakaliki", StateId = 11 },
            new Lga { Id = 212, Name = "Afikpo North", StateId = 11 },
            new Lga { Id = 213, Name = "Afikpo South (Edda)", StateId = 11 },
            new Lga { Id = 214, Name = "Ebonyi", StateId = 11 },
            new Lga { Id = 215, Name = "Ezza North", StateId = 11 },
            new Lga { Id = 216, Name = "Ezza South", StateId = 11 },
            new Lga { Id = 217, Name = "Ikwo", StateId = 11 },
            new Lga { Id = 218, Name = "Ishielu", StateId = 11 },
            new Lga { Id = 219, Name = "Ivo", StateId = 11 },
            new Lga { Id = 220, Name = "Izzi", StateId = 11 },
            new Lga { Id = 221, Name = "Ohaozara", StateId = 11 },
            new Lga { Id = 222, Name = "Ohaukwu", StateId = 11 },
            new Lga { Id = 223, Name = "Onicha", StateId = 11 }
        );

            builder.HasData(
            new Lga { Id = 224, Name = "Akoko-Edo", StateId = 12 },
            new Lga { Id = 225, Name = "Egor", StateId = 12 },
            new Lga { Id = 226, Name = "Esan Central", StateId = 12 },
            new Lga { Id = 227, Name = "Esan North-East", StateId = 12 },
            new Lga { Id = 228, Name = "Esan South-East", StateId = 12 },
            new Lga { Id = 229, Name = "Esan West", StateId = 12 },
            new Lga { Id = 230, Name = "Etsako Central", StateId = 12 },
            new Lga { Id = 231, Name = "Etsako East", StateId = 12 },
            new Lga { Id = 232, Name = "Etsako West", StateId = 12 },
            new Lga { Id = 233, Name = "Igueben", StateId = 12 },
            new Lga { Id = 234, Name = "Ikpoba-Okha", StateId = 12 },
            new Lga { Id = 235, Name = "Oredo", StateId = 12 },
            new Lga { Id = 236, Name = "Orhionmwon", StateId = 12 },
            new Lga { Id = 237, Name = "Ovia North-East", StateId = 12 },
            new Lga { Id = 238, Name = "Ovia South-West", StateId = 12 },
            new Lga { Id = 239, Name = "Owan East", StateId = 12 },
            new Lga { Id = 240, Name = "Owan West", StateId = 12 },
            new Lga { Id = 241, Name = "Uhunmwonde", StateId = 12 }
        );


            builder.HasData(
                new Lga { Id = 242, Name = "Ado Ekiti", StateId = 13 },
                new Lga { Id = 243, Name = "Efon", StateId = 13 },
                new Lga { Id = 244, Name = "Ekiti East", StateId = 13 },
                new Lga { Id = 245, Name = "Ekiti South-West", StateId = 13 },
                new Lga { Id = 246, Name = "Ekiti West", StateId = 13 },
                new Lga { Id = 247, Name = "Emure", StateId = 13 },
                new Lga { Id = 248, Name = "Gbonyin", StateId = 13 },
                new Lga { Id = 249, Name = "Ido-Osi", StateId = 13 },
                new Lga { Id = 250, Name = "Ijero", StateId = 13 },
                new Lga { Id = 251, Name = "Ikere", StateId = 13 },
                new Lga { Id = 252, Name = "Ikole", StateId = 13 },
                new Lga { Id = 253, Name = "Ilejemeje", StateId = 13 },
                new Lga { Id = 254, Name = "Irepodun/Ifelodun", StateId = 13 },
                new Lga { Id = 255, Name = "Ise/Orun", StateId = 13 },
                new Lga { Id = 256, Name = "Moba", StateId = 13 },
                new Lga { Id = 257, Name = "Oye", StateId = 13 }
            );


            builder.HasData(
                new Lga { Id = 258, Name = "Aninri", StateId = 14 },
                new Lga { Id = 259, Name = "Awgu", StateId = 14 },
                new Lga { Id = 260, Name = "Enugu East", StateId = 14 },
                new Lga { Id = 261, Name = "Enugu North", StateId = 14 },
                new Lga { Id = 262, Name = "Enugu South", StateId = 14 },
                new Lga { Id = 263, Name = "Ezeagu", StateId = 14 },
                new Lga { Id = 264, Name = "Igbo Etiti", StateId = 14 },
                new Lga { Id = 265, Name = "Igbo Eze North", StateId = 14 },
                new Lga { Id = 266, Name = "Igbo Eze South", StateId = 14 },
                new Lga { Id = 267, Name = "Isi Uzo", StateId = 14 },
                new Lga { Id = 268, Name = "Nkanu East", StateId = 14 },
                new Lga { Id = 269, Name = "Nkanu West", StateId = 14 },
                new Lga { Id = 270, Name = "Nsukka", StateId = 14 },
                new Lga { Id = 271, Name = "Oji River", StateId = 14 },
                new Lga { Id = 272, Name = "Udenu", StateId = 14 },
                new Lga { Id = 273, Name = "Udi", StateId = 14 },
                new Lga { Id = 274, Name = "Uzo-Uwani", StateId = 14 }
            );

            builder.HasData(
            new Lga { Id = 275, Name = "Akko", StateId = 15 },
            new Lga { Id = 276, Name = "Balanga", StateId = 15 },
            new Lga { Id = 277, Name = "Billiri", StateId = 15 },
            new Lga { Id = 278, Name = "Dukku", StateId = 15 },
            new Lga { Id = 279, Name = "Funakaye", StateId = 15 },
            new Lga { Id = 280, Name = "Gombe", StateId = 15 },
            new Lga { Id = 281, Name = "Kaltungo", StateId = 15 },
            new Lga { Id = 282, Name = "Kwami", StateId = 15 },
            new Lga { Id = 283, Name = "Nafada/Bajoga", StateId = 15 },
            new Lga { Id = 284, Name = "Yamaltu/Deba", StateId = 15 },
            new Lga { Id = 285, Name = "Gombe (city)", StateId = 15 }
        );


            builder.HasData(
                new Lga { Id = 286, Name = "Aboh Mbaise", StateId = 16 },
                new Lga { Id = 287, Name = "Ahiazu Mbaise", StateId = 16 },
                new Lga { Id = 288, Name = "Ehime Mbano", StateId = 16 },
                new Lga { Id = 289, Name = "Ezinihitte", StateId = 16 },
                new Lga { Id = 290, Name = "Ideato North", StateId = 16 },
                new Lga { Id = 291, Name = "Ideato South", StateId = 16 },
                new Lga { Id = 292, Name = "Ihitte/Uboma", StateId = 16 },
                new Lga { Id = 293, Name = "Ikeduru", StateId = 16 },
                new Lga { Id = 294, Name = "Isiala Mbano", StateId = 16 },
                new Lga { Id = 295, Name = "Isu", StateId = 16 },
                new Lga { Id = 296, Name = "Mbaitoli", StateId = 16 },
                new Lga { Id = 297, Name = "Ngor Okpala", StateId = 16 },
                new Lga { Id = 298, Name = "Nkwerre", StateId = 16 },
                new Lga { Id = 299, Name = "Nwangele", StateId = 16 },
                new Lga { Id = 300, Name = "Obowo", StateId = 16 },
                new Lga { Id = 301, Name = "Oguta", StateId = 16 },
                new Lga { Id = 302, Name = "Ohaji/Egbema", StateId = 16 },
                new Lga { Id = 303, Name = "Okigwe", StateId = 16 },
                new Lga { Id = 304, Name = "Onuimo", StateId = 16 },
                new Lga { Id = 305, Name = "Orlu", StateId = 16 },
                new Lga { Id = 306, Name = "Orsu", StateId = 16 },
                new Lga { Id = 307, Name = "Oru East", StateId = 16 },
                new Lga { Id = 308, Name = "Oru West", StateId = 16 },
                new Lga { Id = 309, Name = "Owerri Municipal", StateId = 16 },
                new Lga { Id = 310, Name = "Owerri North", StateId = 16 },
                new Lga { Id = 311, Name = "Owerri West", StateId = 16 },
                new Lga { Id = 312, Name = "Ehime Mbano", StateId = 16 }
            );


            builder.HasData(
            new Lga { Id = 313, Name = "Auyo", StateId = 17 },
            new Lga { Id = 314, Name = "Babura", StateId = 17 },
            new Lga { Id = 315, Name = "Biriniwa", StateId = 17 },
            new Lga { Id = 316, Name = "Birnin Kudu", StateId = 17 },
            new Lga { Id = 317, Name = "Buji", StateId = 17 },
            new Lga { Id = 318, Name = "Dutse", StateId = 17 },
            new Lga { Id = 319, Name = "Gagarawa", StateId = 17 },
            new Lga { Id = 320, Name = "Garki", StateId = 17 },
            new Lga { Id = 321, Name = "Gumel", StateId = 17 },
            new Lga { Id = 322, Name = "Guri", StateId = 17 },
            new Lga { Id = 323, Name = "Gwaram", StateId = 17 },
            new Lga { Id = 324, Name = "Gwiwa", StateId = 17 },
            new Lga { Id = 325, Name = "Hadejia", StateId = 17 },
            new Lga { Id = 326, Name = "Jahun", StateId = 17 },
            new Lga { Id = 327, Name = "Kafin Hausa", StateId = 17 },
            new Lga { Id = 328, Name = "Kaugama", StateId = 17 },
            new Lga { Id = 329, Name = "Kazaure", StateId = 17 },
            new Lga { Id = 330, Name = "Kiri Kasama", StateId = 17 },
            new Lga { Id = 331, Name = "Kiyawa", StateId = 17 },
            new Lga { Id = 332, Name = "Maigatari", StateId = 17 },
            new Lga { Id = 333, Name = "Malam Madori", StateId = 17 },
            new Lga { Id = 334, Name = "Miga", StateId = 17 },
            new Lga { Id = 335, Name = "Ringim", StateId = 17 },
            new Lga { Id = 336, Name = "Roni", StateId = 17 },
            new Lga { Id = 337, Name = "Sule Tankarkar", StateId = 17 },
            new Lga { Id = 338, Name = "Taura", StateId = 17 },
            new Lga { Id = 339, Name = "Yankwashi", StateId = 17 }
        );


            builder.HasData(
            new Lga { Id = 340, Name = "Birnin Gwari", StateId = 18 },
            new Lga { Id = 341, Name = "Chikun", StateId = 18 },
            new Lga { Id = 342, Name = "Giwa", StateId = 18 },
            new Lga { Id = 343, Name = "Igabi", StateId = 18 },
            new Lga { Id = 344, Name = "Ikara", StateId = 18 },
            new Lga { Id = 345, Name = "Jaba", StateId = 18 },
            new Lga { Id = 346, Name = "Jema'a", StateId = 18 },
            new Lga { Id = 347, Name = "Kachia", StateId = 18 },
            new Lga { Id = 348, Name = "Kaduna North", StateId = 18 },
            new Lga { Id = 349, Name = "Kaduna South", StateId = 18 },
            new Lga { Id = 350, Name = "Kagarko", StateId = 18 },
            new Lga { Id = 351, Name = "Kajuru", StateId = 18 },
            new Lga { Id = 352, Name = "Kaura", StateId = 18 },
            new Lga { Id = 353, Name = "Kauru", StateId = 18 },
            new Lga { Id = 354, Name = "Kubau", StateId = 18 },
            new Lga { Id = 355, Name = "Kudan", StateId = 18 },
            new Lga { Id = 356, Name = "Lere", StateId = 18 },
            new Lga { Id = 357, Name = "Makarfi", StateId = 18 },
            new Lga { Id = 358, Name = "Sabon Gari", StateId = 18 },
            new Lga { Id = 359, Name = "Sanga", StateId = 18 },
            new Lga { Id = 360, Name = "Soba", StateId = 18 },
            new Lga { Id = 361, Name = "Zangon Kataf", StateId = 18 },
            new Lga { Id = 362, Name = "Zaria", StateId = 18 }
        );


            builder.HasData(
                new Lga { Id = 363, Name = "Ajingi", StateId = 19 },
                new Lga { Id = 364, Name = "Albasu", StateId = 19 },
                new Lga { Id = 365, Name = "Bagwai", StateId = 19 },
                new Lga { Id = 366, Name = "Bebeji", StateId = 19 },
                new Lga { Id = 367, Name = "Bichi", StateId = 19 },
                new Lga { Id = 368, Name = "Bunkure", StateId = 19 },
                new Lga { Id = 369, Name = "Dala", StateId = 19 },
                new Lga { Id = 370, Name = "Dambatta", StateId = 19 },
                new Lga { Id = 371, Name = "Dawakin Kudu", StateId = 19 },
                new Lga { Id = 372, Name = "Dawakin Tofa", StateId = 19 },
                new Lga { Id = 373, Name = "Doguwa", StateId = 19 },
                new Lga { Id = 374, Name = "Fagge", StateId = 19 },
                new Lga { Id = 375, Name = "Gabasawa", StateId = 19 },
                new Lga { Id = 376, Name = "Garko", StateId = 19 },
                new Lga { Id = 377, Name = "Garun Mallam", StateId = 19 },
                new Lga { Id = 378, Name = "Gaya", StateId = 19 },
                new Lga { Id = 379, Name = "Gezawa", StateId = 19 },
                new Lga { Id = 380, Name = "Gwale", StateId = 19 },
                new Lga { Id = 381, Name = "Gwarzo", StateId = 19 },
                new Lga { Id = 382, Name = "Kabo", StateId = 19 },
                new Lga { Id = 383, Name = "Kano Municipal", StateId = 19 },
                new Lga { Id = 384, Name = "Karaye", StateId = 19 },
                new Lga { Id = 385, Name = "Kibiya", StateId = 19 },
                new Lga { Id = 386, Name = "Kiru", StateId = 19 },
                new Lga { Id = 387, Name = "Kumbotso", StateId = 19 },
                new Lga { Id = 388, Name = "Kunchi", StateId = 19 },
                new Lga { Id = 389, Name = "Kura", StateId = 19 },
                new Lga { Id = 390, Name = "Madobi", StateId = 19 },
                new Lga { Id = 391, Name = "Makoda", StateId = 19 },
                new Lga { Id = 392, Name = "Minjibir", StateId = 19 },
                new Lga { Id = 393, Name = "Nasarawa", StateId = 19 },
                new Lga { Id = 394, Name = "Rano", StateId = 19 },
                new Lga { Id = 395, Name = "Rimin Gado", StateId = 19 },
                new Lga { Id = 396, Name = "Rogo", StateId = 19 },
                new Lga { Id = 397, Name = "Shanono", StateId = 19 },
                new Lga { Id = 398, Name = "Sumaila", StateId = 19 },
                new Lga { Id = 399, Name = "Tarauni", StateId = 19 },
                new Lga { Id = 400, Name = "Tofa", StateId = 19 },
                new Lga { Id = 401, Name = "Tsanyawa", StateId = 19 },
                new Lga { Id = 402, Name = "Tudun Wada", StateId = 19 },
                new Lga { Id = 403, Name = "Ungogo", StateId = 19 },
                new Lga { Id = 404, Name = "Warawa", StateId = 19 },
                new Lga { Id = 405, Name = "Wudil", StateId = 19 }
            );

            builder.HasData(
            new Lga { Id = 406, Name = "Bakori", StateId = 20 },
            new Lga { Id = 407, Name = "Batagarawa", StateId = 20 },
            new Lga { Id = 408, Name = "Batsari", StateId = 20 },
            new Lga { Id = 409, Name = "Baure", StateId = 20 },
            new Lga { Id = 410, Name = "Bindawa", StateId = 20 },
            new Lga { Id = 411, Name = "Charanchi", StateId = 20 },
            new Lga { Id = 412, Name = "Dandume", StateId = 20 },
            new Lga { Id = 413, Name = "Danja", StateId = 20 },
            new Lga { Id = 414, Name = "Dutse", StateId = 20 },
            new Lga { Id = 415, Name = "Faskari", StateId = 20 },
            new Lga { Id = 416, Name = "Funtua", StateId = 20 },
            new Lga { Id = 417, Name = "Ingawa", StateId = 20 },
            new Lga { Id = 418, Name = "Jibia", StateId = 20 },
            new Lga { Id = 419, Name = "Kafur", StateId = 20 },
            new Lga { Id = 420, Name = "Kaita", StateId = 20 },
            new Lga { Id = 421, Name = "Kankara", StateId = 20 },
            new Lga { Id = 422, Name = "Kankia", StateId = 20 },
            new Lga { Id = 423, Name = "Katsina", StateId = 20 },
            new Lga { Id = 424, Name = "Kurfi", StateId = 20 },
            new Lga { Id = 425, Name = "Kusada", StateId = 20 },
            new Lga { Id = 426, Name = "Mai'Adua", StateId = 20 },
            new Lga { Id = 427, Name = "Malumfashi", StateId = 20 },
            new Lga { Id = 428, Name = "Mani", StateId = 20 },
            new Lga { Id = 429, Name = "Mashi", StateId = 20 },
            new Lga { Id = 430, Name = "Matazu", StateId = 20 },
            new Lga { Id = 431, Name = "Musawa", StateId = 20 },
            new Lga { Id = 432, Name = "Rimi", StateId = 20 },
            new Lga { Id = 433, Name = "Sabuwa", StateId = 20 },
            new Lga { Id = 434, Name = "Safana", StateId = 20 },
            new Lga { Id = 435, Name = "Sandamu", StateId = 20 },
            new Lga { Id = 436, Name = "Zango", StateId = 20 },
            new Lga { Id = 437, Name = "Danja (city)", StateId = 20 },
            new Lga { Id = 438, Name = "Funtua (city)", StateId = 20 }
        );


            builder.HasData(
            new Lga { Id = 439, Name = "Aleiro", StateId = 21 },
            new Lga { Id = 440, Name = "Arewa Dandi", StateId = 21 },
            new Lga { Id = 441, Name = "Argungu", StateId = 21 },
            new Lga { Id = 442, Name = "Augie", StateId = 21 },
            new Lga { Id = 443, Name = "Bagudo", StateId = 21 },
            new Lga { Id = 444, Name = "Birnin Kebbi", StateId = 21 },
            new Lga { Id = 445, Name = "Bunza", StateId = 21 },
            new Lga { Id = 446, Name = "Dandi", StateId = 21 },
            new Lga { Id = 447, Name = "Fakai", StateId = 21 },
            new Lga { Id = 448, Name = "Gwandu", StateId = 21 },
            new Lga { Id = 449, Name = "Jega", StateId = 21 },
            new Lga { Id = 450, Name = "Kalgo", StateId = 21 },
            new Lga { Id = 451, Name = "Koko Besse", StateId = 21 },
            new Lga { Id = 452, Name = "Maiyama", StateId = 21 },
            new Lga { Id = 453, Name = "Ngaski", StateId = 21 },
            new Lga { Id = 454, Name = "Sakaba", StateId = 21 },
            new Lga { Id = 455, Name = "Shanga", StateId = 21 },
            new Lga { Id = 456, Name = "Suru", StateId = 21 },
            new Lga { Id = 457, Name = "Wasagu/Danko", StateId = 21 },
            new Lga { Id = 458, Name = "Yauri", StateId = 21 },
            new Lga { Id = 459, Name = "Zuru", StateId = 21 }
        );

            builder.HasData(
                new Lga { Id = 460, Name = "Adavi", StateId = 22 },
                new Lga { Id = 461, Name = "Ajaokuta", StateId = 22 },
                new Lga { Id = 462, Name = "Ankpa", StateId = 22 },
                new Lga { Id = 463, Name = "Bassa", StateId = 22 },
                new Lga { Id = 464, Name = "Dekina", StateId = 22 },
                new Lga { Id = 465, Name = "Ibaji", StateId = 22 },
                new Lga { Id = 466, Name = "Idah", StateId = 22 },
                new Lga { Id = 467, Name = "Igalamela-Odolu", StateId = 22 },
                new Lga { Id = 468, Name = "Ijumu", StateId = 22 },
                new Lga { Id = 469, Name = "Kabba/Bunu", StateId = 22 },
                new Lga { Id = 470, Name = "Kogi", StateId = 22 },
                new Lga { Id = 471, Name = "Lokoja", StateId = 22 },
                new Lga { Id = 472, Name = "Mopa-Muro", StateId = 22 },
                new Lga { Id = 473, Name = "Ofu", StateId = 22 },
                new Lga { Id = 474, Name = "Ogori/Magongo", StateId = 22 },
                new Lga { Id = 475, Name = "Okehi", StateId = 22 },
                new Lga { Id = 476, Name = "Okene", StateId = 22 },
                new Lga { Id = 477, Name = "Olamaboro", StateId = 22 },
                new Lga { Id = 478, Name = "Omala", StateId = 22 },
                new Lga { Id = 479, Name = "Yagba East", StateId = 22 },
                new Lga { Id = 480, Name = "Yagba West", StateId = 22 }
            );


            builder.HasData(
                new Lga { Id = 481, Name = "Asa", StateId = 23 },
                new Lga { Id = 482, Name = "Baruten", StateId = 23 },
                new Lga { Id = 483, Name = "Edu", StateId = 23 },
                new Lga { Id = 484, Name = "Ekiti", StateId = 23 },
                new Lga { Id = 485, Name = "Ifelodun", StateId = 23 },
                new Lga { Id = 486, Name = "Ilorin East", StateId = 23 },
                new Lga { Id = 487, Name = "Ilorin South", StateId = 23 },
                new Lga { Id = 488, Name = "Ilorin West", StateId = 23 },
                new Lga { Id = 489, Name = "Irepodun", StateId = 23 },
                new Lga { Id = 490, Name = "Isin", StateId = 23 },
                new Lga { Id = 491, Name = "Kaiama", StateId = 23 },
                new Lga { Id = 492, Name = "Moro", StateId = 23 },
                new Lga { Id = 493, Name = "Offa", StateId = 23 },
                new Lga { Id = 494, Name = "Oke Ero", StateId = 23 },
                new Lga { Id = 495, Name = "Oyun", StateId = 23 },
                new Lga { Id = 496, Name = "Patigi", StateId = 23 }
            );

            builder.HasData(
               new Lga { Id = 497, Name = "Agege", StateId = 24 },
               new Lga { Id = 498, Name = "Ajeromi-Ifelodun", StateId = 24 },
               new Lga { Id = 499, Name = "Alimosho", StateId = 24 },
               new Lga { Id = 500, Name = "Amuwo-Odofin", StateId = 24 },
               new Lga { Id = 501, Name = "Apapa", StateId = 24 },
               new Lga { Id = 502, Name = "Badagry", StateId = 24 },
               new Lga { Id = 503, Name = "Epe", StateId = 24 },
               new Lga { Id = 504, Name = "Eti-Osa", StateId = 24 },
               new Lga { Id = 505, Name = "Ibeju-Lekki", StateId = 24 },
               new Lga { Id = 506, Name = "Ifako-Ijaiye", StateId = 24 },
               new Lga { Id = 507, Name = "Ikeja", StateId = 24 },
               new Lga { Id = 508, Name = "Ikorodu", StateId = 24 },
               new Lga { Id = 509, Name = "Kosofe", StateId = 24 },
               new Lga { Id = 510, Name = "Lagos Island", StateId = 24 },
               new Lga { Id = 511, Name = "Lagos Mainland", StateId = 24 },
               new Lga { Id = 512, Name = "Mushin", StateId = 24 },
               new Lga { Id = 513, Name = "Ojo", StateId = 24 },
               new Lga { Id = 514, Name = "Oshodi-Isolo", StateId = 24 },
               new Lga { Id = 515, Name = "Shomolu", StateId = 24 },
               new Lga { Id = 516, Name = "Surulere", StateId = 24 }
           );


            builder.HasData(
               new Lga { Id = 517, Name = "Akwanga", StateId = 25 },
               new Lga { Id = 518, Name = "Awe", StateId = 25 },
               new Lga { Id = 519, Name = "Doma", StateId = 25 },
               new Lga { Id = 520, Name = "Karu", StateId = 25 },
               new Lga { Id = 521, Name = "Keana", StateId = 25 },
               new Lga { Id = 522, Name = "Keffi", StateId = 25 },
               new Lga { Id = 523, Name = "Kokona", StateId = 25 },
               new Lga { Id = 524, Name = "Lafia", StateId = 25 },
               new Lga { Id = 525, Name = "Nasarawa", StateId = 25 },
               new Lga { Id = 526, Name = "Nasarawa Egon", StateId = 25 },
               new Lga { Id = 527, Name = "Obi", StateId = 25 },
               new Lga { Id = 528, Name = "Toto", StateId = 25 },
               new Lga { Id = 529, Name = "Wamba", StateId = 25 }
           );

            builder.HasData(
                new Lga { Id = 530, Name = "Agaie", StateId = 26 },
                new Lga { Id = 531, Name = "Agwara", StateId = 26 },
                new Lga { Id = 532, Name = "Bida", StateId = 26 },
                new Lga { Id = 533, Name = "Borgu", StateId = 26 },
                new Lga { Id = 534, Name = "Bosso", StateId = 26 },
                new Lga { Id = 535, Name = "Chanchaga", StateId = 26 },
                new Lga { Id = 536, Name = "Edati", StateId = 26 },
                new Lga { Id = 537, Name = "Gbako", StateId = 26 },
                new Lga { Id = 538, Name = "Gurara", StateId = 26 },
                new Lga { Id = 539, Name = "Katcha", StateId = 26 },
                new Lga { Id = 540, Name = "Kontagora", StateId = 26 },
                new Lga { Id = 541, Name = "Lapai", StateId = 26 },
                new Lga { Id = 542, Name = "Lavun", StateId = 26 },
                new Lga { Id = 543, Name = "Mokwa", StateId = 26 },
                new Lga { Id = 544, Name = "Muya", StateId = 26 },
                new Lga { Id = 545, Name = "Pailoro", StateId = 26 },
                new Lga { Id = 546, Name = "Rafi", StateId = 26 },
                new Lga { Id = 547, Name = "Rijau", StateId = 26 },
                new Lga { Id = 548, Name = "Shiroro", StateId = 26 },
                new Lga { Id = 549, Name = "Suleja", StateId = 26 },
                new Lga { Id = 550, Name = "Tafa", StateId = 26 },
                new Lga { Id = 551, Name = "Wushishi", StateId = 26 },
                new Lga { Id = 552, Name = "Mariga", StateId = 26 },
                new Lga { Id = 553, Name = "Mashegu", StateId = 26 },
                new Lga { Id = 554, Name = "Bosso", StateId = 26 }
            );


            builder.HasData(
              new Lga { Id = 555, Name = "Abeokuta North", StateId = 27 },
              new Lga { Id = 556, Name = "Abeokuta South", StateId = 27 },
              new Lga { Id = 557, Name = "Ado-Odo/Ota", StateId = 27 },
              new Lga { Id = 558, Name = "Egbado North (Yewa North)", StateId = 27 },
              new Lga { Id = 559, Name = "Egbado South (Yewa South)", StateId = 27 },
              new Lga { Id = 560, Name = "Ewekoro", StateId = 27 },
              new Lga { Id = 561, Name = "Ifo", StateId = 27 },
              new Lga { Id = 562, Name = "Ijebu East", StateId = 27 },
              new Lga { Id = 563, Name = "Ijebu North", StateId = 27 },
              new Lga { Id = 564, Name = "Ijebu North East", StateId = 27 },
              new Lga { Id = 565, Name = "Ijebu Ode", StateId = 27 },
              new Lga { Id = 566, Name = "Ikenne", StateId = 27 },
              new Lga { Id = 567, Name = "Imeko Afon", StateId = 27 },
              new Lga { Id = 568, Name = "Ipokia", StateId = 27 },
              new Lga { Id = 569, Name = "Obafemi Owode", StateId = 27 },
              new Lga { Id = 570, Name = "Odogbolu", StateId = 27 },
              new Lga { Id = 571, Name = "Ogun Waterside", StateId = 27 },
              new Lga { Id = 572, Name = "Remo North", StateId = 27 },
              new Lga { Id = 573, Name = "Shagamu", StateId = 27 },
              new Lga { Id = 574, Name = "Yewa North", StateId = 27 }
              );


            builder.HasData(
               new Lga { Id = 575, Name = "Akoko North-East", StateId = 28 },
               new Lga { Id = 576, Name = "Akoko North-West", StateId = 28 },
               new Lga { Id = 577, Name = "Akoko South-East", StateId = 28 },
               new Lga { Id = 578, Name = "Akoko South-West", StateId = 28 },
               new Lga { Id = 579, Name = "Akure North", StateId = 28 },
               new Lga { Id = 580, Name = "Akure South", StateId = 28 },
               new Lga { Id = 581, Name = "Ese-Odo", StateId = 28 },
               new Lga { Id = 582, Name = "Idanre", StateId = 28 },
               new Lga { Id = 583, Name = "Ifedore", StateId = 28 },
               new Lga { Id = 584, Name = "Ilaje", StateId = 28 },
               new Lga { Id = 585, Name = "Ile-Oluji/Okeigbo", StateId = 28 },
               new Lga { Id = 586, Name = "Irele", StateId = 28 },
               new Lga { Id = 587, Name = "Odigbo", StateId = 28 },
               new Lga { Id = 588, Name = "Okitipupa", StateId = 28 },
               new Lga { Id = 589, Name = "Ondo East", StateId = 28 },
               new Lga { Id = 590, Name = "Ondo West", StateId = 28 },
               new Lga { Id = 591, Name = "Ose", StateId = 28 },
               new Lga { Id = 592, Name = "Owo", StateId = 28 }
           );


            builder.HasData(
                new Lga { Id = 593, Name = "Atakunmosa East", StateId = 29 },
                new Lga { Id = 594, Name = "Atakunmosa West", StateId = 29 },
                new Lga { Id = 595, Name = "Aiyedaade", StateId = 29 },
                new Lga { Id = 596, Name = "Aiyedire", StateId = 29 },
                new Lga { Id = 597, Name = "Boluwaduro", StateId = 29 },
                new Lga { Id = 598, Name = "Boripe", StateId = 29 },
                new Lga { Id = 599, Name = "Ede North", StateId = 29 },
                new Lga { Id = 600, Name = "Ede South", StateId = 29 },
                new Lga { Id = 601, Name = "Egbedore", StateId = 29 },
                new Lga { Id = 602, Name = "Ejigbo", StateId = 29 },
                new Lga { Id = 603, Name = "Ife Central", StateId = 29 },
                new Lga { Id = 604, Name = "Ife East", StateId = 29 },
                new Lga { Id = 605, Name = "Ife North", StateId = 29 },
                new Lga { Id = 606, Name = "Ife South", StateId = 29 },
                new Lga { Id = 607, Name = "Ifedayo", StateId = 29 },
                new Lga { Id = 608, Name = "Ifelodun", StateId = 29 },
                new Lga { Id = 609, Name = "Ila", StateId = 29 },
                new Lga { Id = 610, Name = "Ilesa East", StateId = 29 },
                new Lga { Id = 611, Name = "Ilesa West", StateId = 29 },
                new Lga { Id = 612, Name = "Irepodun", StateId = 29 },
                new Lga { Id = 613, Name = "Irewole", StateId = 29 },
                new Lga { Id = 614, Name = "Isokan", StateId = 29 },
                new Lga { Id = 615, Name = "Iwo", StateId = 29 },
                new Lga { Id = 616, Name = "Obokun", StateId = 29 },
                new Lga { Id = 617, Name = "Odo Otin", StateId = 29 },
                new Lga { Id = 618, Name = "Ola Oluwa", StateId = 29 },
                new Lga { Id = 619, Name = "Olorunda", StateId = 29 },
                new Lga { Id = 620, Name = "Oriade", StateId = 29 },
                new Lga { Id = 621, Name = "Orolu", StateId = 29 },
                new Lga { Id = 622, Name = "Osogbo", StateId = 29 }
            );


            builder.HasData(
                new Lga { Id = 623, Name = "Afijio", StateId = 30 },
                new Lga { Id = 624, Name = "Akinyele", StateId = 30 },
                new Lga { Id = 625, Name = "Atiba", StateId = 30 },
                new Lga { Id = 626, Name = "Atisbo", StateId = 30 },
                new Lga { Id = 627, Name = "Egbeda", StateId = 30 },
                new Lga { Id = 628, Name = "Ibadan North", StateId = 30 },
                new Lga { Id = 629, Name = "Ibadan North-East", StateId = 30 },
                new Lga { Id = 630, Name = "Ibadan North-West", StateId = 30 },
                new Lga { Id = 631, Name = "Ibadan South-East", StateId = 30 },
                new Lga { Id = 632, Name = "Ibadan South-West", StateId = 30 },
                new Lga { Id = 633, Name = "Ibarapa Central", StateId = 30 },
                new Lga { Id = 634, Name = "Ibarapa East", StateId = 30 },
                new Lga { Id = 635, Name = "Ibarapa North", StateId = 30 },
                new Lga { Id = 636, Name = "Ido", StateId = 30 },
                new Lga { Id = 637, Name = "Irepo", StateId = 30 },
                new Lga { Id = 638, Name = "Iseyin", StateId = 30 },
                new Lga { Id = 639, Name = "Itesiwaju", StateId = 30 },
                new Lga { Id = 640, Name = "Iwajowa", StateId = 30 },
                new Lga { Id = 641, Name = "Kajola", StateId = 30 },
                new Lga { Id = 642, Name = "Lagelu", StateId = 30 },
                new Lga { Id = 643, Name = "Ogbomosho North", StateId = 30 },
                new Lga { Id = 644, Name = "Ogbomosho South", StateId = 30 },
                new Lga { Id = 645, Name = "Ogo Oluwa", StateId = 30 },
                new Lga { Id = 646, Name = "Olorunsogo", StateId = 30 },
                new Lga { Id = 647, Name = "Oluyole", StateId = 30 },
                new Lga { Id = 648, Name = "Ona Ara", StateId = 30 },
                new Lga { Id = 649, Name = "Orelope", StateId = 30 },
                new Lga { Id = 650, Name = "Ori Ire", StateId = 30 },
                new Lga { Id = 651, Name = "Oyo East", StateId = 30 },
                new Lga { Id = 652, Name = "Oyo West", StateId = 30 },
                new Lga { Id = 653, Name = "Saki East", StateId = 30 },
                new Lga { Id = 654, Name = "Saki West", StateId = 30 },
                new Lga { Id = 655, Name = "Surulere", StateId = 30 }
            );


            builder.HasData(
               new Lga { Id = 656, Name = "Bokkos", StateId = 31 },
               new Lga { Id = 657, Name = "Barkin Ladi", StateId = 31 },
               new Lga { Id = 658, Name = "Bassa", StateId = 31 },
               new Lga { Id = 659, Name = "Jos East", StateId = 31 },
               new Lga { Id = 660, Name = "Jos North", StateId = 31 },
               new Lga { Id = 661, Name = "Jos South", StateId = 31 },
               new Lga { Id = 662, Name = "Kanam", StateId = 31 },
               new Lga { Id = 663, Name = "Kanke", StateId = 31 },
               new Lga { Id = 664, Name = "Langtang North", StateId = 31 },
               new Lga { Id = 665, Name = "Langtang South", StateId = 31 },
               new Lga { Id = 666, Name = "Mangu", StateId = 31 },
               new Lga { Id = 667, Name = "Mikang", StateId = 31 },
               new Lga { Id = 668, Name = "Pankshin", StateId = 31 },
               new Lga { Id = 669, Name = "Qua’an Pan", StateId = 31 },
               new Lga { Id = 670, Name = "Riyom", StateId = 31 },
               new Lga { Id = 671, Name = "Shendam", StateId = 31 },
               new Lga { Id = 672, Name = "Wase", StateId = 31 }
           );


            builder.HasData(
                new Lga { Id = 673, Name = "Abua/Odual", StateId = 32 },
                new Lga { Id = 674, Name = "Ahoada East", StateId = 32 },
                new Lga { Id = 675, Name = "Ahoada West", StateId = 32 },
                new Lga { Id = 676, Name = "Akuku-Toru", StateId = 32 },
                new Lga { Id = 677, Name = "Andoni", StateId = 32 },
                new Lga { Id = 678, Name = "Asari-Toru", StateId = 32 },
                new Lga { Id = 679, Name = "Bonny", StateId = 32 },
                new Lga { Id = 680, Name = "Degema", StateId = 32 },
                new Lga { Id = 681, Name = "Eleme", StateId = 32 },
                new Lga { Id = 682, Name = "Emohua", StateId = 32 },
                new Lga { Id = 683, Name = "Etche", StateId = 32 },
                new Lga { Id = 684, Name = "Gokana", StateId = 32 },
                new Lga { Id = 685, Name = "Ikwerre", StateId = 32 },
                new Lga { Id = 686, Name = "Khana", StateId = 32 },
                new Lga { Id = 687, Name = "Obio/Akpor", StateId = 32 },
                new Lga { Id = 688, Name = "Ogba/Egbema/Ndoni", StateId = 32 },
                new Lga { Id = 689, Name = "Ogu/Bolo", StateId = 32 },
                new Lga { Id = 690, Name = "Okrika", StateId = 32 },
                new Lga { Id = 691, Name = "Omuma", StateId = 32 },
                new Lga { Id = 692, Name = "Opobo/Nkoro", StateId = 32 },
                new Lga { Id = 693, Name = "Oyigbo", StateId = 32 },
                new Lga { Id = 694, Name = "Port Harcourt", StateId = 32 },
                new Lga { Id = 695, Name = "Tai", StateId = 32 }
            );

            builder.HasData(
                new Lga { Id = 696, Name = "Binji", StateId = 33 },
                new Lga { Id = 697, Name = "Bodinga", StateId = 33 },
                new Lga { Id = 698, Name = "Dange Shuni", StateId = 33 },
                new Lga { Id = 699, Name = "Gada", StateId = 33 },
                new Lga { Id = 700, Name = "Goronyo", StateId = 33 },
                new Lga { Id = 701, Name = "Gudu", StateId = 33 },
                new Lga { Id = 702, Name = "Gwadabawa", StateId = 33 },
                new Lga { Id = 703, Name = "Illela", StateId = 33 },
                new Lga { Id = 704, Name = "Isa", StateId = 33 },
                new Lga { Id = 705, Name = "Kebbe", StateId = 33 },
                new Lga { Id = 706, Name = "Kware", StateId = 33 },
                new Lga { Id = 707, Name = "Rabah", StateId = 33 },
                new Lga { Id = 708, Name = "Sabon Birni", StateId = 33 },
                new Lga { Id = 709, Name = "Shagari", StateId = 33 },
                new Lga { Id = 710, Name = "Sokoto North", StateId = 33 },
                new Lga { Id = 711, Name = "Sokoto South", StateId = 33 },
                new Lga { Id = 712, Name = "Tambuwal", StateId = 33 },
                new Lga { Id = 713, Name = "Tangaza", StateId = 33 },
                new Lga { Id = 714, Name = "Tureta", StateId = 33 },
                new Lga { Id = 715, Name = "Wamako", StateId = 33 },
                new Lga { Id = 716, Name = "Wurno", StateId = 33 },
                new Lga { Id = 717, Name = "Yabo", StateId = 33 }
            );

            builder.HasData(
                new Lga { Id = 718, Name = "Ardo Kola", StateId = 34 },
                new Lga { Id = 719, Name = "Bali", StateId = 34 },
                new Lga { Id = 720, Name = "Donga", StateId = 34 },
                new Lga { Id = 721, Name = "Gashaka", StateId = 34 },
                new Lga { Id = 722, Name = "Gassol", StateId = 34 },
                new Lga { Id = 723, Name = "Ibi", StateId = 34 },
                new Lga { Id = 724, Name = "Jalingo", StateId = 34 },
                new Lga { Id = 725, Name = "Karim Lamido", StateId = 34 },
                new Lga { Id = 726, Name = "Kumi", StateId = 34 },
                new Lga { Id = 727, Name = "Lau", StateId = 34 },
                new Lga { Id = 728, Name = "Sardauna", StateId = 34 },
                new Lga { Id = 729, Name = "Takum", StateId = 34 },
                new Lga { Id = 730, Name = "Ussa", StateId = 34 },
                new Lga { Id = 731, Name = "Wukari", StateId = 34 },
                new Lga { Id = 732, Name = "Yorro", StateId = 34 },
                new Lga { Id = 733, Name = "Zing", StateId = 34 }
            );

            builder.HasData(
            new Lga { Id = 734, Name = "Bade", StateId = 35 },
            new Lga { Id = 735, Name = "Bursari", StateId = 35 },
            new Lga { Id = 736, Name = "Damaturu", StateId = 35 },
            new Lga { Id = 737, Name = "Fika", StateId = 35 },
            new Lga { Id = 738, Name = "Fune", StateId = 35 },
            new Lga { Id = 739, Name = "Geidam", StateId = 35 },
            new Lga { Id = 740, Name = "Gujba", StateId = 35 },
            new Lga { Id = 741, Name = "Gulani", StateId = 35 },
            new Lga { Id = 742, Name = "Jakusko", StateId = 35 },
            new Lga { Id = 743, Name = "Karasuwa", StateId = 35 },
            new Lga { Id = 744, Name = "Machina", StateId = 35 },
            new Lga { Id = 745, Name = "Nangere", StateId = 35 },
            new Lga { Id = 746, Name = "Nguru", StateId = 35 },
            new Lga { Id = 747, Name = "Potiskum", StateId = 35 },
            new Lga { Id = 748, Name = "Tarmuwa", StateId = 35 },
            new Lga { Id = 749, Name = "Yunusari", StateId = 35 },
            new Lga { Id = 750, Name = "Yusufari", StateId = 35 }
        );

            builder.HasData(
            new Lga { Id = 751, Name = "Anka", StateId = 36 },
            new Lga { Id = 752, Name = "Bakura", StateId = 36 },
            new Lga { Id = 753, Name = "Bungudu", StateId = 36 },
            new Lga { Id = 754, Name = "Gummi", StateId = 36 },
            new Lga { Id = 755, Name = "Maradun", StateId = 36 },
            new Lga { Id = 756, Name = "Maru", StateId = 36 },
            new Lga { Id = 757, Name = "Shinkafi", StateId = 36 },
            new Lga { Id = 758, Name = "Talata Mafara", StateId = 36 },
            new Lga { Id = 759, Name = "Chafe", StateId = 36 },
            new Lga { Id = 760, Name = "Zurmi", StateId = 36 },
            new Lga { Id = 761, Name = "Kaura Namoda", StateId = 36 },
            new Lga { Id = 762, Name = "Gusau", StateId = 36 },
            new Lga { Id = 763, Name = "Bukkuyum", StateId = 36 },
            new Lga { Id = 764, Name = "Talata Mafara", StateId = 36 },
            new Lga { Id = 771, Name = "Tsafe", StateId = 36 }
        );

            builder.HasData(
                new Lga { Id = 765, Name = "Abaji", StateId = 37 },
                new Lga { Id = 766, Name = "Bwari", StateId = 37 },
                new Lga { Id = 767, Name = "Gwagwalada", StateId = 37 },
                new Lga { Id = 768, Name = "Kuje", StateId = 37 },
                new Lga { Id = 769, Name = "Kwali", StateId = 37 },
                new Lga { Id = 770, Name = "Municipal Area Council", StateId = 37 },
                new Lga { Id = 772, Name = "Ohimini", StateId = 7 },
                new Lga { Id = 773, Name = "Dutsin-Ma", StateId = 20 }
            );

        }
    }

}
