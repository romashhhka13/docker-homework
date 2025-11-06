using Microsoft.EntityFrameworkCore;
using ShpaginApp.Data;
using ShpaginApp.Models.Entities;

namespace ShpaginApp.Models;

public static class SeedData
{
  public static Guid DeterministicGuidFromInt(int seed)
  {
    byte[] bytes = new byte[16];
    byte[] seedBytes = BitConverter.GetBytes(seed);
    bytes[15] = seedBytes[0];
    bytes[14] = seedBytes[1];
    bytes[13] = seedBytes[2];
    bytes[12] = seedBytes[3];
    return new Guid(bytes);
  }
  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var context = new ShpaginAppContext(
        serviceProvider.GetRequiredService<DbContextOptions<ShpaginAppContext>>()
    );

    // Проверяем, есть ли уже данные
    if (context.Books.Any() || context.Authors.Any() || context.Users.Any() ||
        context.BooksAuthors.Any() || context.UsersBooks.Any() || context.Tags.Any())
    {
      return;
    }

    // Генерируем 100 уникальных ID
    // Guid[] Ids = [.. Enumerable.Range(0, 100).Select(_ => Guid.NewGuid())];
    Guid[] Ids = [.. Enumerable.Range(1, 100).Select(i => DeterministicGuidFromInt(i))];

    using var transaction = context.Database.BeginTransaction();

