using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BizStockContext _context;

        public NotificationRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id)
                ?? throw new KeyNotFoundException("Notification not found.");
        }

        public async Task<PaginatedList<Notification>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Notifications.OrderByDescending(n => n.Id);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Notification>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate)
        {
            return await _context.Notifications
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId)
        {
            return await _context.Notifications
                .Where(n => n.RecipientId == recipientId)
                .OrderByDescending(n => n.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByRecipientAsync(Guid recipientId)
        {
            return await _context.Notifications
                .Where(n => n.RecipientId == recipientId && !n.IsRead)
                .OrderByDescending(n => n.Id)
                .Take(10)
                .ToListAsync();
        }

        public async Task<int> CountUnreadByRecipientAsync(Guid recipientId)
        {
            return await _context.Notifications
                .CountAsync(n => n.RecipientId == recipientId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.MarkAsRead();
            }

            await Task.CompletedTask;
        }

        public async Task MarkAllAsReadAsync(Guid recipientId)
        {
            var unread = await _context.Notifications
                .Where(n => n.RecipientId == recipientId && !n.IsRead)
                .ToListAsync();

            foreach (var n in unread)
            {
                n.MarkAsRead();
            }

            await Task.CompletedTask;
        }

        public async Task<PaginatedList<Notification>> GetByRecipientPagedAsync(Guid recipientId, PageRequest pageRequest)
        {
            var query = _context.Notifications
                .Where(n => n.RecipientId == recipientId)
                .OrderByDescending(n => n.DateCreated);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Notification>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }

            await Task.CompletedTask;
        }

        public async Task<Notification> GetByExpression(Expression<Func<Notification, bool>> predicate)
        {
            return await _context.Notifications.FirstOrDefaultAsync(predicate) ??
               throw new ArgumentException("Notification Not Found");
        }
    }

}
