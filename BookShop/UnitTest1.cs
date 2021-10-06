using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace BookShop
{
    public class UnitTest1
    {
        [Fact]
        public void rent4booksIsForbidden()
        {
            var library = new Library();
            library.CanBookABook("Miriam", "Harry Potter 1");
            library.CanBookABook("Miriam", "Harry Potter 2");
            library.CanBookABook("Miriam", "Harry Potter 3");
            var result = library.CanBookABook("Miriam", "Harry Potter 4");
            Assert.False(result);
        }
        [Fact]
        public void rent2PersonTheSameBooksIsForbidden()
        {
            var library = new Library();
            library.CanBookABook("Miriam", "Harry Potter 1");
            var result = library.CanBookABook("Rob", "Harry Potter 4");
            Assert.True(result);
        }
        [Fact]
        public void test()
        {
            var library = new Library();
            library.CanBookABook("Mike", "Harry Potter 5");
            library.CanBookABook("Mike", "Harry Potter 2");
            library.CanBookABook("Mike", "Harry Potter 3");
            var result1=library.CanBookABook("Mike", "Harry Potter 3");
            Assert.False(result1);
            var result = library.CanBookABook("Rob", "Harry Potter 4");
            Assert.True(result);
        }

    }

    public class Library
    {

        public Dictionary<int, BookList> BookHistory = new()
        {
            { 1, new BookList() { Name = "Harry Potter 1", Available = true, User= null}},
            { 2, new BookList() { Name = "Harry Potter 2", Available = true, User = null}},
            { 3, new BookList() { Name = "Harry Potter 3", Available = true, User = null}},
            { 4, new BookList() { Name = "Harry Potter 4", Available = true, User = null}},
            { 5, new BookList() { Name = "Harry Potter 5", Available = true, User = null}}
        };

        public Dictionary<int, PersonDescription> UserHistory = new ()
        {
            {1, new PersonDescription(){ Name = "Miriam", NumberBook =0}},
            {2, new PersonDescription(){ Name = "Mike", NumberBook  = 2}},
            {3, new PersonDescription(){ Name = "Rob", NumberBook = 1}},
        };

        private static int KeyUser { get; set; }
        public static int KeyBook { get; private set; }

        private bool _bookAvailable;

        public bool CanBookABook(string name, string book)
        {
            
            GetUserId(name);
            if (!CheckIfUserHasLessThan4Books() || !CheckIfBookIsAvailable(book)) return false;
            AddUserAndUpdateAvailability();
            AddUserNumberBooks();
            return true;
        }

        private void AddUserNumberBooks()
        {
            UserHistory[KeyUser].NumberBook += 1;
        }

        private void AddUserAndUpdateAvailability()
        {
            BookHistory[KeyBook].User = KeyUser;
            BookHistory[KeyBook].Available = false;
        }

        private bool CheckIfBookIsAvailable(string book)
        {
            foreach (var (key, _) in BookHistory)
            {
                
                if (BookHistory[key].Name != book || !BookHistory[key].Available) continue;
                _bookAvailable = false;
                KeyBook = key;
                _bookAvailable = true;
                break;

            }

            return _bookAvailable;
        }

        private bool CheckIfUserHasLessThan4Books()
        {
            return UserHistory[KeyUser].NumberBook < 3;
        }

        public int GetUserId(string name)
        {
            foreach (var userId in UserHistory
                .Where(userId => UserHistory[userId.Key].Name == name))
            {
                KeyUser = userId.Key;
            }
            return KeyUser;
        }

    }

    public class PersonDescription
    {
        public string Name { get; set; }
        public int NumberBook { get; set; }
    }

    public class BookList
    {
        public string Name { get; set; }
        public bool Available { get; set; }
        public object User { get; set; }
    }
}
