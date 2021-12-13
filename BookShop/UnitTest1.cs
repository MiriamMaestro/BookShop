using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Xunit;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BookShop
{
    public class UnitTest1
    {

        [Fact]
        public void IfLessThan4BookAndBookUnavailable()
        {
            var library = new Library();
            var result = library.BookaBook("Rob", "Harry Potter 1");
            Assert.False(result);

        }
        [Fact]
        public void IfLessThan4BookAndBookAvailable()
        {
            var library = new Library();
            var result = library.BookaBook("Mike", "Harry Potter 6");
            Assert.True(result);

        }
        [Fact]
        public void useIdr()
        {
            var library = new Library();
            var result = library.BookaBook("Rob", "Harry Potter 1");
            Assert.False(result);

        }
        [Fact]
        public void user()
        {
            var library = new Library();
            library.GetUser(2);

        }

    }

    public class Library
    {
        private User user;
        private Book book;


        private int userId { get; set; }

        
        public bool BookaBook( string name, string bookName)
        { 
            userId = ReadUserId(name);
            User user = GetUser(userId);
            Book book = GetBook(bookName);

            if (!CheckIfUserHasLessThan4Books(user) 
                || !CheckIfBookIsAvailable(book)) return false;
            AddUser(user, book);
            AddUserNumberBooks(user);
            return true;
        }

        private void AddUserNumberBooks(User user)
        {
            using (var con = new SqliteConnection("Data Source=Library.sqlite"))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM BookLibrary WHERE User = @User";
                command.Parameters.AddWithValue("@User", user.Name);
                var totalBooks = command.ExecuteScalar();
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "UPDATE UserList SET numberBook = @NumberBook WHERE UserName = @UserName";
                comm.Parameters.AddWithValue("@UserName", user.Name);
                comm.Parameters.AddWithValue("@NumberBook", totalBooks);
                comm.ExecuteScalar();
            }
        }

        private void AddUser(User user, Book book)
        {
            using (var con = new SqliteConnection("Data Source=Library.sqlite"))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "UPDATE BookLibrary SET User = @User WHERE BookId = @BookId";
                command.Parameters.AddWithValue("@User", user.Name);
                command.Parameters.AddWithValue("@BookId", book.BookId);
                command.ExecuteScalar();
                

            }
        }

        private Book GetBook(string bookName)
        {
            Book book = null;
            using (var con = new SqliteConnection("Data Source=Library.sqlite"))
            {
                con.Open();

                string stm = "SELECT * From BookLibrary Where Name= @Name";

                using (SqliteCommand cmd = new SqliteCommand(stm, con))
                {
                    cmd.Parameters.AddWithValue("@Name", bookName);
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            //add items to your list
                            int BookId = (int)(long)rdr["BookId"];
                            string Name = (string)rdr["Name"];
                            var User = Convert.IsDBNull(rdr["User"])?String.Empty : (string)rdr["User"];
                            book = new Book() { BookId = BookId, Name = Name, User = User};
                        }
                    }
                }
                con.Close();

            }
            return book;
        }

        private bool CheckIfBookIsAvailable(Book book)
        {
            if (string.IsNullOrEmpty(book.User)) return true;
            return false;
        }

        private bool CheckIfUserHasLessThan4Books(User user)
        {
            return user.NumberBook < 3;
        }

        public static int ReadUserId( string name)
        {
            using (var connection = new SqliteConnection("Data Source=Library.sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id From UserList where UserName = @name";
                command.Parameters.AddWithValue("@name", name);
                return (int)(long)command.ExecuteScalar();
            }
        }
        public User GetUser(int groupID)
        {
            User user = null;
            using (var con = new SqliteConnection("Data Source=Library.sqlite"))
            {
                con.Open();

                string stm = "SELECT * From UserList Where Id= @Id";

                using (SqliteCommand cmd = new SqliteCommand(stm, con))
                {
                    cmd.Parameters.AddWithValue("@Id", groupID);
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            //add items to your list
                            int UserId = (int)(long)rdr["Id"];
                            string UserName = (string) rdr["UserName"];
                            int NumberBook = (int)(long)rdr["numberBook"];
                            user = new User(){UserId=UserId,Name= UserName, NumberBook = NumberBook};
                        }
                    }

                } con.Close();

            }
            return user;
        }

    }

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int NumberBook { get; set; }
    }
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string? User { get; set; }
    }
}
