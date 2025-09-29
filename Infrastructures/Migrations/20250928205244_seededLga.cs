using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class seededLga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lgas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Lgas",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[,]
                {
                    { 1, "Aba North", 1 },
                    { 2, "Aba South", 1 },
                    { 3, "Arochukwu", 1 },
                    { 4, "Bende", 1 },
                    { 5, "Ikwuano", 1 },
                    { 6, "Isiala Ngwa North", 1 },
                    { 7, "Isiala Ngwa South", 1 },
                    { 8, "Isuikwuato", 1 },
                    { 9, "Obi Ngwa", 1 },
                    { 10, "Ohafia", 1 },
                    { 11, "Osisioma Ngwa", 1 },
                    { 12, "Ugwunagbo", 1 },
                    { 13, "Ukwa East", 1 },
                    { 14, "Ukwa West", 1 },
                    { 15, "Umuahia North", 1 },
                    { 16, "Umuahia South", 1 },
                    { 17, "Umu Nneochi", 1 },
                    { 18, "Demsa", 2 },
                    { 19, "Fufore", 2 },
                    { 20, "Ganye", 2 },
                    { 21, "Girei", 2 },
                    { 22, "Gombi", 2 },
                    { 23, "Guyuk", 2 },
                    { 24, "Hong", 2 },
                    { 25, "Jada", 2 },
                    { 26, "Lamurde", 2 },
                    { 27, "Madagali", 2 },
                    { 28, "Maiha", 2 },
                    { 29, "Mayo-Belwa", 2 },
                    { 30, "Michika", 2 },
                    { 31, "Mubi North", 2 },
                    { 32, "Mubi South", 2 },
                    { 33, "Numan", 2 },
                    { 34, "Shelleng", 2 },
                    { 35, "Song", 2 },
                    { 36, "Toungo", 2 },
                    { 37, "Yola North", 2 },
                    { 38, "Yola South", 2 },
                    { 39, "Abak", 3 },
                    { 40, "Eastern Obolo", 3 },
                    { 41, "Eket", 3 },
                    { 42, "Esit Eket", 3 },
                    { 43, "Essien Udim", 3 },
                    { 44, "Etim Ekpo", 3 },
                    { 45, "Etinan", 3 },
                    { 46, "Ibeno", 3 },
                    { 47, "Ibesikpo Asutan", 3 },
                    { 48, "Ibiono Ibom", 3 },
                    { 49, "Ika", 3 },
                    { 50, "Ikono", 3 },
                    { 51, "Ikot Abasi", 3 },
                    { 52, "Ikot Ekpene", 3 },
                    { 53, "Ini", 3 },
                    { 54, "Itu", 3 },
                    { 55, "Mbo", 3 },
                    { 56, "Mkpat Enin", 3 },
                    { 57, "Nsit Atai", 3 },
                    { 58, "Nsit Ibom", 3 },
                    { 59, "Nsit Ubium", 3 },
                    { 60, "Obot Akara", 3 },
                    { 61, "Okobo", 3 },
                    { 62, "Onna", 3 },
                    { 63, "Oron", 3 },
                    { 64, "Oruk Anam", 3 },
                    { 65, "Udung Uko", 3 },
                    { 66, "Ukanafun", 3 },
                    { 67, "Uruan", 3 },
                    { 68, "Urue-Offong/Oruko", 3 },
                    { 69, "Uyo", 3 },
                    { 70, "Aguata", 4 },
                    { 71, "Anambra East", 4 },
                    { 72, "Anambra West", 4 },
                    { 73, "Anaocha", 4 },
                    { 74, "Awka North", 4 },
                    { 75, "Awka South", 4 },
                    { 76, "Ayamelum", 4 },
                    { 77, "Dunukofia", 4 },
                    { 78, "Ekwusigo", 4 },
                    { 79, "Idemili North", 4 },
                    { 80, "Idemili South", 4 },
                    { 81, "Ihiala", 4 },
                    { 82, "Njikoka", 4 },
                    { 83, "Nnewi North", 4 },
                    { 84, "Nnewi South", 4 },
                    { 85, "Ogbaru", 4 },
                    { 86, "Onitsha North", 4 },
                    { 87, "Onitsha South", 4 },
                    { 88, "Orumba North", 4 },
                    { 89, "Orumba South", 4 },
                    { 90, "Oyi", 4 },
                    { 91, "Alkaleri", 5 },
                    { 92, "Bauchi", 5 },
                    { 93, "Bogoro", 5 },
                    { 94, "Damban", 5 },
                    { 95, "Darazo", 5 },
                    { 96, "Dass", 5 },
                    { 97, "Gamawa", 5 },
                    { 98, "Ganjuwa", 5 },
                    { 99, "Giade", 5 },
                    { 100, "Itas/Gadau", 5 },
                    { 101, "Jama'are", 5 },
                    { 102, "Katagum", 5 },
                    { 103, "Kirfi", 5 },
                    { 104, "Misau", 5 },
                    { 105, "Ningi", 5 },
                    { 106, "Shira", 5 },
                    { 107, "Tafawa Balewa", 5 },
                    { 108, "Toro", 5 },
                    { 109, "Warji", 5 },
                    { 110, "Zaki", 5 },
                    { 111, "Brass", 6 },
                    { 112, "Ekeremor", 6 },
                    { 113, "Kolokuma/Opokuma", 6 },
                    { 114, "Nembe", 6 },
                    { 115, "Ogbia", 6 },
                    { 116, "Sagbama", 6 },
                    { 117, "Southern Ijaw", 6 },
                    { 118, "Yenagoa", 6 },
                    { 119, "Ado", 7 },
                    { 120, "Agatu", 7 },
                    { 121, "Apa", 7 },
                    { 122, "Buruku", 7 },
                    { 123, "Gboko", 7 },
                    { 124, "Guma", 7 },
                    { 125, "Gwer East", 7 },
                    { 126, "Gwer West", 7 },
                    { 127, "Katsina-Ala", 7 },
                    { 128, "Konshisha", 7 },
                    { 129, "Kwande", 7 },
                    { 130, "Logo", 7 },
                    { 131, "Makurdi", 7 },
                    { 132, "Obi", 7 },
                    { 133, "Ogbadibo", 7 },
                    { 134, "Oju", 7 },
                    { 135, "Okpokwu", 7 },
                    { 136, "Otukpo", 7 },
                    { 137, "Tarka", 7 },
                    { 138, "Ukum", 7 },
                    { 139, "Ushongo", 7 },
                    { 140, "Vandeikya", 7 },
                    { 141, "Abadam", 8 },
                    { 142, "Askira/Uba", 8 },
                    { 143, "Bama", 8 },
                    { 144, "Bayo", 8 },
                    { 145, "Biu", 8 },
                    { 146, "Chibok", 8 },
                    { 147, "Damboa", 8 },
                    { 148, "Dikwa", 8 },
                    { 149, "Gubio", 8 },
                    { 150, "Guzamala", 8 },
                    { 151, "Gwoza", 8 },
                    { 152, "Hawul", 8 },
                    { 153, "Jere", 8 },
                    { 154, "Kaga", 8 },
                    { 155, "Kala/Balge", 8 },
                    { 156, "Konduga", 8 },
                    { 157, "Kukawa", 8 },
                    { 158, "Kwaya Kusar", 8 },
                    { 159, "Mafa", 8 },
                    { 160, "Magumeri", 8 },
                    { 161, "Maiduguri", 8 },
                    { 162, "Marte", 8 },
                    { 163, "Mobbar", 8 },
                    { 164, "Monguno", 8 },
                    { 165, "Ngala", 8 },
                    { 166, "Nganzai", 8 },
                    { 167, "Shani", 8 },
                    { 168, "Akpabuyo", 9 },
                    { 169, "Bakassi", 9 },
                    { 170, "Bekwara", 9 },
                    { 171, "Biase", 9 },
                    { 172, "Boki", 9 },
                    { 173, "Calabar Municipal", 9 },
                    { 174, "Calabar South", 9 },
                    { 175, "Etung", 9 },
                    { 176, "Ikom", 9 },
                    { 177, "Obanliku", 9 },
                    { 178, "Obubra", 9 },
                    { 179, "Obudu", 9 },
                    { 180, "Odukpani", 9 },
                    { 181, "Ogoja", 9 },
                    { 182, "Yakuur", 9 },
                    { 183, "Yala", 9 },
                    { 184, "Akampka", 9 },
                    { 185, "Obudu (Obanliku)", 9 },
                    { 186, "Aniocha North", 10 },
                    { 187, "Aniocha South", 10 },
                    { 188, "Bomadi", 10 },
                    { 189, "Burutu", 10 },
                    { 190, "Ethiope East", 10 },
                    { 191, "Ethiope West", 10 },
                    { 192, "Ika North East", 10 },
                    { 193, "Ika South", 10 },
                    { 194, "Isoko North", 10 },
                    { 195, "Isoko South", 10 },
                    { 196, "Ndokwa East", 10 },
                    { 197, "Ndokwa West", 10 },
                    { 198, "Okpe", 10 },
                    { 199, "Oshimili North", 10 },
                    { 200, "Oshimili South", 10 },
                    { 201, "Patani", 10 },
                    { 202, "Sapele", 10 },
                    { 203, "Udu", 10 },
                    { 204, "Ughelli North", 10 },
                    { 205, "Ughelli South", 10 },
                    { 206, "Ukwuani", 10 },
                    { 207, "Uvwie", 10 },
                    { 208, "Warri North", 10 },
                    { 209, "Warri South", 10 },
                    { 210, "Warri South West", 10 },
                    { 211, "Abakaliki", 11 },
                    { 212, "Afikpo North", 11 },
                    { 213, "Afikpo South (Edda)", 11 },
                    { 214, "Ebonyi", 11 },
                    { 215, "Ezza North", 11 },
                    { 216, "Ezza South", 11 },
                    { 217, "Ikwo", 11 },
                    { 218, "Ishielu", 11 },
                    { 219, "Ivo", 11 },
                    { 220, "Izzi", 11 },
                    { 221, "Ohaozara", 11 },
                    { 222, "Ohaukwu", 11 },
                    { 223, "Onicha", 11 },
                    { 224, "Akoko-Edo", 12 },
                    { 225, "Egor", 12 },
                    { 226, "Esan Central", 12 },
                    { 227, "Esan North-East", 12 },
                    { 228, "Esan South-East", 12 },
                    { 229, "Esan West", 12 },
                    { 230, "Etsako Central", 12 },
                    { 231, "Etsako East", 12 },
                    { 232, "Etsako West", 12 },
                    { 233, "Igueben", 12 },
                    { 234, "Ikpoba-Okha", 12 },
                    { 235, "Oredo", 12 },
                    { 236, "Orhionmwon", 12 },
                    { 237, "Ovia North-East", 12 },
                    { 238, "Ovia South-West", 12 },
                    { 239, "Owan East", 12 },
                    { 240, "Owan West", 12 },
                    { 241, "Uhunmwonde", 12 },
                    { 242, "Ado Ekiti", 13 },
                    { 243, "Efon", 13 },
                    { 244, "Ekiti East", 13 },
                    { 245, "Ekiti South-West", 13 },
                    { 246, "Ekiti West", 13 },
                    { 247, "Emure", 13 },
                    { 248, "Gbonyin", 13 },
                    { 249, "Ido-Osi", 13 },
                    { 250, "Ijero", 13 },
                    { 251, "Ikere", 13 },
                    { 252, "Ikole", 13 },
                    { 253, "Ilejemeje", 13 },
                    { 254, "Irepodun/Ifelodun", 13 },
                    { 255, "Ise/Orun", 13 },
                    { 256, "Moba", 13 },
                    { 257, "Oye", 13 },
                    { 258, "Aninri", 14 },
                    { 259, "Awgu", 14 },
                    { 260, "Enugu East", 14 },
                    { 261, "Enugu North", 14 },
                    { 262, "Enugu South", 14 },
                    { 263, "Ezeagu", 14 },
                    { 264, "Igbo Etiti", 14 },
                    { 265, "Igbo Eze North", 14 },
                    { 266, "Igbo Eze South", 14 },
                    { 267, "Isi Uzo", 14 },
                    { 268, "Nkanu East", 14 },
                    { 269, "Nkanu West", 14 },
                    { 270, "Nsukka", 14 },
                    { 271, "Oji River", 14 },
                    { 272, "Udenu", 14 },
                    { 273, "Udi", 14 },
                    { 274, "Uzo-Uwani", 14 },
                    { 275, "Akko", 15 },
                    { 276, "Balanga", 15 },
                    { 277, "Billiri", 15 },
                    { 278, "Dukku", 15 },
                    { 279, "Funakaye", 15 },
                    { 280, "Gombe", 15 },
                    { 281, "Kaltungo", 15 },
                    { 282, "Kwami", 15 },
                    { 283, "Nafada/Bajoga", 15 },
                    { 284, "Yamaltu/Deba", 15 },
                    { 285, "Gombe (city)", 15 },
                    { 286, "Aboh Mbaise", 16 },
                    { 287, "Ahiazu Mbaise", 16 },
                    { 288, "Ehime Mbano", 16 },
                    { 289, "Ezinihitte", 16 },
                    { 290, "Ideato North", 16 },
                    { 291, "Ideato South", 16 },
                    { 292, "Ihitte/Uboma", 16 },
                    { 293, "Ikeduru", 16 },
                    { 294, "Isiala Mbano", 16 },
                    { 295, "Isu", 16 },
                    { 296, "Mbaitoli", 16 },
                    { 297, "Ngor Okpala", 16 },
                    { 298, "Nkwerre", 16 },
                    { 299, "Nwangele", 16 },
                    { 300, "Obowo", 16 },
                    { 301, "Oguta", 16 },
                    { 302, "Ohaji/Egbema", 16 },
                    { 303, "Okigwe", 16 },
                    { 304, "Onuimo", 16 },
                    { 305, "Orlu", 16 },
                    { 306, "Orsu", 16 },
                    { 307, "Oru East", 16 },
                    { 308, "Oru West", 16 },
                    { 309, "Owerri Municipal", 16 },
                    { 310, "Owerri North", 16 },
                    { 311, "Owerri West", 16 },
                    { 312, "Ehime Mbano", 16 },
                    { 313, "Auyo", 17 },
                    { 314, "Babura", 17 },
                    { 315, "Biriniwa", 17 },
                    { 316, "Birnin Kudu", 17 },
                    { 317, "Buji", 17 },
                    { 318, "Dutse", 17 },
                    { 319, "Gagarawa", 17 },
                    { 320, "Garki", 17 },
                    { 321, "Gumel", 17 },
                    { 322, "Guri", 17 },
                    { 323, "Gwaram", 17 },
                    { 324, "Gwiwa", 17 },
                    { 325, "Hadejia", 17 },
                    { 326, "Jahun", 17 },
                    { 327, "Kafin Hausa", 17 },
                    { 328, "Kaugama", 17 },
                    { 329, "Kazaure", 17 },
                    { 330, "Kiri Kasama", 17 },
                    { 331, "Kiyawa", 17 },
                    { 332, "Maigatari", 17 },
                    { 333, "Malam Madori", 17 },
                    { 334, "Miga", 17 },
                    { 335, "Ringim", 17 },
                    { 336, "Roni", 17 },
                    { 337, "Sule Tankarkar", 17 },
                    { 338, "Taura", 17 },
                    { 339, "Yankwashi", 17 },
                    { 340, "Birnin Gwari", 18 },
                    { 341, "Chikun", 18 },
                    { 342, "Giwa", 18 },
                    { 343, "Igabi", 18 },
                    { 344, "Ikara", 18 },
                    { 345, "Jaba", 18 },
                    { 346, "Jema'a", 18 },
                    { 347, "Kachia", 18 },
                    { 348, "Kaduna North", 18 },
                    { 349, "Kaduna South", 18 },
                    { 350, "Kagarko", 18 },
                    { 351, "Kajuru", 18 },
                    { 352, "Kaura", 18 },
                    { 353, "Kauru", 18 },
                    { 354, "Kubau", 18 },
                    { 355, "Kudan", 18 },
                    { 356, "Lere", 18 },
                    { 357, "Makarfi", 18 },
                    { 358, "Sabon Gari", 18 },
                    { 359, "Sanga", 18 },
                    { 360, "Soba", 18 },
                    { 361, "Zangon Kataf", 18 },
                    { 362, "Zaria", 18 },
                    { 363, "Ajingi", 19 },
                    { 364, "Albasu", 19 },
                    { 365, "Bagwai", 19 },
                    { 366, "Bebeji", 19 },
                    { 367, "Bichi", 19 },
                    { 368, "Bunkure", 19 },
                    { 369, "Dala", 19 },
                    { 370, "Dambatta", 19 },
                    { 371, "Dawakin Kudu", 19 },
                    { 372, "Dawakin Tofa", 19 },
                    { 373, "Doguwa", 19 },
                    { 374, "Fagge", 19 },
                    { 375, "Gabasawa", 19 },
                    { 376, "Garko", 19 },
                    { 377, "Garun Mallam", 19 },
                    { 378, "Gaya", 19 },
                    { 379, "Gezawa", 19 },
                    { 380, "Gwale", 19 },
                    { 381, "Gwarzo", 19 },
                    { 382, "Kabo", 19 },
                    { 383, "Kano Municipal", 19 },
                    { 384, "Karaye", 19 },
                    { 385, "Kibiya", 19 },
                    { 386, "Kiru", 19 },
                    { 387, "Kumbotso", 19 },
                    { 388, "Kunchi", 19 },
                    { 389, "Kura", 19 },
                    { 390, "Madobi", 19 },
                    { 391, "Makoda", 19 },
                    { 392, "Minjibir", 19 },
                    { 393, "Nasarawa", 19 },
                    { 394, "Rano", 19 },
                    { 395, "Rimin Gado", 19 },
                    { 396, "Rogo", 19 },
                    { 397, "Shanono", 19 },
                    { 398, "Sumaila", 19 },
                    { 399, "Tarauni", 19 },
                    { 400, "Tofa", 19 },
                    { 401, "Tsanyawa", 19 },
                    { 402, "Tudun Wada", 19 },
                    { 403, "Ungogo", 19 },
                    { 404, "Warawa", 19 },
                    { 405, "Wudil", 19 },
                    { 406, "Bakori", 20 },
                    { 407, "Batagarawa", 20 },
                    { 408, "Batsari", 20 },
                    { 409, "Baure", 20 },
                    { 410, "Bindawa", 20 },
                    { 411, "Charanchi", 20 },
                    { 412, "Dandume", 20 },
                    { 413, "Danja", 20 },
                    { 414, "Dutse", 20 },
                    { 415, "Faskari", 20 },
                    { 416, "Funtua", 20 },
                    { 417, "Ingawa", 20 },
                    { 418, "Jibia", 20 },
                    { 419, "Kafur", 20 },
                    { 420, "Kaita", 20 },
                    { 421, "Kankara", 20 },
                    { 422, "Kankia", 20 },
                    { 423, "Katsina", 20 },
                    { 424, "Kurfi", 20 },
                    { 425, "Kusada", 20 },
                    { 426, "Mai'Adua", 20 },
                    { 427, "Malumfashi", 20 },
                    { 428, "Mani", 20 },
                    { 429, "Mashi", 20 },
                    { 430, "Matazu", 20 },
                    { 431, "Musawa", 20 },
                    { 432, "Rimi", 20 },
                    { 433, "Sabuwa", 20 },
                    { 434, "Safana", 20 },
                    { 435, "Sandamu", 20 },
                    { 436, "Zango", 20 },
                    { 437, "Danja (city)", 20 },
                    { 438, "Funtua (city)", 20 },
                    { 439, "Aleiro", 21 },
                    { 440, "Arewa Dandi", 21 },
                    { 441, "Argungu", 21 },
                    { 442, "Augie", 21 },
                    { 443, "Bagudo", 21 },
                    { 444, "Birnin Kebbi", 21 },
                    { 445, "Bunza", 21 },
                    { 446, "Dandi", 21 },
                    { 447, "Fakai", 21 },
                    { 448, "Gwandu", 21 },
                    { 449, "Jega", 21 },
                    { 450, "Kalgo", 21 },
                    { 451, "Koko Besse", 21 },
                    { 452, "Maiyama", 21 },
                    { 453, "Ngaski", 21 },
                    { 454, "Sakaba", 21 },
                    { 455, "Shanga", 21 },
                    { 456, "Suru", 21 },
                    { 457, "Wasagu/Danko", 21 },
                    { 458, "Yauri", 21 },
                    { 459, "Zuru", 21 },
                    { 460, "Adavi", 22 },
                    { 461, "Ajaokuta", 22 },
                    { 462, "Ankpa", 22 },
                    { 463, "Bassa", 22 },
                    { 464, "Dekina", 22 },
                    { 465, "Ibaji", 22 },
                    { 466, "Idah", 22 },
                    { 467, "Igalamela-Odolu", 22 },
                    { 468, "Ijumu", 22 },
                    { 469, "Kabba/Bunu", 22 },
                    { 470, "Kogi", 22 },
                    { 471, "Lokoja", 22 },
                    { 472, "Mopa-Muro", 22 },
                    { 473, "Ofu", 22 },
                    { 474, "Ogori/Magongo", 22 },
                    { 475, "Okehi", 22 },
                    { 476, "Okene", 22 },
                    { 477, "Olamaboro", 22 },
                    { 478, "Omala", 22 },
                    { 479, "Yagba East", 22 },
                    { 480, "Yagba West", 22 },
                    { 481, "Asa", 23 },
                    { 482, "Baruten", 23 },
                    { 483, "Edu", 23 },
                    { 484, "Ekiti", 23 },
                    { 485, "Ifelodun", 23 },
                    { 486, "Ilorin East", 23 },
                    { 487, "Ilorin South", 23 },
                    { 488, "Ilorin West", 23 },
                    { 489, "Irepodun", 23 },
                    { 490, "Isin", 23 },
                    { 491, "Kaiama", 23 },
                    { 492, "Moro", 23 },
                    { 493, "Offa", 23 },
                    { 494, "Oke Ero", 23 },
                    { 495, "Oyun", 23 },
                    { 496, "Patigi", 23 },
                    { 497, "Agege", 24 },
                    { 498, "Ajeromi-Ifelodun", 24 },
                    { 499, "Alimosho", 24 },
                    { 500, "Amuwo-Odofin", 24 },
                    { 501, "Apapa", 24 },
                    { 502, "Badagry", 24 },
                    { 503, "Epe", 24 },
                    { 504, "Eti-Osa", 24 },
                    { 505, "Ibeju-Lekki", 24 },
                    { 506, "Ifako-Ijaiye", 24 },
                    { 507, "Ikeja", 24 },
                    { 508, "Ikorodu", 24 },
                    { 509, "Kosofe", 24 },
                    { 510, "Lagos Island", 24 },
                    { 511, "Lagos Mainland", 24 },
                    { 512, "Mushin", 24 },
                    { 513, "Ojo", 24 },
                    { 514, "Oshodi-Isolo", 24 },
                    { 515, "Shomolu", 24 },
                    { 516, "Surulere", 24 },
                    { 517, "Akwanga", 25 },
                    { 518, "Awe", 25 },
                    { 519, "Doma", 25 },
                    { 520, "Karu", 25 },
                    { 521, "Keana", 25 },
                    { 522, "Keffi", 25 },
                    { 523, "Kokona", 25 },
                    { 524, "Lafia", 25 },
                    { 525, "Nasarawa", 25 },
                    { 526, "Nasarawa Egon", 25 },
                    { 527, "Obi", 25 },
                    { 528, "Toto", 25 },
                    { 529, "Wamba", 25 },
                    { 530, "Agaie", 26 },
                    { 531, "Agwara", 26 },
                    { 532, "Bida", 26 },
                    { 533, "Borgu", 26 },
                    { 534, "Bosso", 26 },
                    { 535, "Chanchaga", 26 },
                    { 536, "Edati", 26 },
                    { 537, "Gbako", 26 },
                    { 538, "Gurara", 26 },
                    { 539, "Katcha", 26 },
                    { 540, "Kontagora", 26 },
                    { 541, "Lapai", 26 },
                    { 542, "Lavun", 26 },
                    { 543, "Mokwa", 26 },
                    { 544, "Muya", 26 },
                    { 545, "Pailoro", 26 },
                    { 546, "Rafi", 26 },
                    { 547, "Rijau", 26 },
                    { 548, "Shiroro", 26 },
                    { 549, "Suleja", 26 },
                    { 550, "Tafa", 26 },
                    { 551, "Wushishi", 26 },
                    { 552, "Mariga", 26 },
                    { 553, "Mashegu", 26 },
                    { 554, "Bosso", 26 },
                    { 555, "Abeokuta North", 27 },
                    { 556, "Abeokuta South", 27 },
                    { 557, "Ado-Odo/Ota", 27 },
                    { 558, "Egbado North (Yewa North)", 27 },
                    { 559, "Egbado South (Yewa South)", 27 },
                    { 560, "Ewekoro", 27 },
                    { 561, "Ifo", 27 },
                    { 562, "Ijebu East", 27 },
                    { 563, "Ijebu North", 27 },
                    { 564, "Ijebu North East", 27 },
                    { 565, "Ijebu Ode", 27 },
                    { 566, "Ikenne", 27 },
                    { 567, "Imeko Afon", 27 },
                    { 568, "Ipokia", 27 },
                    { 569, "Obafemi Owode", 27 },
                    { 570, "Odogbolu", 27 },
                    { 571, "Ogun Waterside", 27 },
                    { 572, "Remo North", 27 },
                    { 573, "Shagamu", 27 },
                    { 574, "Yewa North", 27 },
                    { 575, "Akoko North-East", 28 },
                    { 576, "Akoko North-West", 28 },
                    { 577, "Akoko South-East", 28 },
                    { 578, "Akoko South-West", 28 },
                    { 579, "Akure North", 28 },
                    { 580, "Akure South", 28 },
                    { 581, "Ese-Odo", 28 },
                    { 582, "Idanre", 28 },
                    { 583, "Ifedore", 28 },
                    { 584, "Ilaje", 28 },
                    { 585, "Ile-Oluji/Okeigbo", 28 },
                    { 586, "Irele", 28 },
                    { 587, "Odigbo", 28 },
                    { 588, "Okitipupa", 28 },
                    { 589, "Ondo East", 28 },
                    { 590, "Ondo West", 28 },
                    { 591, "Ose", 28 },
                    { 592, "Owo", 28 },
                    { 593, "Atakunmosa East", 29 },
                    { 594, "Atakunmosa West", 29 },
                    { 595, "Aiyedaade", 29 },
                    { 596, "Aiyedire", 29 },
                    { 597, "Boluwaduro", 29 },
                    { 598, "Boripe", 29 },
                    { 599, "Ede North", 29 },
                    { 600, "Ede South", 29 },
                    { 601, "Egbedore", 29 },
                    { 602, "Ejigbo", 29 },
                    { 603, "Ife Central", 29 },
                    { 604, "Ife East", 29 },
                    { 605, "Ife North", 29 },
                    { 606, "Ife South", 29 },
                    { 607, "Ifedayo", 29 },
                    { 608, "Ifelodun", 29 },
                    { 609, "Ila", 29 },
                    { 610, "Ilesa East", 29 },
                    { 611, "Ilesa West", 29 },
                    { 612, "Irepodun", 29 },
                    { 613, "Irewole", 29 },
                    { 614, "Isokan", 29 },
                    { 615, "Iwo", 29 },
                    { 616, "Obokun", 29 },
                    { 617, "Odo Otin", 29 },
                    { 618, "Ola Oluwa", 29 },
                    { 619, "Olorunda", 29 },
                    { 620, "Oriade", 29 },
                    { 621, "Orolu", 29 },
                    { 622, "Osogbo", 29 },
                    { 623, "Afijio", 30 },
                    { 624, "Akinyele", 30 },
                    { 625, "Atiba", 30 },
                    { 626, "Atisbo", 30 },
                    { 627, "Egbeda", 30 },
                    { 628, "Ibadan North", 30 },
                    { 629, "Ibadan North-East", 30 },
                    { 630, "Ibadan North-West", 30 },
                    { 631, "Ibadan South-East", 30 },
                    { 632, "Ibadan South-West", 30 },
                    { 633, "Ibarapa Central", 30 },
                    { 634, "Ibarapa East", 30 },
                    { 635, "Ibarapa North", 30 },
                    { 636, "Ido", 30 },
                    { 637, "Irepo", 30 },
                    { 638, "Iseyin", 30 },
                    { 639, "Itesiwaju", 30 },
                    { 640, "Iwajowa", 30 },
                    { 641, "Kajola", 30 },
                    { 642, "Lagelu", 30 },
                    { 643, "Ogbomosho North", 30 },
                    { 644, "Ogbomosho South", 30 },
                    { 645, "Ogo Oluwa", 30 },
                    { 646, "Olorunsogo", 30 },
                    { 647, "Oluyole", 30 },
                    { 648, "Ona Ara", 30 },
                    { 649, "Orelope", 30 },
                    { 650, "Ori Ire", 30 },
                    { 651, "Oyo East", 30 },
                    { 652, "Oyo West", 30 },
                    { 653, "Saki East", 30 },
                    { 654, "Saki West", 30 },
                    { 655, "Surulere", 30 },
                    { 656, "Bokkos", 31 },
                    { 657, "Barkin Ladi", 31 },
                    { 658, "Bassa", 31 },
                    { 659, "Jos East", 31 },
                    { 660, "Jos North", 31 },
                    { 661, "Jos South", 31 },
                    { 662, "Kanam", 31 },
                    { 663, "Kanke", 31 },
                    { 664, "Langtang North", 31 },
                    { 665, "Langtang South", 31 },
                    { 666, "Mangu", 31 },
                    { 667, "Mikang", 31 },
                    { 668, "Pankshin", 31 },
                    { 669, "Qua’an Pan", 31 },
                    { 670, "Riyom", 31 },
                    { 671, "Shendam", 31 },
                    { 672, "Wase", 31 },
                    { 673, "Abua/Odual", 32 },
                    { 674, "Ahoada East", 32 },
                    { 675, "Ahoada West", 32 },
                    { 676, "Akuku-Toru", 32 },
                    { 677, "Andoni", 32 },
                    { 678, "Asari-Toru", 32 },
                    { 679, "Bonny", 32 },
                    { 680, "Degema", 32 },
                    { 681, "Eleme", 32 },
                    { 682, "Emohua", 32 },
                    { 683, "Etche", 32 },
                    { 684, "Gokana", 32 },
                    { 685, "Ikwerre", 32 },
                    { 686, "Khana", 32 },
                    { 687, "Obio/Akpor", 32 },
                    { 688, "Ogba/Egbema/Ndoni", 32 },
                    { 689, "Ogu/Bolo", 32 },
                    { 690, "Okrika", 32 },
                    { 691, "Omuma", 32 },
                    { 692, "Opobo/Nkoro", 32 },
                    { 693, "Oyigbo", 32 },
                    { 694, "Port Harcourt", 32 },
                    { 695, "Tai", 32 },
                    { 696, "Binji", 33 },
                    { 697, "Bodinga", 33 },
                    { 698, "Dange Shuni", 33 },
                    { 699, "Gada", 33 },
                    { 700, "Goronyo", 33 },
                    { 701, "Gudu", 33 },
                    { 702, "Gwadabawa", 33 },
                    { 703, "Illela", 33 },
                    { 704, "Isa", 33 },
                    { 705, "Kebbe", 33 },
                    { 706, "Kware", 33 },
                    { 707, "Rabah", 33 },
                    { 708, "Sabon Birni", 33 },
                    { 709, "Shagari", 33 },
                    { 710, "Sokoto North", 33 },
                    { 711, "Sokoto South", 33 },
                    { 712, "Tambuwal", 33 },
                    { 713, "Tangaza", 33 },
                    { 714, "Tureta", 33 },
                    { 715, "Wamako", 33 },
                    { 716, "Wurno", 33 },
                    { 717, "Yabo", 33 },
                    { 718, "Ardo Kola", 34 },
                    { 719, "Bali", 34 },
                    { 720, "Donga", 34 },
                    { 721, "Gashaka", 34 },
                    { 722, "Gassol", 34 },
                    { 723, "Ibi", 34 },
                    { 724, "Jalingo", 34 },
                    { 725, "Karim Lamido", 34 },
                    { 726, "Kumi", 34 },
                    { 727, "Lau", 34 },
                    { 728, "Sardauna", 34 },
                    { 729, "Takum", 34 },
                    { 730, "Ussa", 34 },
                    { 731, "Wukari", 34 },
                    { 732, "Yorro", 34 },
                    { 733, "Zing", 34 },
                    { 734, "Bade", 35 },
                    { 735, "Bursari", 35 },
                    { 736, "Damaturu", 35 },
                    { 737, "Fika", 35 },
                    { 738, "Fune", 35 },
                    { 739, "Geidam", 35 },
                    { 740, "Gujba", 35 },
                    { 741, "Gulani", 35 },
                    { 742, "Jakusko", 35 },
                    { 743, "Karasuwa", 35 },
                    { 744, "Machina", 35 },
                    { 745, "Nangere", 35 },
                    { 746, "Nguru", 35 },
                    { 747, "Potiskum", 35 },
                    { 748, "Tarmuwa", 35 },
                    { 749, "Yunusari", 35 },
                    { 750, "Yusufari", 35 },
                    { 751, "Anka", 36 },
                    { 752, "Bakura", 36 },
                    { 753, "Bungudu", 36 },
                    { 754, "Gummi", 36 },
                    { 755, "Maradun", 36 },
                    { 756, "Maru", 36 },
                    { 757, "Shinkafi", 36 },
                    { 758, "Talata Mafara", 36 },
                    { 759, "Chafe", 36 },
                    { 760, "Zurmi", 36 },
                    { 761, "Kaura Namoda", 36 },
                    { 762, "Gusau", 36 },
                    { 763, "Bukkuyum", 36 },
                    { 764, "Talata Mafara", 36 },
                    { 765, "Abaji", 37 },
                    { 766, "Bwari", 37 },
                    { 767, "Gwagwalada", 37 },
                    { 768, "Kuje", 37 },
                    { 769, "Kwali", 37 },
                    { 770, "Municipal Area Council", 37 },
                    { 771, "Tsafe", 36 },
                    { 772, "Ohimini", 7 },
                    { 773, "Dutsin-Ma", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 383);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 384);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 385);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 386);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 387);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 388);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 389);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 390);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 391);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 392);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 393);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 394);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 395);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 396);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 397);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 398);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 399);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 408);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 409);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 410);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 412);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 413);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 414);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 415);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 416);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 417);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 418);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 419);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 420);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 424);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 425);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 426);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 427);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 428);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 429);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 430);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 431);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 432);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 433);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 434);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 435);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 436);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 437);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 438);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 439);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 440);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 441);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 442);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 443);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 444);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 445);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 446);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 447);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 448);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 449);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 450);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 451);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 452);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 453);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 454);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 455);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 456);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 457);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 458);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 459);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 460);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 461);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 462);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 463);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 464);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 465);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 466);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 467);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 468);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 469);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 470);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 471);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 472);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 473);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 474);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 475);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 476);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 477);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 478);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 479);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 480);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 481);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 482);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 483);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 484);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 485);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 486);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 487);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 488);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 489);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 490);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 491);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 492);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 493);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 494);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 495);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 496);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 497);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 498);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 499);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 538);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 539);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 547);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 548);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 549);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 550);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 551);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 552);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 553);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 554);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 555);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 556);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 557);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 558);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 559);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 560);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 561);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 562);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 563);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 564);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 565);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 566);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 567);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 568);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 569);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 570);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 571);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 572);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 573);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 574);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 575);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 576);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 577);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 578);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 579);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 580);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 581);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 582);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 583);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 584);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 585);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 586);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 587);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 588);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 589);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 590);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 591);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 592);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 593);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 594);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 595);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 596);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 597);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 598);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 599);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 600);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 601);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 602);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 603);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 604);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 605);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 606);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 607);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 608);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 609);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 610);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 611);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 612);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 613);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 614);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 615);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 616);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 617);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 618);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 619);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 620);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 621);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 622);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 623);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 624);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 625);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 626);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 627);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 628);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 629);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 630);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 631);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 632);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 633);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 634);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 635);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 636);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 637);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 638);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 639);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 640);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 641);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 642);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 643);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 644);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 645);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 646);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 647);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 648);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 649);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 650);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 651);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 652);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 653);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 654);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 655);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 656);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 657);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 658);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 659);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 660);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 661);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 662);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 663);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 664);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 665);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 666);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 667);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 668);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 669);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 670);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 671);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 672);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 673);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 674);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 675);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 676);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 677);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 678);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 679);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 680);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 681);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 682);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 683);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 684);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 685);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 686);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 687);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 688);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 689);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 690);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 691);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 692);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 693);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 694);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 695);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 696);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 697);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 698);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 699);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 700);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 701);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 702);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 703);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 704);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 705);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 706);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 707);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 708);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 709);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 710);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 711);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 712);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 713);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 714);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 715);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 716);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 717);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 718);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 719);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 720);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 721);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 722);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 723);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 724);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 725);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 726);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 727);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 728);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 729);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 730);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 731);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 732);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 733);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 734);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 735);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 736);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 737);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 738);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 739);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 740);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 741);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 742);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 743);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 744);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 745);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 746);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 747);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 748);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 749);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 750);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 751);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 752);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 753);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 754);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 755);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 756);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 757);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 758);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 759);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 760);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 761);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 762);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 763);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 764);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 765);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 766);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 767);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 768);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 769);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 770);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 771);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 772);

            migrationBuilder.DeleteData(
                table: "Lgas",
                keyColumn: "Id",
                keyValue: 773);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lgas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
