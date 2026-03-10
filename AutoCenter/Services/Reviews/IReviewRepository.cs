using AutoCenter.Web.Dtos.Review;

namespace AutoCenter.Web.Services.Reviews
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(string authorId, string targetUserId, ReviewCreateDto dto, CancellationToken ct = default);
        Task<bool> ExistsAsync(string authorId, string targetUserId, CancellationToken ct = default);
        Task<IReadOnlyList<ReviewViewDto>> GetReviewsById(string targetUserId, CancellationToken ct= default);
    }
}
