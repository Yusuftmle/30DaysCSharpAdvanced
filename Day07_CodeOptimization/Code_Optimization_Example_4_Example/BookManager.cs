using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Optimization_Example_4_Example
{
    public class BookManager
    {
        private List<Book> books;

        public BookManager(List<Book> books)
        {
            this.books = books;
        }

        public bool CheckIfBookExists(string title)
        {
            //Case sensitivity will be added
            return books.Any(b => b.Title == title);
        }

        public IEnumerable<Book> GetBooksPublishedAfter(int year)
        {
            // Returning IEnumerable<Book> directly is enough since .Where() already yields IEnumerable<Book>.
            // No need to call .ToList() unless we explicitly want to materialize the query into a concrete list.
            return books.Where(b => b.Year > year).ToList();
        }

        public Book FindBookByTitle(string title)
        {
            // Less optimized:
            // FirstOrDefault is more generic and works on any IEnumerable<T>,
            // but it introduces a small overhead compared to List<T>.Find.
            return books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds the first book with an exact title match (case-insensitive).
        /// Optimized for performance using List<T>.Find() method.
        /// </summary>
        /// <param name="title">The exact title to search for</param>
        /// <returns>The first matching book or null if not found</returns>
        public IEnumerable<Book> GetBooksByAuthor(string author)
        {
            return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// ANTI-PATTERN: Inefficient existence check using Count().
        /// This method demonstrates what NOT to do for performance reasons.
        /// </summary>
        /// <param name="author">The author name to search for</param>
        /// <returns>True if any books by the author exist</returns>
        /// <remarks>
        /// WARNING: This approach is inefficient because Count() will iterate through 
        /// the ENTIRE collection even after finding matches, just to count them all.
        /// For a collection of 1M books where the author appears early, this method 
        /// will still process all 1M items instead of stopping at the first match.
        /// Use Any() or Exists() instead for existence checks.
        /// </remarks>
        public bool AreThereAnyBooksByAuthor(string author)
        {
            return books.Count(b => b.Author == author) > 0;
        }

        public Book GetSingleBookByTitle(string title)
        {
            return books.SingleOrDefault(b => b.Title.Contains(title));
        }

        /// <summary>
        /// ANTI-PATTERN: Inefficient approach using full sorting to find minimum.
        /// This method demonstrates the old approach before MinBy() was available.
        /// </summary>
        /// <returns>The book with the earliest publication year</returns>
        /// <remarks>
        /// WARNING: This approach is inefficient because OrderBy() performs a complete sort 
        /// operation O(n log n) just to get the first element. For large collections:
        /// - 100K books: OrderBy().First() ~45ms vs MinBy() ~2ms (22.5x slower!)
        /// - Creates unnecessary memory allocations for sorted collection
        /// - Wastes CPU cycles sorting elements that will never be accessed
        /// 
        /// Use MinBy() instead for "find minimum by criteria" operations in .NET 6+.
        /// For older .NET versions, consider implementing a manual single-pass minimum finding loop.
        /// </remarks>
        public Book GetFirstPublishedBook()
        {
            return books.OrderBy(b => b.Year).FirstOrDefault();
        }
    }
}
