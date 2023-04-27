using System.Linq.Expressions;

namespace CSC2037_SportsPro_Ch15.Models
{
    public class QueryOptions<T>
    {
        // public properties for sorting, filtering, and paging
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, bool>> Where { get; set; } = null!;

        /* Code for working with Include strings */
        // private backing field for property and method 
        private string[] includes = Array.Empty<string>();

        // public write-only property for Include strings – converts comma-separated string to array 
        // and stores in private backing field
        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        // public get method for Include strings - returns private backing field or empty string array 
        // if private backing field is null
        public string[] GetIncludes() => includes ?? new string[0];

        // read-only properties 
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}
