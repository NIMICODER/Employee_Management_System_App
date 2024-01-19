using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Models.Utility
{
    /// <summary>
    /// Pagination and filtering query parameters object
    /// </summary>
    public class RequestParameters
    {
        /// <summary>
        /// The maximum page size to return.
        /// </summary>
        const int maxPageSize = 50;

        /// <summary>
        /// Page number 1-2, 2-3
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Default keyword if non was passed from the request
        /// </summary>
        private string _keyword = string.Empty;

        /// <summary>
        /// Seach by keyword or filtering result by search query string.
        /// </summary>
        public string Keywords
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = (string.IsNullOrWhiteSpace(value) ? _keyword : value);
            }
        }

        /// <summary>
        /// Default page size if non was passed from the request
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// Get and Set the pageSize to return. Return MaxPageSize <see cref="maxPageSize"/> if the page size from the request is greater than 10 but defaults to ten if it is less than or equal to ten
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

    }

}
