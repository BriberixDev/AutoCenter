using AutoCenter.Web.Dtos.Review;
using AutoCenter.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Models;

namespace AutoCenter.Web.Services.Reviews
{
    public class ReviewService : IReviewRepository
    {
        private readonly AutoCenterDbContext _context;
        public ReviewService(AutoCenterDbContext context)
        {
            _context = context;
        }
        public async Task AddReviewAsync(string authorId, string targetUserId, ReviewCreateDto dto, CancellationToken ct = default)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (await ExistsAsync(authorId, targetUserId, ct))
                throw new InvalidOperationException("Review already exists.");

            if (string.IsNullOrWhiteSpace(authorId))
            {
                throw new ArgumentException("You need to be logged in.", nameof(authorId));
            }
            if (string.IsNullOrWhiteSpace(targetUserId))
            {
                throw new ArgumentException("Target user id is required.", nameof(targetUserId));
            }
            var targetUserExists = await _context.Users.AnyAsync(u => u.Id == targetUserId, ct);
            if (!targetUserExists)
                throw new KeyNotFoundException("Target user not found.");
            var rating = dto.Rating;
            var comment = dto.Comment?.Trim();
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Rating must be between 1 and 5.");
            }
            var review = new Review
            {
                AuthorId = authorId,
                TargetUserId = targetUserId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            await _context.Set<Review>().AddAsync(review, ct);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<bool> ExistsAsync(string authorId, string targetUserId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(authorId))
            {
                throw new ArgumentException("Author id is required.", nameof(authorId));
            }
            if (string.IsNullOrWhiteSpace(targetUserId))
            {
                throw new ArgumentException("Target user id is required.", nameof(targetUserId));
            }
            return await _context.Set<Review>().AnyAsync(r => r.AuthorId == authorId && r.TargetUserId == targetUserId && !r.IsDeleted, ct);
        }
        public async Task<IReadOnlyList<ReviewViewDto>> GetReviewsById(string targetUserId, CancellationToken ct = default)
        {
            return await _context.Set<Review>()
                .Where(r => r.TargetUserId == targetUserId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewViewDto
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    AuthorId = r.AuthorId
                })
                .ToListAsync(ct);
        }
    }
}