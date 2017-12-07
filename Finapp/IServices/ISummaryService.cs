using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ISummaryService
    {
        bool CreateSummary(Summary summary);
        Summary GetSummaryByAssociate(Associate associate);
        IEnumerable<Summary> GetAllSummaries();
    }
}
