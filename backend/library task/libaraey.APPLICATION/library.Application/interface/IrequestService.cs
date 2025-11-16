using libaraey.APPLICATION.library.Application.dto;

namespace libaraey.APPLICATION.library.Application
{
    public interface IrequestService
    {
        bool CancelRequest(int requestId, int memberId);
        Task<bool> CreateRequest(requestDTO req);
        IEnumerable<requestDTO> GetAllRequests();
        bool ApproveRequest(int requestId, string ended);
        IEnumerable<requestDTO> GetMemberRequests(int memberId);
    }
}
