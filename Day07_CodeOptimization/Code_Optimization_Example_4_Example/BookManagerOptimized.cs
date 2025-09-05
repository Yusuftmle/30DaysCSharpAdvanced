using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Optimization_Example_4_Example
{
    public class BookManagerOptimized
    {
        private List<Book> books;

        public BookManagerOptimized(List<Book> books)
        {
            this.books = books;
        }
        //added case insensitivity
        public bool CheckIfBookExists(string title)
        {
            return books.Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
        // Returning IEnumerable<Book> directly is enough since .Where() already yields IEnumerable<Book>.
        // No need to call .ToList() unless we explicitly want to materialize the query into a concrete list.
        public IEnumerable<Book> GetBooksPublishedAfter(int year)
        {
            return books.Where(b => b.Year > year);
        }
        public Book FindBookByTitle(string title)
        {
            // Optimized for List<T>:
            // List<T>.Find is more efficient since it works directly on List<T> 
            // without the extra abstraction layer of LINQ (no enumerator/delegate overhead).
            return books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Retrieves all books by a specific author (exact match, case-insensitive).
        /// Uses deferred execution - query is not executed until enumerated.
        /// </summary>
        /// <param name="author">The exact author name to filter by</param>
        /// <returns>An IEnumerable of books matching the specified author</returns>
        /// <remarks>
        /// Performance note: Uses Equals() for exact matching which is more efficient 
        /// than Contains() and avoids false positives like partial name matches.
        /// </remarks>
        public IEnumerable<Book> GetBooksByAuthor(string author)
        {

            return books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if there are any books by the specified author in the collection.
        /// Uses early exit strategy for optimal performance - stops at first match.
        /// </summary>
        /// <param name="author">The exact author name to search for (case-insensitive)</param>
        /// <returns>True if at least one book by the author exists, false otherwise</returns>
        /// <remarks>
        /// Performance: Uses Exists() method which stops execution immediately upon finding 
        /// the first matching book, rather than counting all matches like Count() > 0.
        /// For large collections, this provides significant performance benefits.
        /// Time Complexity: O(n) worst case, O(1) best case when match is found early.
        /// </remarks>
        public bool AreThereAnyBooksByAuthor(string author)
        {
            // Alternative LINQ approach (slightly less performant due to abstraction overhead):
            // return books.Any(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
            return books.Exists(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)); // List<T>.Exists() - Native method like "find" // Direkt array access
            //return books.Any(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)); // LINQ Any() - Extension method  IEnumerable abstraction
        }

        public Book GetSingleBookByTitle(string title)
        {
            return books.SingleOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Gets the book that was published earliest (minimum publication year).
        /// Uses MinBy() for optimal performance with single-pass iteration.
        /// </summary>
        /// <returns>The book with the earliest publication year, or null if collection is empty</returns>
        /// <remarks>
        /// Performance: MinBy() uses O(n) time complexity with constant memory usage.
        /// This is significantly more efficient than OrderBy().First() which would require 
        /// O(n log n) time complexity due to full sorting operation.
        /// Available in .NET 6+ - designed specifically for "find minimum by criteria" scenarios.
        /// </remarks>
        public Book GetFirstPublishedBook()
        {
            return books.MinBy(b => b.Year);
        }

    }
}
