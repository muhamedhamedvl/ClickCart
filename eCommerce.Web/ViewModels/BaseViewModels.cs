using eCommerce.Entities;
using eCommerce.Shared.Enums;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Web.ViewModels
{
    public class PageViewModel
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageURL { get; set; }
        public string PageImageURL { get; set; }

        public List<string> PageCanonicalURLs { get; set; } //= new List<string>();

        public PageViewModel()
        {
            PageCanonicalURLs = new List<string>();
        }
    }

    public class CommentablePageViewModel : PageViewModel
    {
        public int EntityID { get; set; }
        public int RecordID { get; set; }

        public List<Comment> Comments { get; set; }
    }

    /// <summary>
    /// http://jasonwatmore.com/post/2015/10/30/aspnet-mvc-pagination-example-with-logic-like-google
    /// </summary>
    public class Pager
    {
        public Pager(
            int totalItems,
            int? page = 1,
            int recordSize = 10,
            int maxPages = 10)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)recordSize);

            // ensure current page isn't out of range
            if (!page.HasValue || page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (page <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (page + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = page.Value - maxPagesBeforeCurrentPage;
                    endPage = page.Value + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (page.Value - 1) * recordSize;
            var endIndex = Math.Min(startIndex + recordSize - 1, totalItems - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage);

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            CurrentPage = page.Value;
            PageSize = recordSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Pages = pages;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public IEnumerable<int> Pages { get; private set; }
    }
}