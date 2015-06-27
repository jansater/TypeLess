
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeLess.Net.Entity
{
    public class QueryOptions
    {
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public EFMergeOptions MergeOption { get; set; }
        public List<String> Includes { get; private set; }
        

        public QueryOptions()
        {
            this.SortBy = null;
            this.SortAscending = false;
            this.Page = 0;
            this.PageSize = 0;
            MergeOption = EFMergeOptions.AppendOnly;
            this.Includes = new List<string>();
        }

        public QueryOptions (string sortBy, bool sortAscending = true, int page = 1, int pageSize = 30)
        {
            if (sortBy == null) {
                throw new ArgumentNullException("Sort by is required");
            }
            this.SortBy = sortBy;
            this.SortAscending = sortAscending;
            this.Page = page;
            this.PageSize = pageSize;
            MergeOption = EFMergeOptions.AppendOnly;
            this.Includes = new List<string>();
        }

        public QueryOptions Include(string include) {
            this.Includes.Add(include);
            return this;
        }

        public QueryOptions Include(params string[] include)
        {
            this.Includes.AddRange(include);
            return this;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return SortBy.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return SortBy.GetHashCode();
        }

    }
}