    try
    {
      // ============= BOOKS =============
      context.Books.AddRange(
        // Серия "Дающий" - Лоис Лоури
        new Book
        {
          Id = Ids[0],
          Name = "Дающий",
          Year = 1993,
          Publisher = "Розовый Жираф",
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Первая книга знаменитой тетралогии о дистопическом обществе без эмоций и воспоминаний.",
        },
        new Book
        {
          Id = Ids[1],
          Name = "В поисках синего",
          Publisher = "Розовый Жираф",
          Year = 2000,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Продолжение истории о мире, где люди лишены чувств.",
        },
        new Book
        {
          Id = Ids[2],
          Name = "Вестник",
          Publisher = "Розовый Жираф",
          Year = 2004,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Третья книга тетралогии о поиске свободы и истины.",
        },
        new Book
        {
          Id = Ids[3],
          Name = "Сын",
          Publisher = "Розовый Жираф",
          Year = 2012,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Заключительная часть тетралогии 'Дающий'.",
        },

        // Достоевский
        new Book
        {
          Id = Ids[4],
          Name = "Белые ночи",
          Publisher = "Азбука",
          Year = 1848,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Сентиментальный роман о любви и мечтах в Петербурге.",
        },
        new Book
        {
          Id = Ids[5],
          Name = "Идиот",
          Publisher = "АСТ",
          Year = 1869,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Роман о князе Мышкине - 'положительно прекрасном человеке'.",
        },
        new Book
        {
          Id = Ids[6],
          Name = "Преступление и наказание",
          Publisher = "АСТ",
          Year = 1866,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "История Раскольникова и его теории о 'праве на кровь'.",
        },
        new Book
        {
          Id = Ids[7],
          Name = "Братья Карамазовы",
          Publisher = "Эксмо",
          Year = 1880,
          AgeRating = RussianAgeRatingEnum.EIGHTEEN,
          Annotation = "Философский роман о вере, сомнении и нравственности.",
        },
        new Book
        {
          Id = Ids[8],
          Name = "Бесы",
          Publisher = "Эксмо",
          Year = 1872,
          AgeRating = RussianAgeRatingEnum.EIGHTEEN,
          Annotation = "Роман о революционерах и разрушительных идеях.",
        },

        // Пушкин
        new Book
        {
          Id = Ids[9],
          Name = "Евгений Онегин",
          Publisher = "Азбука",
          Year = 1833,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Роман в стихах о судьбе дворянина и его любви.",
        },
        new Book
        {
          Id = Ids[10],
          Name = "Борис Годунов",
          Publisher = "АСТ",
          Year = 1831,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Драма о царе Борисе и борьбе за власть.",
        },
        new Book
        {
          Id = Ids[11],
          Name = "Капитанская дочка",
          Publisher = "Детская литература",
          Year = 1836,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Исторический роман о Пугачевском восстании.",
        },
        new Book
        {
          Id = Ids[12],
          Name = "Дубровский",
          Publisher = "Азбука",
          Year = 1841,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Романтическая повесть о благородном разбойнике.",
        },
        new Book
        {
          Id = Ids[13],
          Name = "Повести Белкина",
          Publisher = "АСТ",
          Year = 1831,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Сборник из пяти повестей о русской жизни.",
        },

        // Ремарк
        new Book
        {
          Id = Ids[14],
          Name = "На Западном фронте без перемен",
          Publisher = "АСТ",
          Year = 1929,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Антивоенный роман о Первой мировой войне.",
        },
        new Book
        {
          Id = Ids[15],
          Name = "Триумфальная арка",
          Publisher = "АСТ",
          Year = 1945,
          AgeRating = RussianAgeRatingEnum.EIGHTEEN,
          Annotation = "История любви немецкого хирурга в предвоенном Париже.",
        },
        new Book
        {
          Id = Ids[16],
          Name = "Три товарища",
          Publisher = "АСТ",
          Year = 1936,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Роман о дружбе и любви в послевоенной Германии.",
        },
        new Book
        {
          Id = Ids[17],
          Name = "Ночь в Лиссабоне",
          Publisher = "АСТ",
          Year = 1962,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "История беженца, спасающегося от нацистов.",
        },
        new Book
        {
          Id = Ids[18],
          Name = "Возлюби ближнего своего",
          Publisher = "АСТ",
          Year = 1941,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Роман о немецких эмигрантах в Америке.",
        },

        // Дополнительные авторы для разнообразия
        // Толстой
        new Book
        {
          Id = Ids[19],
          Name = "Война и мир",
          Publisher = "Эксмо",
          Year = 1869,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Эпопея о России во время войны 1812 года.",
        },
        new Book
        {
          Id = Ids[20],
          Name = "Анна Каренина",
          Publisher = "АСТ",
          Year = 1877,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Трагическая история о любви и семье.",
        },

        // Оруэлл
        new Book
        {
          Id = Ids[21],
          Name = "1984",
          Publisher = "АСТ",
          Year = 1949,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Антиутопия о тоталитарном государстве.",
        },
        new Book
        {
          Id = Ids[22],
          Name = "Скотный двор",
          Publisher = "АСТ",
          Year = 1945,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Сатирическая повесть-притча о революции.",
        },

        // Брэдбери
        new Book
        {
          Id = Ids[23],
          Name = "451 градус по Фаренгейту",
          Publisher = "Эксмо",
          Year = 1953,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Антиутопия о мире, где книги запрещены.",
        },
        new Book
        {
          Id = Ids[24],
          Name = "Марсианские хроники",
          Publisher = "Эксмо",
          Year = 1950,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Сборник рассказов о колонизации Марса.",
        },

        // Хемингуэй
        new Book
        {
          Id = Ids[25],
          Name = "Старик и море",
          Publisher = "АСТ",
          Year = 1952,
          AgeRating = RussianAgeRatingEnum.TWELVE,
          Annotation = "Повесть о рыбаке и его борьбе с гигантской рыбой.",
        },
        new Book
        {
          Id = Ids[26],
          Name = "Прощай, оружие!",
          Publisher = "АСТ",
          Year = 1929,
          AgeRating = RussianAgeRatingEnum.SIXTEEN,
          Annotation = "Роман о любви во время Первой мировой войны.",
        }
      );
      context.SaveChanges();

      // ============= AUTHORS =============
      context.Authors.AddRange(
        new Author
        {
          Id = Ids[40],
          Name = "Лоис Лоури",
          Description = "Американская писательница, автор детских и подростковых книг, дважды лауреат медали Ньюбери."
        },
        new Author
        {
          Id = Ids[41],
          Name = "Федор Достоевский",
          Description = "Великий русский писатель, философ и мыслитель, один из самых значимых мировых романистов."
        },
        new Author
        {
          Id = Ids[42],
          Name = "Александр Пушкин",
          Description = "Величайший русский поэт, основоположник современного русского литературного языка."
        },
        new Author
        {
          Id = Ids[43],
          Name = "Эрих Мария Ремарк",
          Description = "Немецкий писатель, известный антивоенными романами о 'потерянном поколении'."
        },
        new Author
        {
          Id = Ids[44],
          Name = "Лев Толстой",
          Description = "Один из величайших писателей мира, автор эпических романов и нравственный философ."
        },
        new Author
        {
          Id = Ids[45],
          Name = "Джордж Оруэлл",
          Description = "Британский писатель и публицист, известный антиутопиями и политической сатирой."
        },
        new Author
        {
          Id = Ids[46],
          Name = "Рэй Брэдбери",
          Description = "Американский писатель-фантаст, мастер короткого рассказа и литературной фантастики."
        },
        new Author
        {
          Id = Ids[47],
          Name = "Эрнест Хемингуэй",
          Description = "Американский писатель, лауреат Нобелевской премии, представитель 'потерянного поколения'."
        }
      );
      context.SaveChanges();

      // ============= BOOK-AUTHORS RELATIONS =============
      context.BooksAuthors.AddRange(
        // Лоис Лоури
        new BookAuthor { BookId = Ids[0], AuthorId = Ids[40] },
        new BookAuthor { BookId = Ids[1], AuthorId = Ids[40] },
        new BookAuthor { BookId = Ids[2], AuthorId = Ids[40] },
        new BookAuthor { BookId = Ids[3], AuthorId = Ids[40] },

        // Достоевский
        new BookAuthor { BookId = Ids[4], AuthorId = Ids[41] },
        new BookAuthor { BookId = Ids[5], AuthorId = Ids[41] },
        new BookAuthor { BookId = Ids[6], AuthorId = Ids[41] },
        new BookAuthor { BookId = Ids[7], AuthorId = Ids[41] },
        new BookAuthor { BookId = Ids[8], AuthorId = Ids[41] },

        // Пушкин
        new BookAuthor { BookId = Ids[9], AuthorId = Ids[42] },
        new BookAuthor { BookId = Ids[10], AuthorId = Ids[42] },
        new BookAuthor { BookId = Ids[11], AuthorId = Ids[42] },
        new BookAuthor { BookId = Ids[12], AuthorId = Ids[42] },
        new BookAuthor { BookId = Ids[13], AuthorId = Ids[42] },

        // Ремарк
        new BookAuthor { BookId = Ids[14], AuthorId = Ids[43] },
        new BookAuthor { BookId = Ids[15], AuthorId = Ids[43] },
        new BookAuthor { BookId = Ids[16], AuthorId = Ids[43] },
        new BookAuthor { BookId = Ids[17], AuthorId = Ids[43] },
        new BookAuthor { BookId = Ids[18], AuthorId = Ids[43] },

        // Толстой
        new BookAuthor { BookId = Ids[19], AuthorId = Ids[44] },
        new BookAuthor { BookId = Ids[20], AuthorId = Ids[44] },

        // Оруэлл
        new BookAuthor { BookId = Ids[21], AuthorId = Ids[45] },
        new BookAuthor { BookId = Ids[22], AuthorId = Ids[45] },

        // Брэдбери
        new BookAuthor { BookId = Ids[23], AuthorId = Ids[46] },
        new BookAuthor { BookId = Ids[24], AuthorId = Ids[46] },

        // Хемингуэй
        new BookAuthor { BookId = Ids[25], AuthorId = Ids[47] },
        new BookAuthor { BookId = Ids[26], AuthorId = Ids[47] }
      );
      context.SaveChanges();

      // ============= TAGS =============
      context.Tags.AddRange(
        new Tag { Id = Ids[50], Value = "Классика" },
        new Tag { Id = Ids[51], Value = "Русская литература" },
        new Tag { Id = Ids[52], Value = "Философия" },
        new Tag { Id = Ids[53], Value = "Психология" },
        new Tag { Id = Ids[54], Value = "Драма" },
        new Tag { Id = Ids[55], Value = "Романтика" },
        new Tag { Id = Ids[56], Value = "Дистопия" },
        new Tag { Id = Ids[57], Value = "Фантастика" },
        new Tag { Id = Ids[58], Value = "Детектив" },
        new Tag { Id = Ids[59], Value = "Война" },
        new Tag { Id = Ids[60], Value = "Исторический роман" },
        new Tag { Id = Ids[61], Value = "Приключения" },
        new Tag { Id = Ids[62], Value = "Подростковая литература" },
        new Tag { Id = Ids[63], Value = "Семья" },
        new Tag { Id = Ids[64], Value = "Социальная проза" },
        new Tag { Id = Ids[65], Value = "Антиутопия" },
        new Tag { Id = Ids[66], Value = "Трагедия" },
        new Tag { Id = Ids[67], Value = "Сатира" },
        new Tag { Id = Ids[68], Value = "Поэзия" },
        new Tag { Id = Ids[69], Value = "20 век" }
      );
      context.SaveChanges();

      // ============= BOOK-TAGS RELATIONS =============
      context.BooksTags.AddRange(
        // Дающий - дистопия, подростковая, фантастика
        new BookTag { BookId = Ids[0], TagId = Ids[56] },
        new BookTag { BookId = Ids[0], TagId = Ids[62] },
        new BookTag { BookId = Ids[0], TagId = Ids[57] },
        new BookTag { BookId = Ids[1], TagId = Ids[56] },
        new BookTag { BookId = Ids[1], TagId = Ids[62] },
        new BookTag { BookId = Ids[2], TagId = Ids[56] },
        new BookTag { BookId = Ids[2], TagId = Ids[62] },
        new BookTag { BookId = Ids[3], TagId = Ids[56] },
        new BookTag { BookId = Ids[3], TagId = Ids[62] },

        // Белые ночи - классика, русская, романтика
        new BookTag { BookId = Ids[4], TagId = Ids[50] },
        new BookTag { BookId = Ids[4], TagId = Ids[51] },
        new BookTag { BookId = Ids[4], TagId = Ids[55] },
        new BookTag { BookId = Ids[4], TagId = Ids[54] },

        // Идиот - классика, философия, психология, драма
        new BookTag { BookId = Ids[5], TagId = Ids[50] },
        new BookTag { BookId = Ids[5], TagId = Ids[51] },
        new BookTag { BookId = Ids[5], TagId = Ids[52] },
        new BookTag { BookId = Ids[5], TagId = Ids[53] },
        new BookTag { BookId = Ids[5], TagId = Ids[54] },

        // Преступление и наказание - классика, философия, психология, детектив
        new BookTag { BookId = Ids[6], TagId = Ids[50] },
        new BookTag { BookId = Ids[6], TagId = Ids[51] },
        new BookTag { BookId = Ids[6], TagId = Ids[52] },
        new BookTag { BookId = Ids[6], TagId = Ids[53] },
        new BookTag { BookId = Ids[6], TagId = Ids[58] },

        // Братья Карамазовы - классика, философия, драма
        new BookTag { BookId = Ids[7], TagId = Ids[50] },
        new BookTag { BookId = Ids[7], TagId = Ids[51] },
        new BookTag { BookId = Ids[7], TagId = Ids[52] },
        new BookTag { BookId = Ids[7], TagId = Ids[54] },
        new BookTag { BookId = Ids[7], TagId = Ids[63] },

        // Бесы - классика, философия, социальная проза
        new BookTag { BookId = Ids[8], TagId = Ids[50] },
        new BookTag { BookId = Ids[8], TagId = Ids[51] },
        new BookTag { BookId = Ids[8], TagId = Ids[52] },
        new BookTag { BookId = Ids[8], TagId = Ids[64] },

        // Евгений Онегин - классика, русская, поэзия, романтика
        new BookTag { BookId = Ids[9], TagId = Ids[50] },
        new BookTag { BookId = Ids[9], TagId = Ids[51] },
        new BookTag { BookId = Ids[9], TagId = Ids[68] },
        new BookTag { BookId = Ids[9], TagId = Ids[55] },

        // Борис Годунов - классика, исторический, драма
        new BookTag { BookId = Ids[10], TagId = Ids[50] },
        new BookTag { BookId = Ids[10], TagId = Ids[51] },
        new BookTag { BookId = Ids[10], TagId = Ids[60] },
        new BookTag { BookId = Ids[10], TagId = Ids[54] },

        // Капитанская дочка - классика, исторический, приключения
        new BookTag { BookId = Ids[11], TagId = Ids[50] },
        new BookTag { BookId = Ids[11], TagId = Ids[51] },
        new BookTag { BookId = Ids[11], TagId = Ids[60] },
        new BookTag { BookId = Ids[11], TagId = Ids[61] },

        // Дубровский - классика, романтика, приключения
        new BookTag { BookId = Ids[12], TagId = Ids[50] },
        new BookTag { BookId = Ids[12], TagId = Ids[51] },
        new BookTag { BookId = Ids[12], TagId = Ids[55] },
        new BookTag { BookId = Ids[12], TagId = Ids[61] },

        // Повести Белкина - классика, русская
        new BookTag { BookId = Ids[13], TagId = Ids[50] },
        new BookTag { BookId = Ids[13], TagId = Ids[51] },

        // На Западном фронте - война, драма, 20 век
        new BookTag { BookId = Ids[14], TagId = Ids[59] },
        new BookTag { BookId = Ids[14], TagId = Ids[54] },
        new BookTag { BookId = Ids[14], TagId = Ids[69] },
        new BookTag { BookId = Ids[14], TagId = Ids[50] },

        // Триумфальная арка - война, романтика, драма
        new BookTag { BookId = Ids[15], TagId = Ids[59] },
        new BookTag { BookId = Ids[15], TagId = Ids[55] },
        new BookTag { BookId = Ids[15], TagId = Ids[54] },
        new BookTag { BookId = Ids[15], TagId = Ids[69] },

        // Три товарища - война, романтика, драма
        new BookTag { BookId = Ids[16], TagId = Ids[59] },
        new BookTag { BookId = Ids[16], TagId = Ids[55] },
        new BookTag { BookId = Ids[16], TagId = Ids[54] },
        new BookTag { BookId = Ids[16], TagId = Ids[69] },

        // Ночь в Лиссабоне - война, драма
        new BookTag { BookId = Ids[17], TagId = Ids[59] },
        new BookTag { BookId = Ids[17], TagId = Ids[54] },
        new BookTag { BookId = Ids[17], TagId = Ids[69] },

        // Возлюби ближнего - война, социальная проза
        new BookTag { BookId = Ids[18], TagId = Ids[59] },
        new BookTag { BookId = Ids[18], TagId = Ids[64] },
        new BookTag { BookId = Ids[18], TagId = Ids[69] },

        // Война и мир - классика, исторический, война, семья
        new BookTag { BookId = Ids[19], TagId = Ids[50] },
        new BookTag { BookId = Ids[19], TagId = Ids[51] },
        new BookTag { BookId = Ids[19], TagId = Ids[60] },
        new BookTag { BookId = Ids[19], TagId = Ids[59] },
        new BookTag { BookId = Ids[19], TagId = Ids[63] },

        // Анна Каренина - классика, драма, семья, трагедия
        new BookTag { BookId = Ids[20], TagId = Ids[50] },
        new BookTag { BookId = Ids[20], TagId = Ids[51] },
        new BookTag { BookId = Ids[20], TagId = Ids[54] },
        new BookTag { BookId = Ids[20], TagId = Ids[63] },
        new BookTag { BookId = Ids[20], TagId = Ids[66] },

        // 1984 - антиутопия, фантастика, социальная проза
        new BookTag { BookId = Ids[21], TagId = Ids[65] },
        new BookTag { BookId = Ids[21], TagId = Ids[57] },
        new BookTag { BookId = Ids[21], TagId = Ids[64] },
        new BookTag { BookId = Ids[21], TagId = Ids[69] },

        // Скотный двор - сатира, социальная проза
        new BookTag { BookId = Ids[22], TagId = Ids[67] },
        new BookTag { BookId = Ids[22], TagId = Ids[64] },
        new BookTag { BookId = Ids[22], TagId = Ids[69] },

        // 451 - антиутопия, фантастика
        new BookTag { BookId = Ids[23], TagId = Ids[65] },
        new BookTag { BookId = Ids[23], TagId = Ids[57] },
        new BookTag { BookId = Ids[23], TagId = Ids[69] },

        // Марсианские хроники - фантастика
        new BookTag { BookId = Ids[24], TagId = Ids[57] },
        new BookTag { BookId = Ids[24], TagId = Ids[69] },

        // Старик и море - классика, драма
        new BookTag { BookId = Ids[25], TagId = Ids[50] },
        new BookTag { BookId = Ids[25], TagId = Ids[54] },
        new BookTag { BookId = Ids[25], TagId = Ids[69] },

        // Прощай, оружие - война, романтика, драма
        new BookTag { BookId = Ids[26], TagId = Ids[59] },
        new BookTag { BookId = Ids[26], TagId = Ids[55] },
        new BookTag { BookId = Ids[26], TagId = Ids[54] },
        new BookTag { BookId = Ids[26], TagId = Ids[69] }
      );
      context.SaveChanges();

      // ============= USERS =============
      context.Users.AddRange(
        new User
        {
          Id = Ids[70],
          IsAdmin = true,
          UserName = "admin",
          PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=",
          Email = "admin@email.com",
          DisplayName = "Администратор",
          BirthDate = null,
        },
        new User
        {
          Id = Ids[71],
          IsAdmin = false,
          UserName = "artem_semenov",
          PasswordHash = "61po9iIsfktlnanlRZGhEoTWA8fqhqSkgBT6P3SXe7g=",
          Email = "artem.semenov@email.com",
          DisplayName = "Артем Семенов",
          BirthDate = new DateOnly(1995, 5, 15),
        },
        new User
        {
          Id = Ids[72],
          IsAdmin = false,
          UserName = "aleksei_rudnev",
          PasswordHash = "LsCuSa8BvqazlSgn1yOPLMBxDhcQLUpEH4lu3nTYlhI=",
          Email = "aleksei.rudnev@email.com",
          DisplayName = "Алексей Руднев",
          BirthDate = new DateOnly(2001, 11, 3),
        },
        new User
        {
          Id = Ids[73],
          IsAdmin = false,
          UserName = "nikolai_tretyakov",
          PasswordHash = "/P/jM9/x0iq3VvQlH5HzQQQye2+0LYApxtGHn3pLOXg=",
          Email = "nikolai.tretyakov@email.com",
          DisplayName = "Третьяков Николай Сергеевич",
          BirthDate = new DateOnly(1998, 3, 22),
        },
        new User
        {
          Id = Ids[74],
          IsAdmin = false,
          UserName = "valeria_mironova",
          PasswordHash = "rH7zBrVxrbunFLRVhXLYDAudC3iJjPstPGO1MwUwtZA=",
          Email = "valeria.mironova@email.com",
          DisplayName = "Миронова Валерия Александровна",
          BirthDate = new DateOnly(2004, 12, 1),
        },
        new User
        {
          Id = Ids[75],
          IsAdmin = false,
          UserName = "maria_volkova",
          PasswordHash = "3NLIorl8iShTkM1WKU6U1WJJ2VdsJ4on0keh70AGpOg=",
          Email = "maria.volkova@email.com",
          DisplayName = "Мария Волкова",
          BirthDate = new DateOnly(1992, 7, 18),
        },
        new User
        {
          Id = Ids[76],
          IsAdmin = false,
          UserName = "dmitry_sokolov",
          PasswordHash = "+bZ9jEo4i7BubzDbKGfNVyXPKPmF6SsyEvKCejWrLN8=",
          Email = "dmitry.sokolov@email.com",
          DisplayName = "Дмитрий Соколов",
          BirthDate = new DateOnly(1988, 2, 9),
        },
        new User
        {
          Id = Ids[77],
          IsAdmin = false,
          UserName = "elena_kozlova",
          PasswordHash = "yFQbo+MjASJGsfVpL/m+7J6PddCx+vH2bRiI8og9LZI=",
          Email = "elena.kozlova@email.com",
          DisplayName = "Елена Козлова",
          BirthDate = new DateOnly(1999, 9, 30),
        }
      );
      context.SaveChanges();

      // ============= USER-BOOKS RELATIONS =============
      context.UsersBooks.AddRange(new List<UserBook>
        {
          // artem_semenov (71) - любит философскую классику
          new UserBook { UserId = Ids[71], BookId = Ids[6], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[71], BookId = Ids[7], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[71], BookId = Ids[5], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[71], BookId = Ids[8], Status = BookStatusEnum.READING, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[71], BookId = Ids[19], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[71], BookId = Ids[20], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[71], BookId = Ids[21], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },

          // aleksei_rudnev (72) - фанат Ремарка и военной прозы
          new UserBook { UserId = Ids[72], BookId = Ids[14], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[72], BookId = Ids[16], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[72], BookId = Ids[15], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = true },
          new UserBook { UserId = Ids[72], BookId = Ids[17], Status = BookStatusEnum.READING, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[72], BookId = Ids[26], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[72], BookId = Ids[18], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },

          // nikolai_tretyakov (73) - любит русскую классику
          new UserBook { UserId = Ids[73], BookId = Ids[9], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[73], BookId = Ids[11], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[73], BookId = Ids[12], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[73], BookId = Ids[19], Status = BookStatusEnum.READING, Rating = null, Favorite = true },
          new UserBook { UserId = Ids[73], BookId = Ids[6], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[73], BookId = Ids[20], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },

          // valeria_mironova (74) - любит дистопии и фантастику
          new UserBook { UserId = Ids[74], BookId = Ids[0], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[74], BookId = Ids[1], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[74], BookId = Ids[2], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[74], BookId = Ids[3], Status = BookStatusEnum.READING, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[74], BookId = Ids[21], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[74], BookId = Ids[23], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[74], BookId = Ids[24], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[74], BookId = Ids[22], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = false },

          // maria_volkova (75) - универсальный читатель, любит все понемногу
          new UserBook { UserId = Ids[75], BookId = Ids[6], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[75], BookId = Ids[9], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[75], BookId = Ids[14], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[75], BookId = Ids[16], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[75], BookId = Ids[0], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[75], BookId = Ids[21], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[75], BookId = Ids[19], Status = BookStatusEnum.READING, Rating = null, Favorite = true },
          new UserBook { UserId = Ids[75], BookId = Ids[25], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },

          // dmitry_sokolov (76) - любитель классики, но ставит средние оценки
          new UserBook { UserId = Ids[76], BookId = Ids[19], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[76], BookId = Ids[20], Status = BookStatusEnum.COMPLETED, Rating = 3, Favorite = false },
          new UserBook { UserId = Ids[76], BookId = Ids[7], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[76], BookId = Ids[5], Status = BookStatusEnum.COMPLETED, Rating = 3, Favorite = false },
          new UserBook { UserId = Ids[76], BookId = Ids[11], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = null },
          new UserBook { UserId = Ids[76], BookId = Ids[10], Status = BookStatusEnum.READING, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[76], BookId = Ids[13], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },

          // elena_kozlova (77) - молодая читательница, любит современную прозу и дистопии
          new UserBook { UserId = Ids[77], BookId = Ids[21], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[77], BookId = Ids[23], Status = BookStatusEnum.COMPLETED, Rating = 5, Favorite = true },
          new UserBook { UserId = Ids[77], BookId = Ids[0], Status = BookStatusEnum.COMPLETED, Rating = 4, Favorite = true },
          new UserBook { UserId = Ids[77], BookId = Ids[22], Status = BookStatusEnum.READING, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[77], BookId = Ids[24], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[77], BookId = Ids[1], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = false },

          // Дополнительные записи для проверки трендов (недавние добавления)
          new UserBook { UserId = Ids[72], BookId = Ids[21], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[73], BookId = Ids[21], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },
          new UserBook { UserId = Ids[76], BookId = Ids[23], Status = BookStatusEnum.WANT_TO_READ, Rating = null, Favorite = null },

          // Общие книги для проверки совместимости пользователей
          new UserBook { UserId = Ids[71], BookId = Ids[4], Status = BookStatusEnum.COMPLETED, Rating = 3, Favorite = false },
          new UserBook { UserId = Ids[75], BookId = Ids[4], Status = BookStatusEnum.COMPLETED, Rating = 3, Favorite = false },
        }
      );
      context.SaveChanges();

      context.SaveChanges();
      transaction.Commit();
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      Console.WriteLine($"❌ Ошибка при инициализации данных: {ex.Message}");
      throw;
    }
  }
}
